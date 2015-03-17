using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Description;
using AutoMapper;
using DealWatcher.ConfigurationManagement;
using DealWatcher.Models;

namespace DealWatcher.Controllers
{
    [Authorize(Roles = "ConfigurationEditor")]
    public class ConfigurationsController : ApiController
    {
        private DealWatcherService_dbEntities db = new DealWatcherService_dbEntities();

        // GET: api/Configurations
        public IEnumerable<ConfigurationViewModel> GetConfigurations()
        {
            var results = Mapper.Map(db.Configurations.ToList(), new List<ConfigurationViewModel>());
            return results;
        }

        [Route("TestSave")]
        [ResponseType(typeof(Object))]
        public IHttpActionResult GetTestSave()
        {
            RemoteConfigurationManager.Configuration.SaveConfig(
                "A Configuration String that can be changed at run time", "MyFancyKey");

            RemoteConfigurationManager.Configuration.SaveConfig((Int16) 12, "MyIntKey");
            var intConfig = RemoteConfigurationManager.Configuration.FetchConfig<int>("MyIntKey");

            RemoteConfigurationManager.Configuration.SaveConfig(TimeSpan.FromDays(1), "MyTimeSpanKey");
            var timespanConfig = RemoteConfigurationManager.Configuration.FetchConfig<TimeSpan>("MyTimeSpanKey");

            var savedConfig = RemoteConfigurationManager.Configuration.FetchConfig<String>("MyFancyKey");
            return Ok(savedConfig);
        }

        [ResponseType(typeof(ConfigurationViewModel))]
        public IHttpActionResult GetConfiguration(String key)
        {
            try
            {
                var config = RemoteConfigurationManager.Configuration.FetchConfig(key);
                return Ok(new ConfigurationViewModel()
                {
                    Key = key,
                    Value = config,
                    Type = config.GetType().Name,
                });
            }
            catch (InvalidConfigurationException)
            {
                return NotFound();
            }
        }

        // POST: api/Configurations
        [ResponseType(typeof(ConfigurationViewModel))]
        public IHttpActionResult PostConfiguration(ConfigurationViewModel configuration)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            RemoteConfigurationManager.Configuration.SaveConfig(configuration.Value, configuration.Key);
            var savedConfig = RemoteConfigurationManager.Configuration.FetchConfig(configuration.Key);
            return Ok(new ConfigurationViewModel
            {
                Key = configuration.Key,
                Value = savedConfig,
                Type = savedConfig.GetType().Name,
            });
        }

        // DELETE: api/Configurations/5
        public async Task<IHttpActionResult> DeleteConfiguration(String key)
        {
            var configuration = await
                db.Configurations.FirstOrDefaultAsync(c => c.Key.Equals(key, StringComparison.CurrentCultureIgnoreCase));
            if (configuration == null)
            {
                return NotFound();
            }

            db.Configurations.Remove(configuration);
            await db.SaveChangesAsync();
            RemoteConfigurationManager.Configuration.RefreshConfigurations();
            return Ok();
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