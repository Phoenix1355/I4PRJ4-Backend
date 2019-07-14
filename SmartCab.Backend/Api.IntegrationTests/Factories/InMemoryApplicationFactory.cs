using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Api;
using Api.BusinessLogicLayer.Interfaces;
using Api.DataAccessLayer;
using Api.Integration.Test.Fakes;
using Api.IntegrationTests.Fakes;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.TestHost;
using Microsoft.EntityFrameworkCore.Diagnostics;


namespace SmartCabPoc.Integration.Test
{
    public class InMemoryApplicationFactory<TFakeStartup>
        : WebApplicationFactory<FakeStartup>
    {

        private string _guid;
        private ServiceProvider _serviceProvider;
        public InMemoryApplicationFactory(string guid)
        {
            _guid = guid;
        }

        protected override IWebHostBuilder CreateWebHostBuilder()
        {
            return WebHost.CreateDefaultBuilder(null)
                .UseStartup<FakeStartup>();
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

            //To overwrite services add them here. 
            builder.ConfigureTestServices(services =>
            {
               services.AddScoped<IGoogleMapsApiService, FakeGoogleMapsApiService>();
               services.AddScoped<IPushNotificationService, FakeAppCenterPushNotificationService>();
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