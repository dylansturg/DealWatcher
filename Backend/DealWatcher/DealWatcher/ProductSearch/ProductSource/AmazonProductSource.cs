using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DealWatcher.Models;
using DealWatcher.ProductSearch.ProductSource.Amazon;
using RefactorThis.GraphDiff;
using WebGrease.Css.Extensions;

namespace DealWatcher.ProductSearch.ProductSource
{
    public class AmazonProductSource : IProductSource
    {
        public const string AmazonCodeType = "ASIN";
        public const string AmazonSellerName = "Amazon";
        public const int DisplayNameLength = 100;

        public async Task<IEnumerable<Product>> SearchAsync(DealWatcherService_dbEntities db, ProductSearchViewModel search)
        {
            var request = new AmazonRequest(search);
            var searchResponses = await request.ExecuteAsync();
            var amazonSearchResults = new List<AmazonProduct>();
            foreach (var response in searchResponses)
            {
                var amazonProducts = response.ParsedResults;
                amazonSearchResults.AddRange(amazonProducts);
            }

            return CombineAmazonProducts(db, amazonSearchResults);
        }

        private IEnumerable<Product> CombineAmazonProducts(DealWatcherService_dbEntities db, IEnumerable<AmazonProduct> products)
        {
            var foundProducts = new Dictionary<String, Product>();
            var savedProductIds = new HashSet<int>();

            var amazonSeller = db.Sellers.First(seller => seller.Name == AmazonSellerName);

            foreach (var amazonProduct in products)
            {
                if (!foundProducts.ContainsKey(amazonProduct.ASIN))
                {
                    foundProducts.Add(amazonProduct.ASIN, GetOrCreateProduct(db, amazonProduct));
                }

                var product = foundProducts[amazonProduct.ASIN];
                
                UpdateProductCodes(db, product, amazonProduct);
                AppendFoundPrice(db, amazonProduct, product, amazonSeller);
                UpdateProductImages(product, amazonProduct);

                var attachedProduct = db.UpdateGraph(product,
                    map =>
                        map.OwnedCollection(p => p.ProductCodes)
                            .OwnedCollection(p => p.ProductImages)
                            .OwnedCollection(p => p.ProductPrices));
                db.SaveChanges();

                if (attachedProduct != null)
                {
                    savedProductIds.Add(attachedProduct.Id);
                }

            }

            return savedProductIds.Select(id => db.Products.Find(id));
        }

        private static void UpdateProductImages(Product product, AmazonProduct amazonProduct)
        {
            var existingImages = product.ProductImages.ToList();
            foreach (var image in amazonProduct.ImageUrls)
            {
                if (!existingImages.Any(prodImg => prodImg.Url == image))
                {
                    product.ProductImages.Add(new ProductImage()
                    {
                        Product = product,
                        Url = image,
                        ProductId = product.Id
                    });
                }
            }
        }

        private static void AppendFoundPrice(DealWatcherService_dbEntities db, AmazonProduct amazonProduct, Product product, Seller seller)
        {
            if (product.ProductPrices.Any(price =>
            {
                var matches = true;
                matches &= price.SellerId == seller.Id;
                matches &= price.Price == (Decimal) amazonProduct.Price;
                matches &= price.Gathered > (DateTime.UtcNow.Subtract(TimeSpan.FromHours(1)));
                return matches;
            }))
            {
                return;
            }

            var foundPrice = new ProductPrice()
            {
                Current = true,
                Gathered = DateTime.Now,
                LocationUrl = amazonProduct.DetailsUrl,
                Price = (Decimal) amazonProduct.Price,
                Product = product,
                ProductId = product.Id,
                Seller = seller,
                SellerId = seller.Id
            };
            product.ProductPrices.Add(foundPrice);
        }

        private void UpdateProductCodes(DealWatcherService_dbEntities db, Product product, AmazonProduct amazonProduct)
        {
            var existingCodes = product.ProductCodes.ToList();

            var foundCodes = new Dictionary<String, String> {{amazonProduct.ASIN, AmazonCodeType}};
            amazonProduct.EANs.ForEach(ean => foundCodes.Add(ean, "EAN"));
            amazonProduct.UPCs.ForEach(upc => foundCodes.Add(upc, "UPC"));

            foreach (var productCode in foundCodes)
            {
                if (
                    !existingCodes.Any(
                        code => code.Code == productCode.Key && code.ProductCodeType.Type == productCode.Value))
                {
                    var codeType = db.ProductCodeTypes.FirstOrDefault(type => type.Type == productCode.Value);
                    if (codeType == null)
                    {
                        throw new ProductSearchException(
                            String.Format("AmazonProductSource failed to find ProductCodeType for Type {0}",
                                productCode.Value));
                    }

                    var createdProductCode = new ProductCode()
                    {
                        Code = productCode.Key,
                        ProductCodeType = codeType,
                        Product = product,
                        TypeId = codeType.Id,
                    };
                    product.ProductCodes.Add(createdProductCode);
                }
            }
        }


        private Product GetOrCreateProduct(DealWatcherService_dbEntities db, AmazonProduct amazonProduct)
        {
            var amazonsCode = db.ProductCodeTypes.FirstOrDefault(type => type.Type == AmazonCodeType);
            if (amazonsCode == null)
            {
                throw new ProductSearchException(
                    String.Format("AmazonProductSource failed to find ProductCodeType for {0} (Amazon)",
                        AmazonCodeType));
            }

            var existingProduct =
                db.ProductCodes.AsNoTracking().Where(
                    code => code.Code == amazonProduct.ASIN && code.ProductCodeType.Id == amazonsCode.Id)
                    .Select(code => code.Product);
            if (existingProduct.Any())
            {
                return existingProduct.First();
            }

            var product = new Product()
            {
                DisplayName = amazonProduct.Title.Length <= DisplayNameLength ? 
                    amazonProduct.Title : amazonProduct.Title.Substring(0, DisplayNameLength - 3) + "...",
            };
            return product;
        }
    }
}