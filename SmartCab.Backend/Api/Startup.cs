﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Api.BusinessLogicLayer.Interfaces;
using Api.BusinessLogicLayer.Services;
using Api.DataAccessLayer;
using Api.DataAccessLayer.Interfaces;
using Api.DataAccessLayer.Models;
using Api.DataAccessLayer.Repositories;
using Api.DataTransferObjects;
using AutoMapper;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Swashbuckle.AspNetCore.Swagger;

namespace Api
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

            //================== AutoMapper setup =======================
            services.AddAutoMapper(mapper =>
            {
                mapper.CreateMap<RideDTO, Ride>().ReverseMap(); //Setup two way map for RideDTO <-> Ride. This must be done for all wanted mappings
            });

            //================== DbContext setup ========================
            services.AddDbContext<ApplicationContext>(options =>
                options.UseSqlServer(GetConnectionString()));

            //================== Dependency injection setup =============
            services.AddScoped<IRideService, RideService>();
            services.AddScoped<IRideRepository, RideRepository>();

            //===================== Swagger setup =======================
            services.AddSwaggerGen(x =>
            {
                //The generated Swagger JSON file will have these properties.
                x.SwaggerDoc("v1", new Info
                {
                    Title = "SmartCab WebAPi Documentation",
                    Description = "This is the documentation for SmartCab's awesome WebAPI",
                    Version = "1.0"
                });

                //Locate the XML file being generated by ASP.NET and tell swagger to use those XML comments
                var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
                x.IncludeXmlComments(xmlPath);
            });
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

            //This line enables the app to use Swagger, with the configuration in the ConfigureServices method
            app.UseSwagger();

            //This line enables Swagger UI, which provides us with a nice, simple UI with which we can view our API calls
            app.UseSwaggerUI(x =>
            {
                x.SwaggerEndpoint("./swagger/v1/swagger.json", "SmartCab Web API");
                x.RoutePrefix = string.Empty;
            });

            app.UseHttpsRedirection();
            app.UseMvc();
        }

        private string GetConnectionString()
        {
            //return Configuration["ConnectionString"]; //will look in secrets.json
            return @"data source=.\sqlexpress;initial catalog=SmartCabDev;integrated security=true;";
        }
    }
}
