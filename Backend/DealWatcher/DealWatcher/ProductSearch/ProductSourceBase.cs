using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Threading.Tasks;
using DealWatcher.Models;
using DealWatcher.ProductSearch.ProductSource;
using RefactorThis.GraphDiff;

namespace DealWatcher.ProductSearch
{
    public abstract class ProductSourceBase
    {
        protected static TimeSpan DefaultPriceCacheDuration = TimeSpan.FromDays(1);

        public async Task<IEnumerable<Product>> SearchAsync(DealWatcherService_dbEntities db,
            ProductSearchViewModel search)
        {
            var request = CreateApiRequest(search);

            var searchResponses = await request.ExecuteAsync();
            var amazonSearchResults = new List<IApiProduct>();
            if (searchResponses == null || !searchResponses.Any())
            {
                return new List<Product>();
            }

            foreach (var amazonProducts in searchResponses.Where(resp => resp != null).Select(response => response.ParsedProducts))
            {
                amazonSearchResults.AddRange(amazonProducts);
            }

            return ConvertApiProducts(db, amazonSearchResults);
        }


        protected IEnumerable<Product> ConvertApiProducts(DealWatcherService_dbEntities entities,
            IEnumerable<IApiProduct> apiProducts)
        {
            var foundProducts = new Dictionary<String, Product>();
            var savedProductIds = new HashSet<int>();

            var seller = ProductSeller(entities);

            foreach (var apiProduct in apiProducts)
            {
                if (!foundProducts.ContainsKey(apiProduct.UniqueIdentifier))
                {
                    foundProducts.Add(apiProduct.UniqueIdentifier, GetEquivalentOrCreateProduct(entities, apiProduct));
                }

                var product = foundProducts[apiProduct.UniqueIdentifier];
                AppendNewProductCodes(entities, product, apiProduct);
                AppendNewProductImages(entities, product, apiProduct);
                AppendNewProductPrices(entities, product, apiProduct);

                var attachedProduct = entities.UpdateGraph(product,
                    map =>
                        map.OwnedCollection(p => p.ProductCodes)
                            .OwnedCollection(p => p.ProductImages)
                            .OwnedCollection(p => p.ProductPrices));
                entities.SaveChanges();

                if (attachedProduct != null)
                {
                    savedProductIds.Add(attachedProduct.Id);
                }

            }

            return savedProductIds.Select(id => entities.Products.Find(id));
        }

        protected TimeSpan ProductCacheLifetime(DealWatcherService_dbEntities entities)
        {
            var seller = ProductSeller(entities);
            var cacheExpiration = entities.PriceCacheDurations.FirstOrDefault(p => p.SellerId == seller.Id);
            return cacheExpiration != null ? cacheExpiration.CacheLifetme : DefaultPriceCacheDuration;
        }

        protected abstract ProductCodeType SellersProductCodeType(DealWatcherService_dbEntities entities);

        protected abstract Seller ProductSeller(DealWatcherService_dbEntities entities);

        protected abstract Product GetEquivalentOrCreateProduct(DealWatcherService_dbEntities entities,
            IApiProduct apiProduct);

        protected abstract void AppendNewProductCodes(DealWatcherService_dbEntities entities, Product baseProduct,
            IApiProduct apiProduct);

        protected abstract void AppendNewProductPrices(DealWatcherService_dbEntities entities, Product baseProduct,
            IApiProduct apiProduct);

        protected abstract void AppendNewProductImages(DealWatcherService_dbEntities entities, Product baseProduct,
            IApiProduct apiProduct);

        protected abstract IApiRequest CreateApiRequest(ProductSearchViewModel productSearch);
    }
}
