using Configuration.Common.Dtos;
using Configuration.Core.Abstract;
using Configuration.Core.Concrete;
using Configuration.Domain.Abstract;
using Configuration.Domain.Concrete;
using Configuration.Domain.Context;
using Configuration.Web.Helpers;
using ConfigurationReader.Abstract;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Runtime;
using ConfigurationReader.Helpers;

namespace Configuration.Web
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllersWithViews().AddRazorRuntimeCompilation();

            services.AddDbContext<ApplicationDbContext>(
                options => options.UseSqlServer(Configuration.GetConnectionString("ConfigurationDB")));


            //service injection
            services.AddScoped<IUnitOfWork, UnitOfWork>();
            services.AddScoped<IConfigurationService, ConfigurationService>();

            //Options Pattern
            services.Configure<OptionsDto>(Configuration.GetSection("ConfigOptions"));

            var refreshTimerConvertResult = int.TryParse(Configuration["ConfigOptions:RefreshTimerIntervalInMs"], out int refreshTimerIntervalInMs);
            var redisPortResult = int.TryParse(Configuration["Redis:Port"], out int redisPort);

            //injecting ConfigurationReader Service with parameters
            services.AddSingleton<IConfigurationReader>(sp =>
                ActivatorUtilities.CreateInstance<ConfigurationReader.Concrete.ConfigurationReader>
                (
                    sp, 
                    Configuration["ConfigOptions:ApplicationName"],  
                    Configuration.GetConnectionString("ConfigurationDB"), 
                    refreshTimerConvertResult ? refreshTimerIntervalInMs : 5000
                ));

            //injecting CacheHelper with parameters
            services.AddSingleton<Helpers.ICacheHelper>(sp =>
                ActivatorUtilities.CreateInstance<Helpers.CacheHelper>
                (
                    sp, 
                    Configuration["Redis:Host"],
                    redisPortResult ? redisPort : 6379
                ));

            //class library setup
            services.SetupConfigReader(Configuration.GetConnectionString("ConfigurationDB"), Configuration["Redis:Host"], redisPortResult ? redisPort : 6379);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }
            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Configuration}/{action=List}/{id?}");
            });
        }
    }
}
