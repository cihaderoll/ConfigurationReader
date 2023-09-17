using Configuration.Common.Dtos;
using Configuration.Core.Abstract;
using Configuration.Domain.Abstract;
using Microsoft.EntityFrameworkCore.Diagnostics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Configuration.Core.Concrete
{
    public class ConfigurationService: IConfigurationService
    {
        private IUnitOfWork unitOfWork;

        public ConfigurationService(IUnitOfWork _unitOfWork)
        {
            unitOfWork = _unitOfWork;
        }

        public async Task<List<ConfigurationDto>> GetAllConfigurations()
        {
            var entityList = await unitOfWork.Configurations.GetAllAsync();

            var model = entityList.Select(o => new ConfigurationDto
            {
                Id = o.Id,
                Name = o.Name,
                ApplicationName = o.ApplicationName,
                IsActive = o.IsActive,
                Type = o.Type,
                Value = o.Value
            }).ToList();

            return model;
        }

        public async Task<bool> AddOrUpdateConfiguration(ConfigurationDto data)
        {
            if (await CheckForDuplicateConfigNameAsync(data.Name))
            {
                Console.WriteLine("ConfigurationService AddOrUpdateConfiguration duplicate config name, data.Name -> " + data.Name);
                return false;
            }

            if(data.Id > 0)
            {
                var currentModel = await unitOfWork.Configurations.GetByIdAsync(data.Id);
                if(currentModel == null)
                {
                    Console.WriteLine("ConfigurationService AddOrUpdateConfiguration currentModel is null, data.Id -> " + data.Id);
                    return false;
                }

                currentModel.Name = data.Name;
                currentModel.Type = data.Type;
                currentModel.Value = data.Value;
            }
            else
            {
                var model = new Domain.Models.Configuration
                {
                    Name = data.Name,
                    Type = data.Type,
                    Value = data.Value,
                    IsActive = true
                };

                await unitOfWork.Configurations.AddAsync(model);
            }

            var result = await unitOfWork.SaveChangesAsync();

            return result >= 0;
        }

        public async Task<bool> DeleteConfiguration(List<int> configIdList)
        {
            if (!configIdList.Any())
            {
                Console.WriteLine("ConfigurationService DeleteConfiguration configIdList is empty");
                return false;
            }

            var currentModelList = await unitOfWork.Configurations.GetByIdListAsync(configIdList);
            if (!currentModelList.Any())
            {
                Console.WriteLine("ConfigurationService DeleteConfiguration currentModelList is empty");
                return false;
            }

            foreach (var currentModel in currentModelList)
            {
                currentModel.IsActive = false;
            }

            var result = await unitOfWork.SaveChangesAsync();

            return result >= 0;
        }

        #region Private

        private async Task<bool> CheckForDuplicateConfigNameAsync(string configName)
        {
            return await unitOfWork.Configurations.CheckForDuplicateConfigNameAsync(configName);
        }

        #endregion
    }
}
