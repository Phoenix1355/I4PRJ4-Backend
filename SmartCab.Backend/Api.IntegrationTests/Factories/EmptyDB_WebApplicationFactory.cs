using System.Data.Common;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Api;
using Api.DataAccessLayer;
using Api.DataAccessLayer.UnitTests.Factories;


namespace SmartCabPoc.Integration.Test
{
    public class EmptyDB_WebApplicationFactory<TStartup>
        : WebApplicationFactory<Startup>
    {
        private DbConnection _connection;

        public EmptyDB_WebApplicationFactory(DbConnection connection)
        {
            _connection = connection;
        }


        protected override void ConfigureWebHost(IWebHostBuilder builder)
        {
            builder.ConfigureServices(services =>
            {
                // Create a new service provider.
                var serviceProvider = new ServiceCollection()
                    .AddEntityFrameworkSqlServer()
                    .AddEntityFrameworkProxies()
                    .BuildServiceProvider();

                services.AddDbContext<ApplicationContext>(options =>
                {
                    options.UseLazyLoadingProxies();
                    options.UseSqlServer(_connection);
                    options.UseInternalServiceProvider(serviceProvider);
                });
            });
        }
    }
}