using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Linq;
using DealWatcher.Models;
using Configuration = DealWatcher.Models.Configuration;

namespace DealWatcher.ConfigurationManagement
{
    public class RemoteConfigurationManager
    {
        private static TimeSpan _cacheduration = TimeSpan.Zero;
        private static TimeSpan CachedConfigLifetime
        {
            get
            {
                if (_cacheduration == TimeSpan.Zero)
                {
                    var minutes = Int16.Parse(ConfigurationManager.AppSettings["ConfigRefreshAfterMinutes"]);
                    _cacheduration = TimeSpan.FromMinutes(minutes);
                }
                return _cacheduration;
            }
        }

        private static readonly IDictionary<Type, Type> _typesToConvert = new Dictionary<Type, Type>
        {
            {typeof(Int64), typeof(Int32)},
            {typeof(Int16), typeof(Int32)}
        };

        private static RemoteConfigurationManager _instance;
        public static RemoteConfigurationManager Configuration
        {
            get { return _instance ?? (_instance = new RemoteConfigurationManager(new DealWatcherService_dbEntities())); }
        }

        private readonly DealWatcherService_dbEntities _entities;
        private DateTime _lastConfigRefresh = DateTime.UtcNow;
        private IEnumerable<Configuration> _configurations;
        private IEnumerable<Configuration> Configurations
        {
            get
            {
                if (DateTime.UtcNow > _lastConfigRefresh.Add(CachedConfigLifetime))
                {
                    _configurations = _entities.Configurations.ToList();
                    _lastConfigRefresh = DateTime.UtcNow;
                }
                return _configurations;
            }
            set
            {
                _configurations = value;
                _lastConfigRefresh = DateTime.UtcNow;
            }
        }

        private IEnumerable<ConfigurationValueType> _configurationTypes;

        private RemoteConfigurationManager(DealWatcherService_dbEntities entities)
        {
            _entities = entities;
            Configurations = entities.Configurations.ToList();
            _configurationTypes = entities.ConfigurationValueTypes.ToList();
        }

        public void RefreshConfigurations()
        {
            // Defers the update until the next time the config is accessed
            _lastConfigRefresh = DateTime.MinValue.ToUniversalTime();
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
            var nontypedResult = FetchConfig(key, typeof(T));
            try
            {
                return (T)nontypedResult;
            }
            catch (Exception)
            {
                throw new InvalidConfigurationException(nontypedResult.GetType().FullName, typeof(T));
            }
        }

        public Object FetchConfig(String key)
        {
            return FetchConfig(key, null);
        }

        public Object FetchConfig(String key, Type checkType)
        {
            var config = QueryConfig(key);
            if (config == null)
            {
                throw new InvalidConfigurationException("Failed to find a configuration with the given key.");
            }

            if (checkType != null && config.ConfigurationValueType.Class != checkType.FullName)
            {
                throw new InvalidConfigurationException(config.ConfigurationValueType.Class, checkType);
            }

            return ConfigurationValueFactory.ConstructValue(config.Value);
        }

        public Object BytesToConfig(byte[] bytes)
        {
            return ConfigurationValueFactory.ConstructValue(bytes);
        }

        public void SaveConfig<T>(T value, String key)
        {
            if (value == null)
            {
                throw new InvalidConfigurationException("Attempting to save NULL");
            }
            var config = QueryConfig(key);
            if (config == null)
            {
                config = new Configuration
                {
                    Key = key
                };
                _entities.Configurations.Add(config);
            }

            object savingValue = value;
            if (_typesToConvert.ContainsKey(value.GetType()))
            {
                var convertTo = _typesToConvert[value.GetType()];
                var converter = TypeDescriptor.GetConverter(value.GetType());
                try
                {
                    savingValue = converter.ConvertTo(value, convertTo);
                }
                catch (NotSupportedException)
                {
                    savingValue = null;
                }

                if (savingValue == null)
                {
                    throw new InvalidConfigurationException(String.Format("Cannot utilize Type {0} as Configuration value.", value.GetType().FullName));
                }
            }

            var valType = savingValue.GetType().FullName;
            var configType = _configurationTypes.FirstOrDefault(t => t.Class == savingValue.GetType().FullName);
            if (configType == null)
            {
                throw new InvalidConfigurationException(
                    "Failed to find an appropriate ConfigurationValueType of Type " + savingValue.GetType().FullName);
            }

            config.TypeId = configType.Id;
            config.Value = ConfigurationValueFactory.SerializeConfigBytes(savingValue);
            _entities.SaveChanges();

            RefreshConfigurations();
        }

        private Configuration QueryConfig(String key)
        {
            return Configurations.FirstOrDefault(c => c.Key.Equals(key, StringComparison.CurrentCultureIgnoreCase));
        }
    }
}