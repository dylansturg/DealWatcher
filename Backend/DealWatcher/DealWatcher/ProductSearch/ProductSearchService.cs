using DealWatcher.Models;
using DealWatcher.ProductSearch.ProductSource;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using RefactorThis.GraphDiff;

namespace DealWatcher.ProductSearch
{
    public class ProductSearchService
    {
        protected static IEnumerable<IProductSource> ProductSources = new List<IProductSource>()
        {
            new AmazonProductSource(),
        };
        
        public async static Task<IEnumerable<Product>> SearchAsync(DealWatcherService_dbEntities Entities, ProductSearchBindingModel Search)
        {
            var searchResults = new List<Product>();
            foreach (var productSource in ProductSources)
            {
                var products = await productSource.SearchAsync(Search);
            }

            return searchResults;
        }

        public async static Task<IEnumerable<ProductPrice>> SearchProductPricesAsync(DealWatcherService_dbEntities Entities, Product Search)
        {
            return null;
        }
    }
}