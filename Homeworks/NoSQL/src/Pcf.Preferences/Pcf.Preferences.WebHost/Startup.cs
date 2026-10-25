using System;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Pcf.Preferences.Core.Abstractions.Repositories;
using Pcf.Preferences.DataAccess;
using Pcf.Preferences.DataAccess.Data;
using Pcf.Preferences.DataAccess.Repositories;

namespace Pcf.Preferences.WebHost
{
    public class Startup
    {
        public IConfiguration Configuration { get; }

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public void ConfigureServices(IServiceCollection services)
        {
            var databaseConnection = Configuration.GetConnectionString("PromocodeFactoryPreferencesDb");
            if (string.IsNullOrWhiteSpace(databaseConnection))
            {
                throw new InvalidOperationException("Не задана строка подключения PromocodeFactoryPreferencesDb.");
            }

            var redisConnection = Configuration.GetConnectionString("Redis");
            if (string.IsNullOrWhiteSpace(redisConnection))
            {
                throw new InvalidOperationException("Не задана строка подключения Redis.");
            }

            services.AddControllers().AddMvcOptions(x =>
                x.SuppressAsyncSuffixInActionNames = false);
            services.AddDbContext<DataContext>(x =>
            {
                x.UseNpgsql(databaseConnection);
                x.UseSnakeCaseNamingConvention();
            });
            services.AddStackExchangeRedisCache(options =>
            {
                options.Configuration = redisConnection;
                options.InstanceName = "preferences:";
            });
            services.AddScoped<EfPreferenceRepository>();
            services.AddScoped<IPreferenceRepository, CachedPreferenceRepository>();
            services.AddScoped<IDbInitializer, EfDbInitializer>();

            services.AddOpenApiDocument(options =>
            {
                options.Title = "PromoCode Factory Preferences API Doc";
                options.Version = "1.0";
            });
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, IDbInitializer dbInitializer)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseHsts();
            }

            app.UseOpenApi();
            app.UseSwaggerUi(x =>
            {
                x.DocExpansion = "list";
            });

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });

            dbInitializer.InitializeDb();
        }
    }
}
