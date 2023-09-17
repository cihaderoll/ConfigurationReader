using Configuration.Common.Dtos;
using Configuration.Domain.Abstract;
using Configuration.Domain.Context;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Configuration.Domain.Concrete
{
    public class UnitOfWork : IUnitOfWork
    {
        private ApplicationDbContext _context;

        public UnitOfWork(ApplicationDbContext ctx, IOptions<OptionsDto> configOptions)
        {
            _context = ctx;
        }

        public IRepository<Models.Configuration> Configurations => new ConfigurationRepository(_context);

        public async Task<int> SaveChangesAsync()
        {
            return await _context.SaveChangesAsync();
        }

        public void Dispose()
        {
            _context.Dispose();
        }
    }
}
