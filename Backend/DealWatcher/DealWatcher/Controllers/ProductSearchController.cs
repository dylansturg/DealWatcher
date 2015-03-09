using DealWatcher.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using DealWatcher.ProductSearch;
using AutoMapper;
using System.Data.Entity.Validation;
using System.Diagnostics;
using System.Text;

namespace DealWatcher.Controllers
{
    [Authorize]
    public class ProductSearchController : ApiController
    {
        private DealWatcherService_dbEntities db = new DealWatcherService_dbEntities();
        // GET: api/ProductSearch
        public async Task<IEnumerable<ProductViewModel>> Get()
        {
            try
            {
                var searchResults = await ProductSearchService.SearchAsync(db, new ProductSearchBindingModel()
                {
                    Keywords = "iPad",
                });
                return Mapper.Map(searchResults, new List<ProductViewModel>());
            }
            catch (DbEntityValidationException dbEx)
            {
                StringBuilder errors = new StringBuilder();
                foreach (var validationErrors in dbEx.EntityValidationErrors)
                {
                    foreach (var validationError in validationErrors.ValidationErrors)
                    {
                        errors.Append(String.Format("Property: {0} Error: {1}&", validationError.PropertyName, validationError.ErrorMessage));
                        Trace.TraceInformation("Property: {0} Error: {1}", validationError.PropertyName, validationError.ErrorMessage);
                    }
                }
                String errorMessage = errors.ToString();
                Console.WriteLine(errorMessage);
            }

            return null;
        }

        // POST: api/ProductSearch
        public async Task<IEnumerable<Product>> Post(ProductSearchBindingModel value)
        {

            return null;
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
