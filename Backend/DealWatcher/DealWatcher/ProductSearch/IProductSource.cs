using DealWatcher.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DealWatcher.ProductSearch
{
    interface IProductSource
    {
        public async Task<IEnumerable<Product>> SearchAsync(ProductSearchBindingModel search);
        public async Task<IEnumerable<ProductPrice>> SearchProductPricesAsync(Product search);
    }
}
