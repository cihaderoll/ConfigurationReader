using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConfigurationReader.Helpers
{
    public static class ServiceCollectionExtensions
    {
        public static void SetupConfigReader(this IServiceCollection services, string connectionString, string redisHost, int redisPort)
        {
            services.AddSingleton<ICacheHelper>(sp =>
                ActivatorUtilities.CreateInstance<CacheHelper>
                (
                    sp,
                    redisHost,
                    redisPort
                ));
        }
    }
}
