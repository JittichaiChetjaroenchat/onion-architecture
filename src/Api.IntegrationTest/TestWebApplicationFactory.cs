using System;
using System.Linq;
using System.Threading.Tasks;
using Application.Persistence;
using DotNet.Testcontainers.Builders;
using DotNet.Testcontainers.Configurations;
using DotNet.Testcontainers.Containers;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Persistence;

namespace Api.IntegrationTest
{
    public class TestWebApplicationFactory : WebApplicationFactory<Program>
    {
        private readonly string Database = "test_database";
        private readonly string Username = "test_user";
        private readonly string Password = "test_password";
        private readonly int OriginalPort = 3306;
        private readonly int ExposePort = Random.Shared.Next(10000, 60000);
        private readonly MySqlTestcontainer _dbContainer;
        
        public TestWebApplicationFactory()
        {
            // Builder database container
            _dbContainer = new TestcontainersBuilder<MySqlTestcontainer>()
                .WithDatabase(new MySqlTestcontainerConfiguration
                {
                    Database = Database,
                    Username = Username,
                    Password = Password,
                })
                .WithPortBinding(ExposePort, OriginalPort)
                .WithWaitStrategy(Wait.ForUnixContainer().UntilPortIsAvailable(OriginalPort))
                .Build();

            // Start database container
            Task.Run(() => _dbContainer.StartAsync()).Wait();
        }

        protected override void ConfigureWebHost(IWebHostBuilder builder)
        {
            Environment.SetEnvironmentVariable("ASPNETCORE_ENVIRONMENT", "Development");

            builder.ConfigureTestServices(services =>
            {
                #region Authentication

                services.AddAuthentication(options =>
                {
                    options.DefaultAuthenticateScheme = TestAuthenticationHandler.SCHEME;
                    options.DefaultChallengeScheme = TestAuthenticationHandler.SCHEME;
                }).AddTestAuthentication(TestAuthenticationHandler.SCHEME, TestAuthenticationHandler.SCHEME, options => { });

                #endregion

                #region Database

                // Remove database's context
                var descriptor = services.SingleOrDefault(s => s.ServiceType == typeof(DbContextOptions<DatabaseContext>));
                if (descriptor != null)
                {
                    services.Remove(descriptor);
                }

                // Create database's context
                var version = ServerVersion.AutoDetect(_dbContainer.ConnectionString);
                services.AddDbContextPool<DatabaseContext>(options =>
                {
                    options.UseMySql(_dbContainer.ConnectionString, version)
                        .EnableSensitiveDataLogging()
                        .EnableDetailedErrors();
                });

                services.AddScoped<IDatabaseContext>(provider => provider.GetService<DatabaseContext>());

                #endregion
            });
        }

        public override async ValueTask DisposeAsync()
        {
            // Stop database container
            await _dbContainer.StopAsync();
        }
    }
}