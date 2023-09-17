using ConfigurationReader.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConfigurationReader.Abstract
{
    public interface IConfigurationReader
    {
        /// <summary>
        /// Gets the value of config with the given type
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <returns></returns>
        Task<T> GetValue<T>(string key);

        /// <summary>
        /// Gets all active configurations
        /// </summary>
        /// <returns></returns>
        Task<T> GetAllConfigurations<T>();

        /// <summary>
        /// Refreshes the application cache when an update, delete orr add
        /// operations performed on configuration list
        /// </summary>
        /// <param name="cacheKey"></param>
        //Task RefreshConfigurationCache(string cacheKey);
    }
}
