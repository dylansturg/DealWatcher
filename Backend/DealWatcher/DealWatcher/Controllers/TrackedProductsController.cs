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
using Microsoft.AspNet.Identity;

namespace DealWatcher.Controllers
{
    [Authorize]
    [RoutePrefix("api/trackedproducts")]
    public class TrackedProductsController : ApiController
    {
        private DealWatcherService_dbEntities db = new DealWatcherService_dbEntities();

        // GET: api/TrackedProducts
        [ResponseType(typeof(IEnumerable<ProductViewModel>))]
        public async Task<IHttpActionResult> GetTrackedProducts()
        {
            User localUser = null;
            try
            {
                localUser = await GetLocalUserForRequest();
            }
            catch (ArgumentException)
            {
                return NotFound();
            }

            var userTracked = db.TrackedProducts.Where(tp => tp.UserId == localUser.Id && tp.StoppedTracking == null);
            var products = userTracked.Select(tp => tp.Product);

            return Ok(Mapper.Map(await products.ToListAsync(), new List<ProductViewModel>()));
        }

        [Route("track")]
        [HttpGet]
        public async Task<IHttpActionResult> TrackProduct(int productId)
        {
            User localUser = null;
            try
            {
                localUser = await GetLocalUserForRequest();
            }
            catch (ArgumentException)
            {
                return NotFound();
            }

            var product = await db.Products.FindAsync(productId);
            if (product == null)
            {
                return NotFound();
            }

            var existing =
                await
                    db.TrackedProducts.FirstOrDefaultAsync(tp => tp.ProductId == productId && tp.UserId == localUser.Id);
            if (existing != null)
            {
                if (existing.StoppedTracking == null)
                {
                    return StatusCode(HttpStatusCode.NotModified);
                }

                existing.StartedTracking = DateTimeOffset.UtcNow;
                existing.StoppedTracking = null;
                await db.SaveChangesAsync();
                return Ok();
            }

            var userTracked = new TrackedProduct
            {
                UserId = localUser.Id,
                ProductId = productId,
                StartedTracking = DateTimeOffset.UtcNow,
            };

            db.TrackedProducts.Add(userTracked);
            await db.SaveChangesAsync();
            return Ok();
        }

        
        [Route("stoptracking")]
        [HttpDelete]
        public async Task<IHttpActionResult> DeleteTrackedProduct(int productId)
        {
            User localUser = null;
            try
            {
                localUser = await GetLocalUserForRequest();
            }
            catch (ArgumentException)
            {
                return NotFound();
            }

            var trackedProduct =
                await
                    db.TrackedProducts.FirstOrDefaultAsync(tp => tp.ProductId == productId && tp.UserId == localUser.Id);
            if (trackedProduct == null || trackedProduct.StoppedTracking != null)
            {
                return NotFound();
            }

            trackedProduct.StoppedTracking = DateTimeOffset.UtcNow;
            await db.SaveChangesAsync();
            return Ok();
        }

        private async Task<User> GetLocalUserForRequest()
        {
            User result = null;

            if (User != null && User.Identity != null)
            {
                var userId = User.Identity.GetUserId();
                if (!String.IsNullOrEmpty(userId))
                {
                    result = await db.Users.FirstOrDefaultAsync(u => u.AspNetUserId == userId);
                }
            }

            if (result == null)
            {
                throw new ArgumentNullException();
            }

            return result;

        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool TrackedProductExists(int id)
        {
            return db.TrackedProducts.Count(e => e.Id == id) > 0;
        }
    }
}