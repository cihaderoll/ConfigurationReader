using Configuration.Common.Dtos;
using Configuration.Core.Abstract;
using Configuration.Web.Helpers;
using ConfigurationReader.Abstract;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Configuration.Web.Controllers
{
    public class ConfigurationController : Controller
    {
        private readonly IConfigurationService configurationService;
        private readonly IConfigurationReader configurationReader;
        private readonly ICacheHelper _cacheHelper;

        private readonly string ConfigurationListKey = "ConfigurationList";

        public ConfigurationController(
            IConfigurationService _configurationService, IConfigurationReader _configurationReader, ICacheHelper cacheHelper)
        {
            configurationService = _configurationService;
            configurationReader = _configurationReader;
            _cacheHelper = cacheHelper;
        }

        [HttpGet]
        public async Task<ViewResult> List()
        {
            List<ConfigurationDto> data;

            //config reader örnek kullanımı
            var useCache = await configurationReader.GetValue<bool>("UseCache");

            if (useCache && _cacheHelper.IsInCache(ConfigurationListKey))
                data = _cacheHelper.Get<List<ConfigurationDto>>(ConfigurationListKey);
            else
            {
                data = await GetConfigurationListModelFromDLL();
                SetConfigurationListCache(data);
            }

            return View(data);
        }

        [HttpPost]
        public async Task<JsonResult> AddOrUpdateConfiguration(ConfigurationDto data)
        {
            var result = await configurationService.AddOrUpdateConfiguration(data);

            //config reader örnek kullanımı
            var useCache = await configurationReader.GetValue<bool>("UseCache");

            //if result is true we refresh the cache value
            if (useCache && result && _cacheHelper.IsInCache(ConfigurationListKey))
            {
                var dataList = await GetConfigurationListModelFromDLL();
                SetConfigurationListCache(dataList);
            }

            return new JsonResult(result);
        }

        [HttpPost]
        public async Task<JsonResult> DeleteConfiguration(List<int> configIdList)
        {
            var result = await configurationService.DeleteConfiguration(configIdList);

            //config reader örnek kullanımı
            var useCache = await configurationReader.GetValue<bool>("UseCache");

            //if result is true we refresh the cache value
            if (useCache && result && _cacheHelper.IsInCache(ConfigurationListKey))
            {
                var dataList = await GetConfigurationListModelFromDLL();
                SetConfigurationListCache(dataList);
            }

            return new JsonResult(result);
        }

        private async Task<List<ConfigurationDto>> GetConfigurationListModel()
        {
            var dataList = await configurationService.GetAllConfigurations();

            return dataList;
        }

        /// <summary>
        /// ConfigurationReader üzerinden configuration listesini döner.
        /// </summary>
        /// <returns></returns>
        private async Task<List<ConfigurationDto>> GetConfigurationListModelFromDLL()
        {
            var dataList = await configurationReader.GetAllConfigurations<List<ConfigurationDto>>();

            return dataList;
        }

        private void SetConfigurationListCache(List<ConfigurationDto> dataList)
        {
            _cacheHelper.Set(ConfigurationListKey, dataList);
        }
    }
}
