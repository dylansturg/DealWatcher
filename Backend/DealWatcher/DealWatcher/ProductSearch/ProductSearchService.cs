using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DealWatcher.Models;
using DealWatcher.ProductSearch.ProductSource;
using WebGrease.Css.Extensions;

namespace DealWatcher.ProductSearch
{
    public class ProductSearchService
    {
        protected static IEnumerable<IProductSource> ProductSources = new List<IProductSource>()
        {
            new AmazonProductSource()
        };
        
        public async static Task<IEnumerable<Product>> SearchAsync(DealWatcherService_dbEntities entities, ProductSearchBindingModel search)
        {
            var searchModel = new ProductSearchViewModel()
            {
                Keywords = search.Keywords,
                ProductName = search.ProductName,
                ProductCode = search.ProductCode,
                ProductCodeTypeId = search.ProductCodeTypeId,
                ProductCodeType = search.ProductCodeTypeId != null ? entities.ProductCodeTypes.Find(search.ProductCodeTypeId).Type : null
            };

            var searchResults = new ConcurrentBag<Product>();
            var searchTasks = ProductSources.Select<IProductSource, Task>(productSource => Task.Factory.StartNew(async () =>
            {
                var products = await productSource.SearchAsync(entities, searchModel); 
                products.ForEach(prod => searchResults.Add(prod));
            })).ToList();

            await Task.WhenAll(searchTasks);

            return searchResults;
        }

        public async static Task<IEnumerable<ProductPrice>> SearchProductPricesAsync(DealWatcherService_dbEntities entities, Product search)
        {
            return null;
        }
    }
}