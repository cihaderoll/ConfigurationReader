using Configuration.Common.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Configuration.Core.Abstract
{
    public interface IConfigurationService
    {
        /// <summary>
        /// Gets all configurations from storage
        /// </summary>
        /// <returns></returns>
        Task<List<ConfigurationDto>> GetAllConfigurations();

        /// <summary>
        /// Adds or updates configuration
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        Task<bool> AddOrUpdateConfiguration(ConfigurationDto data);

        /// <summary>
        /// Sets IsActive prop to false of the given configuration list
        /// </summary>
        /// <param name="configId"></param>
        /// <returns></returns>
        Task<bool> DeleteConfiguration(List<int> configIdList);
    }
}
