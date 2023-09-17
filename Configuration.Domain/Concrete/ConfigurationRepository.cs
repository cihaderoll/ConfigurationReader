using Configuration.Domain.Abstract;
using Configuration.Domain.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Configuration.Domain.Concrete
{
    public class ConfigurationRepository : IRepository<Models.Configuration>
    {
        protected readonly DbContext context;

        public ConfigurationRepository(DbContext _ctx)
        {
            context = _ctx;
        }

        public async Task AddAsync(Models.Configuration model)
        {
            await context.Set<Models.Configuration>().AddAsync(model);
        }

        public async Task<IEnumerable<Models.Configuration>> GetAllAsync()
        {
            return await context.Set<Models.Configuration>().ToListAsync();
        }

        public async Task<Models.Configuration> GetByIdAsync(int id)
        {
            return await context.Set<Models.Configuration>()
                .Where(o => o.IsActive && o.Id == id).FirstOrDefaultAsync();
        }

        public async Task<List<Models.Configuration>> GetByIdListAsync(List<int> idList)
        {
            return await context.Set<Models.Configuration>()
                .Where(o => o.IsActive && idList.Contains(o.Id)).ToListAsync();
        }

        public async Task<bool> CheckForDuplicateConfigNameAsync(string configName)
        {
            return await context.Set<Models.Configuration>()
                .AnyAsync(o => o.IsActive && o.Name == configName);
        }
    }
}
