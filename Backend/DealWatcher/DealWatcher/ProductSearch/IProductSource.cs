using System.Collections.Generic;
using System.Threading.Tasks;
using DealWatcher.Models;

namespace DealWatcher.ProductSearch
{
    public interface IProductSource
    {
        Task<IEnumerable<Product>> SearchAsync(DealWatcherService_dbEntities db, ProductSearchViewModel search);
    }
}
