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
using DealWatcher.ConfigurationManagement;
using DealWatcher.Models;

namespace DealWatcher.Controllers
{
    public class ConfigurationsController : ApiController
    {
        private DealWatcherService_dbEntities db = new DealWatcherService_dbEntities();

        // GET: api/Configurations
        public IQueryable<Configuration> GetConfigurations()
        {
            return db.Configurations;
        }

        [Route("TestSave")]
        [ResponseType(typeof(Object))]
        public IHttpActionResult GetTestSave()
        {
            RemoteConfigurationManager.Configuration.SaveConfig(
                "A Configuration String that can be changed at run time", "MyFancyKey");

            RemoteConfigurationManager.Configuration.SaveConfig(32, "MyIntKey");
            var intConfig = RemoteConfigurationManager.Configuration.FetchConfig<Int32>("MyIntKey");

            RemoteConfigurationManager.Configuration.SaveConfig(TimeSpan.FromDays(1), "MyTimeSpanKey");
            var timespanConfig = RemoteConfigurationManager.Configuration.FetchConfig<TimeSpan>("MyTimeSpanKey");

            var savedConfig = RemoteConfigurationManager.Configuration.FetchConfig<String>("MyFancyKey");
            return Ok(savedConfig);
        }

        // GET: api/Configurations/5
        [ResponseType(typeof(Configuration))]
        public async Task<IHttpActionResult> GetConfiguration(int id)
        {
            Configuration configuration = await db.Configurations.FindAsync(id);
            if (configuration == null)
            {
                return NotFound();
            }

            return Ok(configuration);
        }

        // PUT: api/Configurations/5
        [ResponseType(typeof(void))]
        public async Task<IHttpActionResult> PutConfiguration(int id, Configuration configuration)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != configuration.Id)
            {
                return BadRequest();
            }

            db.Entry(configuration).State = EntityState.Modified;

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ConfigurationExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return StatusCode(HttpStatusCode.NoContent);
        }

        // POST: api/Configurations
        [ResponseType(typeof(Configuration))]
        public async Task<IHttpActionResult> PostConfiguration(Configuration configuration)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.Configurations.Add(configuration);

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                if (ConfigurationExists(configuration.Id))
                {
                    return Conflict();
                }
                else
                {
                    throw;
                }
            }

            return CreatedAtRoute("DefaultApi", new { id = configuration.Id }, configuration);
        }

        // DELETE: api/Configurations/5
        [ResponseType(typeof(Configuration))]
        public async Task<IHttpActionResult> DeleteConfiguration(int id)
        {
            Configuration configuration = await db.Configurations.FindAsync(id);
            if (configuration == null)
            {
                return NotFound();
            }

            db.Configurations.Remove(configuration);
            await db.SaveChangesAsync();

            return Ok(configuration);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool ConfigurationExists(int id)
        {
            return db.Configurations.Count(e => e.Id == id) > 0;
        }
    }
}