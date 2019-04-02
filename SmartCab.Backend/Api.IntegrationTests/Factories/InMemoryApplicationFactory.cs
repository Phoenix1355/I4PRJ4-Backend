using System;
using System.Data.Common;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Api;
using Api.DataAccessLayer;
using Api.DataAccessLayer.UnitTests.Factories;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.Extensions.Logging;


namespace SmartCabPoc.Integration.Test
{
    public class InMemoryApplicationFactory<TStartup>
        : WebApplicationFactory<Startup>
    {

        private string _guid;
        private ServiceProvider _serviceProvider;
        public InMemoryApplicationFactory(string guid)
        {
            _guid = guid;
        }

        protected override void ConfigureWebHost(IWebHostBuilder builder)
        {
            builder.ConfigureServices(services =>
            {
                // Create a new service provider.
                var serviceProvider = new ServiceCollection()
                    .AddEntityFrameworkInMemoryDatabase()
                    .AddEntityFrameworkProxies()
                    .BuildServiceProvider();

                services.AddDbContext<ApplicationContext>(options =>
                {
                    options.UseLazyLoadingProxies();
                    options.ConfigureWarnings(warning => warning.Ignore(InMemoryEventId.TransactionIgnoredWarning));
                    options.UseInMemoryDatabase(_guid);
                    options.UseInternalServiceProvider(serviceProvider);
                });

                _serviceProvider = services.BuildServiceProvider();

                // Create a scope to obtain a reference to the database
                // context (ApplicationDbContext).
                using (var scope = _serviceProvider.CreateScope())
                {
                    var scopedServices = scope.ServiceProvider;
                    var db = scopedServices.GetRequiredService<ApplicationContext>();
                    
                    // Ensure the database is created.
                    db.Database.EnsureCreated();
                }
            
        });
        }

        public ApplicationContext CreateContext()
        {
            var scope = _serviceProvider.CreateScope();
           
                var scopedServices = scope.ServiceProvider;
                return scopedServices.GetRequiredService<ApplicationContext>();
            
        }
    }
}