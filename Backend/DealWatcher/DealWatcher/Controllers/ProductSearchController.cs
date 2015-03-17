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
using System.Web.Http.Description;

namespace DealWatcher.Controllers
{
    [Authorize]
    public class ProductSearchController : ApiController
    {
        private DealWatcherService_dbEntities db = new DealWatcherService_dbEntities();

        // POST: api/ProductSearch
        [ResponseType(typeof(IEnumerable<ProductViewModel>))]
        public async Task<IHttpActionResult> Post(ProductSearchBindingModel value)
        {
            if (ModelState.IsValid)
            {
                var searchResults = await ProductSearchService.SearchAsync(db, value);
                return Ok(Mapper.Map(searchResults, new List<ProductViewModel>()));
            }

            return new System.Web.Http.Results.BadRequestResult(this);
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
