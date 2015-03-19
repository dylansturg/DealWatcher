using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using DealWatcher.Models;
using DealWatcher.ProductSearch.ProductSource;
using WebGrease.Css.Extensions;

namespace DealWatcher.ProductSearch
{
    public class ProductSearchService
    {
        protected static IEnumerable<ProductSourceBase> ProductSources = new List<ProductSourceBase>()
        {
            new AmazonProductSource()
        };
        
        public async static Task<IEnumerable<Product>> SearchAsync(DealWatcherService_dbEntities entities, ProductSearchBindingModel search)
        {
            var searchModel = Mapper.Map(search, new ProductSearchViewModel());
            searchModel.ProductCodeType = search.ProductCodeTypeId != null
                ? entities.ProductCodeTypes.Find(search.ProductCodeTypeId).Type
                : null;

            var searchResults = new ConcurrentBag<Product>();
            var searchTasks = ProductSources.Select<ProductSourceBase, Task>(productSource => TaskEx.Run(async () =>
            {
                var products = await productSource.SearchAsync(entities, searchModel); 
                products.ForEach(prod => searchResults.Add(prod));
            })).ToList();

            await Task.WhenAll(searchTasks);

            return searchResults.OrderBy(prod => prod.Id);
        }

        public async static Task<IEnumerable<ProductPrice>> SearchProductPricesAsync(DealWatcherService_dbEntities entities, Product search)
        {
            return null;
        }
    }
}