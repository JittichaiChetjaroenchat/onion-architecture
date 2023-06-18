using Application.Persistence;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Persistence
{
    public static class Startup
    {
        public static void AddPersistence(this IServiceCollection services, IConfiguration configuration)
        {
            var mySqlConnectionString = configuration.GetConnectionString("MySql");

            //// For Microsoft SQL Server
            //services.AddDbContext<DatabaseContext>(options =>
            //    options.UseSqlServer(connectionString));

            // For MySQl or MariaDB
            var version = ServerVersion.AutoDetect(mySqlConnectionString);
            services.AddDbContextPool<DatabaseContext>(options =>
            {
                options.UseMySql(mySqlConnectionString, version)
                .EnableSensitiveDataLogging()
                .EnableDetailedErrors();
            });

            // Register dependency injection
            services.AddScoped<IDatabaseContext>(provider => provider.GetService<DatabaseContext>());
        }

        public static void UsePersistence(this IApplicationBuilder app)
        {
            using var serviceScope = app.ApplicationServices
                .GetRequiredService<IServiceScopeFactory>()
                .CreateScope();
            using var context = serviceScope.ServiceProvider.GetService<DatabaseContext>();

            context.Database.Migrate();
        }
    }
}