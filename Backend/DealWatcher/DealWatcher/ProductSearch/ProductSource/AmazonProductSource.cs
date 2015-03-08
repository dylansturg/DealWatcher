using DealWatcher.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace DealWatcher.ProductSearch.ProductSource
{
    public class AmazonProductSource : IProductSource
    {
        public async Task<IEnumerable<Product>> SearchAsync(ProductSearchBindingModel search)
        {
            throw new NotImplementedException();
        }

        public async Task<IEnumerable<ProductPrice>> SearchProductPricesAsync(Product search)
        {
            throw new NotImplementedException();
        }
    }
}