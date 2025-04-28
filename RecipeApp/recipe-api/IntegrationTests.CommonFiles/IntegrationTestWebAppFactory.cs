using DotNet.Testcontainers.Builders;
using IngredientsApi.Data;
using IngredientsApi.Models;
using IngredientsApi.Services;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using RecipesApi.Models;
using Testcontainers.MsSql;
using Xunit;

namespace IntegrationTests.CommonFiles
{
    public class IntegrationTestWebAppFactory : WebApplicationFactory<Program>, IAsyncLifetime
    {

        private readonly MsSqlContainer _dbContainer = new MsSqlBuilder()
            .WithImage("mcr.microsoft.com/mssql/server:2022-latest")
            .WithPassword("P@ssw0rd123!")
            .WithWaitStrategy(Wait.ForUnixContainer().UntilPortIsAvailable(1433))
            .Build();

        private string _connectionString;
        private const string DatabaseName = "TestDb";

        protected override void ConfigureWebHost(IWebHostBuilder builder)
        {
            builder.ConfigureTestServices(service =>
            {
                var descriptor = service
                .SingleOrDefault(s => s.ServiceType == typeof(DbContextOptions<AppDbContext>));

                if (descriptor != null)
                {
                    service.Remove(descriptor);
                }

                service.AddDbContext<AppDbContext>(options =>
                {
                    options
                    .UseSqlServer(_connectionString);

                });

                service.AddScoped<IngredientService>();
            });
        }

        public async Task InitializeAsync()
        {
            await _dbContainer.StartAsync();
            //await WaitUntilDatabaseIsReady(_dbContainer.GetConnectionString());

            var baseConnectionString = _dbContainer.GetConnectionString();

            using (var connection = new SqlConnection(baseConnectionString))
            {
                await connection.OpenAsync();
                using var command = new SqlCommand($"CREATE DATABASE {DatabaseName}", connection);
                await command.ExecuteNonQueryAsync();
            }

            // Update connection string to use the custom database
            var builder = new SqlConnectionStringBuilder(baseConnectionString)
            {
                InitialCatalog = DatabaseName
            };
            _connectionString = builder.ToString();

            // Create the database schema
            using var scope = Services.CreateScope();
            var dbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();
            await dbContext.Database.EnsureCreatedAsync();

        }

        public new async Task DisposeAsync()
        {
            await _dbContainer.StopAsync();
            await _dbContainer.DisposeAsync();
        }


        //private static async task waituntildatabaseisready(string connectionstring, int retries = 10)
        //{
        //    for (int i = 0; i < retries; i++)
        //    {
        //        try
        //        {
        //            using var connection = new sqlconnection(connectionstring);
        //            await connection.openasync();
        //            return;
        //        }
        //        catch (sqlexception)
        //        {
        //            await task.delay(timespan.fromseconds(2));
        //        }
        //    }
        //    throw new invalidoperationexception("sql server did not become ready in time.");
        //}

    }

}
