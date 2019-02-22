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

            //Set up dependencies --> dependency injection. Se some tips in the link below:
            //https://cmatskas.com/net-core-dependency-injection-with-constructor-parameters-2/
            services.AddScoped<ISmartCabContext>(s => new SmartCabContext(GetConnectionString()));
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

        /// <summary>
        /// Gets the connection string for the sql database. 
        /// <para>This is stored in azure if run online or in secrets.json (which is not included in version control) if run locally.</para>
        /// Check out these sources for an explanation:
        /// <para>https://stackoverflow.com/questions/34269106/read-connectionstring-outside-startup-from-appsetting-json-in-vnext/40902384</para>
        /// <para>https://blogs.msdn.microsoft.com/waws/2018/06/12/asp-net-core-settings-for-azure-app-service/</para>
        /// </summary>
        /// <returns>The connection string for the database</returns>
        private string GetConnectionString()
        {
            var connectionString = Configuration.GetConnectionString("SmartCabPoC");

            if (string.IsNullOrEmpty(connectionString))
            {
                connectionString = Configuration["ConnectionString"];
            }

            return connectionString;
        }
    }
}

