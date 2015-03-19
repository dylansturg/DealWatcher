using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Description;
using AutoMapper;
using DealWatcher.Models;

namespace DealWatcher.Controllers
{
    [Authorize]
    public class ProductPricesController : ApiController
    {
        private DealWatcherService_dbEntities db = new DealWatcherService_dbEntities();

        // GET: api/ProductPrices/5
        /// <summary>
        /// Get all ProductPrice instances associated with the Product with the given Product Id.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [ResponseType(typeof(List<ProductPriceViewModel>))]
        public async Task<IHttpActionResult> GetProductPrice(int id)
        {
            var targetProduct = await db.Products.FirstOrDefaultAsync(p => p.Id == id);
            if (targetProduct == null)
            {
                return NotFound();
            }

            var prices = targetProduct.ProductPrices.ToList();
            var viewModels = Mapper.Map(prices, new List<ProductPriceViewModel>());
            return Ok(viewModels);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool ProductPriceExists(int id)
        {
            return db.ProductPrices.Count(e => e.Id == id) > 0;
        }
    }
}