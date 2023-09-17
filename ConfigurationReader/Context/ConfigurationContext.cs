using Microsoft.EntityFrameworkCore;

namespace ConfigurationReader.Context
{
    public class ConfigurationContext: DbContext
    {
        private string _connectionString;

        public ConfigurationContext(DbContextOptions<ConfigurationContext> opts): base(opts)
        {
            
        }

        public DbSet<Models.Configuration> Configurations { get; set; }
    }
}
