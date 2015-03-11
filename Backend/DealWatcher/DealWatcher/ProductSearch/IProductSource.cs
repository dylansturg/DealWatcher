using System;
using System.Collections.Generic;
using System.Configuration;
using System.Threading.Tasks;
using DealWatcher.Models;

namespace DealWatcher.ProductSearch
{
    public abstract class ProductSourceBase
    {
        protected static TimeSpan DefaultPriceCacheDuration = TimeSpan.FromDays(1);

        public abstract Task<IEnumerable<Product>> SearchAsync(DealWatcherService_dbEntities db, ProductSearchViewModel search);
    }
}
