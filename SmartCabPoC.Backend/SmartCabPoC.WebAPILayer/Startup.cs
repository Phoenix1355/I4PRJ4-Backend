using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using SmartCabPoC.BusinessLayer.Abstractions;
using SmartCabPoC.BusinessLayer.Services;
using SmartCabPoC.DataLayer;
using SmartCabPoC.DataLayer.Abstractions;
using SmartCabPoC.DataLayer.Repositories;

namespace SmartCabPoC.WebAPILayer
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
            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_2);

            //The connectionstring is stored in Azure's appsettings to avoid version control
            //https://stackoverflow.com/questions/34269106/read-connectionstring-outside-startup-from-appsetting-json-in-vnext/40902384
            //https://blogs.msdn.microsoft.com/waws/2018/06/12/asp-net-core-settings-for-azure-app-service/
            var connectionString = Configuration.GetConnectionString("SmartCabPoC");

            //Set up dependencies --> dependency injection
            //https://cmatskas.com/net-core-dependency-injection-with-constructor-parameters-2/
            services.AddScoped<ISmartCabContext>(s => new SmartCabContext(connectionString));
            services.AddScoped<IRideService, RideService>();
            services.AddScoped<IRideRepository, RideRepository>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseMvc();
        }
    }
}
