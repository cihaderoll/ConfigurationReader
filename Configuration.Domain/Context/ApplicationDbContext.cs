using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Configuration.Domain.Models;
using Microsoft.Extensions.Options;
using System.Runtime;
using Configuration.Common.Dtos;
using System.Threading;

namespace Configuration.Domain.Context
{
    public class ApplicationDbContext: DbContext
    {
        public readonly OptionsDto _configOpts;

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> opts, IOptions<OptionsDto> configOptions) : base(opts)
        {
            _configOpts = configOptions.Value;
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            //Adding global ApplicationName filter to prevent seeing other application's config
            modelBuilder.Entity<Models.Configuration>().HasQueryFilter(o => o.ApplicationName == _configOpts.ApplicationName);
        }

        public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            SetGlobalProps();
            return await base.SaveChangesAsync(cancellationToken);
        }

        /// <summary>
        /// Sets globally predefined properties
        /// </summary>
        private void SetGlobalProps()
        {
            var entities = ChangeTracker.Entries<Models.Configuration>()
                .Where(x => x.State == EntityState.Added || x.State == EntityState.Modified);

            foreach (var entity in entities)
            {
                entity.Entity.ApplicationName = _configOpts.ApplicationName;
            }
        }

        public DbSet<Models.Configuration> Configurations { get; set; }
    }
}
