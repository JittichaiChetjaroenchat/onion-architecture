﻿using System;
using System.Linq;
using System.Threading.Tasks;
using Application.Persistence;
using DotNet.Testcontainers.Builders;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Persistence;
using Testcontainers.MySql;

namespace Api.IntegrationTest
{
    public class TestWebApplicationFactory : WebApplicationFactory<Program>
    {
        private readonly string Database = "test_database";
        private readonly string Username = "test_user";
        private readonly string Password = "test_password";
        private readonly int OriginalPort = 3306;
        private readonly int ExposePort = Random.Shared.Next(10000, 60000);
        private readonly MySqlContainer _dbContainer;

        public TestWebApplicationFactory()
        {
            // Builder database container
            _dbContainer = new MySqlBuilder()
                .WithDatabase(Database)
                .WithUsername(Username)
                .WithPassword(Password)
                .WithPortBinding(ExposePort, OriginalPort)
                .WithWaitStrategy(Wait.ForUnixContainer().UntilPortIsAvailable(OriginalPort))
                .WithCleanUp(true)
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
                var connectionString = _dbContainer.GetConnectionString();
                var version = ServerVersion.AutoDetect(connectionString);
                services.AddDbContextPool<DatabaseContext>(options =>
                {
                    options.UseMySql(connectionString, version)
                        .EnableSensitiveDataLogging()
                        .EnableDetailedErrors();
                });

                services.AddScoped<IDatabaseContext>(provider => provider.GetService<DatabaseContext>());

                #endregion
            });
        }

        public override async ValueTask DisposeAsync()
        {
            if (_dbContainer != null)
            {
                // Stop database container
                await _dbContainer.StopAsync();
            }
        }
    }
}