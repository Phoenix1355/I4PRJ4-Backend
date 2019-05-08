using System;
using Api.DataAccessLayer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;


namespace Api
{
    public class FakeStartup : Startup
    {
        public FakeStartup(IConfiguration configuration) : base(configuration)
        {
            
        }
        /// <summary>
        /// This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        /// </summary>
        /// <param name="app">The application builder for the application</param>
        /// <param name="env">The environment for the application</param>
        /// <param name="dbContext">The DbContext used by the application</param>
        /// <param name="services">The service container for the application</param>
        public override void Configure(IApplicationBuilder app, IHostingEnvironment env, ApplicationContext dbContext, IServiceProvider services)
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

            //This line enables the app to use Swagger, with the configuration in the ConfigureServices method
            app.UseSwagger();

            //This line enables Swagger UI, which provides us with a nice, simple UI with which we can view our API calls
            app.UseSwaggerUI(x =>
            {
                x.SwaggerEndpoint("./swagger/v1/swagger.json", "SmartCab Web API");
                x.RoutePrefix = string.Empty;
            });

            app.UseHttpsRedirection();
            app.UseAuthentication(); //Important to add this before "app.UseMvc" otherwise authentication won't work
            app.UseMvc();

            //Create database if it does not exist and apply pending migrations, then create role if needed

            //dbContext.Database.Migrate();
            dbContext.Database.EnsureCreated();

            CreateRoles(services).Wait();
        }
    }
}
