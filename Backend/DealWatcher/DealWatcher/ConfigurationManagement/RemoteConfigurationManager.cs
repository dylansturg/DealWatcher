using System;
using System.Linq;
using DealWatcher.Models;

namespace DealWatcher.ConfigurationManagement
{
    public class RemoteConfigurationManager
    {
        private static RemoteConfigurationManager _instance;
        public static RemoteConfigurationManager Configuration
        {
            get { return _instance ?? (_instance = new RemoteConfigurationManager(new DealWatcherService_dbEntities())); }
        }

        private DealWatcherService_dbEntities _entities;
        private RemoteConfigurationManager(DealWatcherService_dbEntities entities)
        {
            _entities = entities;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <exception cref="InvalidConfigurationException">The specified type did not match the configuration or the given key could not be found.</exception>
        /// <returns></returns>
        public T FetchConfig<T>(String key)
        {
            var config = QueryConfig(key);
            if (config == null)
            {
                throw new InvalidConfigurationException("Failed to find a configuration with the given key.");
            }

            if (config.ConfigurationValueType.Class != typeof (T).FullName)
            {
                throw new InvalidConfigurationException(config.ConfigurationValueType.Class, typeof(T));
            }

            var parsedResult = ConfigurationValueFactory.ConstructValue(config.Value);
            try
            {
                return (T) parsedResult;
            }
            catch (Exception)
            {
                throw new InvalidConfigurationException(parsedResult.GetType().FullName, typeof(T));
            }
        }

        public void SaveConfig<T>(T value, String key)
        {
            var config = QueryConfig(key);
            if (config == null)
            {
                config = new Models.Configuration()
                {
                    Key = key,
                };
                _entities.Configurations.Add(config);
            }

            var configType = _entities.ConfigurationValueTypes.FirstOrDefault(t => t.Class == typeof (T).FullName);
            if (configType == null)
            {
                throw new InvalidConfigurationException(
                    "Failed to find an appropriate ConfigurationValueType of Type " + typeof (T).FullName);
            }

            config.TypeId = configType.Id;
            config.Value = ConfigurationValueFactory.SerializeConfigBytes(value);
            _entities.SaveChanges();
        }

        private Models.Configuration QueryConfig(String key)
        {
            return _entities.Configurations.FirstOrDefault(c => c.Key == key);
        }
    }
}