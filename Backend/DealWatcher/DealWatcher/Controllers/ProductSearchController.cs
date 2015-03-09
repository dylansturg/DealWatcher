using DealWatcher.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using DealWatcher.ProductSearch;

namespace DealWatcher.Controllers
{
    public class ProductSearchController : ApiController
    {
        private DealWatcherService_dbEntities db = new DealWatcherService_dbEntities();
        // GET: api/ProductSearch
        public async Task<IEnumerable<Product>> Get()
        {
            return await ProductSearchService.SearchAsync(db, new ProductSearchBindingModel()
            {
                Keywords = "iPad",
                ProductName = "iPad Air 2",
            });
        }

        // GET: api/ProductSearch/5
        public string Get(int id)
        {
            return "value";
        }

        // POST: api/ProductSearch
        public void Post([FromBody]string value)
        {
        }

        // PUT: api/ProductSearch/5
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE: api/ProductSearch/5
        public void Delete(int id)
        {
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }

            base.Dispose(disposing);
        }
    }
}
