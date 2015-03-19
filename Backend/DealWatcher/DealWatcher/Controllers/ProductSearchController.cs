using System.Collections.Generic;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Description;
using System.Web.Http.Results;
using AutoMapper;
using DealWatcher.Models;
using DealWatcher.ProductSearch;

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

            return new BadRequestResult(this);
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
