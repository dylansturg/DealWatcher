using System;
using System.Collections.Generic;
using System.Linq;
using DealWatcher.Models;
using DealWatcher.ProductSearch.ProductSource.Amazon;
using WebGrease.Css.Extensions;

namespace DealWatcher.ProductSearch.ProductSource
{
    public class AmazonProductSource : ProductSourceBase
    {
        public const string AmazonCodeType = "ASIN";
        public const string AmazonSellerName = "Amazon";
        public const int DisplayNameLength = 100;

        protected ProductCodeType AmazonProductCodeType { get; private set; }
        protected Seller AmazonSeller { get; private set; }

        protected override Seller ProductSeller(DealWatcherService_dbEntities entities)
        {
            return AmazonSeller ?? (AmazonSeller = entities.Sellers.First(seller => seller.Name == AmazonSellerName));
        }

        protected override ProductCodeType SellersProductCodeType(DealWatcherService_dbEntities entities)
        {
            return AmazonProductCodeType ??
                   (AmazonProductCodeType = entities.ProductCodeTypes.First(code => code.Type == AmazonCodeType));
        }

        protected override Product GetEquivalentOrCreateProduct(DealWatcherService_dbEntities entities, IApiProduct apiProduct)
        {
            var amazonsCode = AmazonProductCodeType ??
                              (AmazonProductCodeType =
                                  entities.ProductCodeTypes.FirstOrDefault(type => type.Type == AmazonCodeType));
            if (amazonsCode == null)
            {
                throw new ProductSearchException(
                    String.Format("AmazonProductSource failed to find ProductCodeType for {0} (Amazon)",
                        AmazonCodeType));
            }

            var amazonProduct = apiProduct as AmazonProduct;
            if (amazonProduct == null)
            {
                throw new ProductSearchException(
                    "AmazonProductSource expected ApiProducts of type AmazonProduct, received " +
                    apiProduct.GetType().FullName);
            }

            var existingProduct =
                entities.ProductCodes.AsNoTracking().Where(
                    code => code.Code == amazonProduct.ASIN && code.ProductCodeType.Id == amazonsCode.Id)
                    .Select(code => code.Product);
            if (existingProduct.Any())
            {
                return existingProduct.First();
            }

            var product = new Product
            {
                DisplayName = amazonProduct.Title.Length <= DisplayNameLength ?
                    amazonProduct.Title : amazonProduct.Title.Substring(0, DisplayNameLength - 3) + "..."
            };
            return product;
        }

        protected override void AppendNewProductCodes(DealWatcherService_dbEntities entities, Product baseProduct, IApiProduct apiProduct)
        {
            var existingCodes = baseProduct.ProductCodes.ToList();
            var amazonProduct = apiProduct as AmazonProduct;
            if (amazonProduct == null)
            {
                throw new ProductSearchException(
                    "AmazonProductSource expected ApiProducts of type AmazonProduct, received " +
                    apiProduct.GetType().FullName);
            }

            var foundCodes = new Dictionary<String, String> { { amazonProduct.ASIN, AmazonCodeType } };
            amazonProduct.EANs.ForEach(ean => foundCodes.Add(ean, "EAN"));
            amazonProduct.UPCs.ForEach(upc => foundCodes.Add(upc, "UPC"));

            foreach (var productCode in foundCodes)
            {
                if (existingCodes.Any(
                    code => code.Code == productCode.Key && code.ProductCodeType.Type == productCode.Value))
                {
                    // Ignore ig already exists
                    continue;
                }
                var codeType = entities.ProductCodeTypes.FirstOrDefault(type => type.Type == productCode.Value);
                if (codeType == null)
                {
                    throw new ProductSearchException(
                        String.Format("AmazonProductSource failed to find ProductCodeType for Type {0}",
                            productCode.Value));
                }

                var createdProductCode = new ProductCode
                {
                    Code = productCode.Key,
                    ProductCodeType = codeType,
                    Product = baseProduct,
                    TypeId = codeType.Id
                };
                baseProduct.ProductCodes.Add(createdProductCode);
            }
        }

        protected override void AppendNewProductPrices(DealWatcherService_dbEntities entities, Product baseProduct, IApiProduct apiProduct)
        {
            var seller = ProductSeller(entities);
            var amazonProduct = apiProduct as AmazonProduct;
            if (amazonProduct == null)
            {
                throw new ProductSearchException("Invalid IApiProduct for AppendNewProductPrices in AmazonProductSource");
            }

            var cacheDuration = ProductCacheLifetime(entities);
            var existingMatchingPrice = baseProduct.ProductPrices.FirstOrDefault(price =>
            {
                var matches = price.SellerId == seller.Id;
                matches &= price.Price == (Decimal) amazonProduct.Price;
                matches &= price.Gathered.UtcDateTime > DateTime.UtcNow.Subtract(cacheDuration);
                return matches;
            });

            if (existingMatchingPrice != null)
            {
                return;
            }

            var foundPrice = new ProductPrice
            {
                Current = true,
                Gathered = DateTime.Now,
                LocationUrl = amazonProduct.DetailsUrl,
                Price = (Decimal)amazonProduct.Price,
                Product = baseProduct,
                Seller = seller,
                SellerId = seller.Id
            };

            baseProduct.ProductPrices.Add(foundPrice);
        }

        protected override void AppendNewProductImages(DealWatcherService_dbEntities entities, Product baseProduct, IApiProduct apiProduct)
        {
            var amazonProduct = apiProduct as AmazonProduct;
            if (amazonProduct == null)
            {
                throw new ProductSearchException("AppendNewProductImages received invalid IApiProduct in AmazonProductSource");
            }
            var existingImages = baseProduct.ProductImages.ToList();
            foreach (var image in amazonProduct.ImageUrls.Where(image => existingImages.All(prodImg => prodImg.Url != image)))
            {
                baseProduct.ProductImages.Add(new ProductImage
                {
                    Product = baseProduct,
                    Url = image
                });
            }
        }

        protected override IApiRequest CreateApiRequest(ProductSearchViewModel productSearch)
        {
            return new AmazonRequest(productSearch);
        }
    }
}