using ConfigurationReader.Abstract;
using ConfigurationReader.Context;
using ConfigurationReader.Helpers;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using ConfigurationReader.Dtos;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Newtonsoft.Json.Linq;
using Microsoft.EntityFrameworkCore.Metadata;

namespace ConfigurationReader.Concrete
{
    public class ConfigurationReader : IConfigurationReader
    {
        private readonly ConfigurationContext _context;
        private readonly string applicationName;
        private readonly int refreshTimerIntervalInMs;
        private readonly ICacheHelper cacheHelper;
        private readonly string ConfigurationReaderListKey = "ConfigurationReaderListKey";

        public ConfigurationReader(string _applicationName, string _connectionString, int _refreshTimerIntervalInMs, ICacheHelper _cacheHelper)
        {
            var optionsBuilder = new DbContextOptionsBuilder<ConfigurationContext>();
            optionsBuilder.UseSqlServer(_connectionString);

            _context = new ConfigurationContext(optionsBuilder.Options);
            applicationName = _applicationName;
            cacheHelper = _cacheHelper;

            var task = Task.Run(async () =>
            {
                for (; ; )
                {
                    await Task.Delay(_refreshTimerIntervalInMs);
                    await RefreshConfigurationCache(ConfigurationReaderListKey);
                }
            });
        }

        public async Task<T> GetAllConfigurations<T>()
        {
            var dataList = new List<ConfigurationDto>();

            if (cacheHelper.IsInCache(ConfigurationReaderListKey))
                dataList = cacheHelper.Get<List<ConfigurationDto>>(ConfigurationReaderListKey);
            else
            {
                try
                {
                    dataList = await _context.Configurations
                        .Where(o => o.IsActive
                            && o.ApplicationName == applicationName)
                        .Select(k => new ConfigurationDto
                        {
                            Id = k.Id,
                            Name = k.Name,
                            Type = k.Type,
                            Value = k.Value,
                            ApplicationName = applicationName,
                            IsActive = k.IsActive
                        }).ToListAsync();

                    SetCacheAndJsonFile(dataList);
                }
                catch(Exception ex) 
                {
                    dataList = JsonHelper.GetGonfigListFromJsonFile();
                }
            }

            var result = TypeHelper.GetValueWithType<T>(JsonConvert.SerializeObject(dataList));

            return result;
        }

        public async Task<T> GetValue<T>(string key)
        {
            var value = string.Empty;
            if (string.IsNullOrEmpty(key))
                return default(T);

            if (cacheHelper.IsInCache(key))
                value = cacheHelper.Get<string>(key);
            else
            {
                try
                {
                    value = await _context.Configurations
                        .Where(o => o.IsActive
                            && o.ApplicationName == applicationName
                            && o.Name == key)
                        .Select(k => k.Value).FirstOrDefaultAsync();

                    if (string.IsNullOrEmpty(value))
                    {
                        Console.WriteLine("ConfigurationReader GetValue error. valueInfo is null");
                        return default;
                    }

                    //set cache
                    cacheHelper.Set(key, value);
                }
                //eğer veritabanında bir problem olmuşsa ve ilgili data cachete yoksa
                //en son kaydedilen appConfig.json dosyası üzerinden işlem yapılır
                catch (Exception ex)
                {
                    var dataList = JsonHelper.GetGonfigListFromJsonFile();

                    value = dataList
                        .Where(o => o.IsActive
                            && o.ApplicationName == applicationName
                            && o.Name == key)
                        .Select(k => k.Value).FirstOrDefault();
                }
            }

            if (string.IsNullOrEmpty(value))
                return default(T);

            var result = TypeHelper.GetValueWithType<T>(JsonConvert.SerializeObject(value));

            return result;
        }

        private async Task RefreshConfigurationCache(string cacheKey)
        {
            try
            {
                var dataList = await _context.Configurations
                .Where(o => o.IsActive
                    && o.ApplicationName == applicationName)
                .Select(k => new ConfigurationDto
                {
                    Id = k.Id,
                    Name = k.Name,
                    Type = k.Type,
                    Value = k.Value,
                    ApplicationName = applicationName,
                    IsActive = k.IsActive
                }).ToListAsync();

                SetCacheAndJsonFile(dataList);
            }
            catch (Exception ex)
            {

            }
        }

        private void SetCacheAndJsonFile(List<ConfigurationDto> dataList)
        {
            //set cache
            cacheHelper.Set(ConfigurationReaderListKey, dataList);

            //set config json file
            JsonHelper.SetAppConfigJsonFileData(dataList);
        }
    }
}
