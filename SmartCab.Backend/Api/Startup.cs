using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Api.BusinessLogicLayer.DataTransferObjects;
using Api.BusinessLogicLayer.Interfaces;
using Api.BusinessLogicLayer.Services;
using Api.DataAccessLayer;
using Api.DataAccessLayer.Interfaces;
using Api.DataAccessLayer.Models;
using Api.DataAccessLayer.Repositories;
using Api.Requests;
using AutoMapper;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
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
                mapper.CreateMap<CreateRideRequest, Ride>().ReverseMap(); //Setup two way map for CreateRideRequest <-> Ride. This must be done for all wanted mappings
                mapper.CreateMap<Customer, CustomerDto>();
            });

            //================== DbContext setup ========================
            services.AddDbContext<ApplicationContext>(options =>
                options.UseSqlServer(GetConnectionString()));

            // ======= Add Identity ========
            services.AddIdentity<ApplicationUser, IdentityRole>()
                    .AddEntityFrameworkStores<ApplicationContext>()
                    .AddDefaultTokenProviders();

            // ====== Configure password requirements =====
            services.Configure<IdentityOptions>(options =>
            {
                options.Password.RequireDigit = true;
                options.Password.RequiredLength = 8;
                options.Password.RequireLowercase = true;
                options.Password.RequireNonAlphanumeric = true;
                options.Password.RequireUppercase = true;
            });

            // ======= Define policies ======
            services.AddAuthorization(options =>
            {
                options.AddPolicy("CustomerRights", policy => policy.RequireRole("Customer"));
                options.AddPolicy("TaxiCompanyRights", policy => policy.RequireRole("TaxiCompany"));
            });

            // ======= Add JWT Authentication ========
            JwtSecurityTokenHandler.DefaultInboundClaimTypeMap.Clear(); // => remove default claims
            services
                .AddAuthentication(options =>
                {
                    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                    options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
                    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;

                })
                .AddJwtBearer(cfg =>
                {
                    cfg.RequireHttpsMetadata = false;
                    cfg.SaveToken = true;
                    cfg.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuer = true,
                        ValidateAudience = true,
                        ValidIssuer = Configuration["JwtIssuer"],
                        ValidAudience = Configuration["JwtIssuer"],
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Configuration["JwtKey"])),
                        ClockSkew = TimeSpan.Zero // remove delay of token when expire
                    };
                });

            //================== Dependency injection setup =============
            services.AddScoped<IRideService, RideService>();
            services.AddScoped<IRideRepository, RideRepository>();
            services.AddScoped<IJwtService, JwtService>();
            services.AddScoped<ICustomerService, CustomerService>();
            services.AddScoped<IApplicationUserRepository, ApplicationUserRepository>();
            services.AddScoped<ICustomerRepository, CustomerRepository>();

            //===================== Swagger setup =======================
            services.AddSwaggerGen(x =>
            {
                //The generated Swagger JSON file will have these properties.
                x.SwaggerDoc("v1", new Info
                {
                    Title = "SmartCab WebAPi Documentation",
                    Description = "This is the documentation for SmartCab's WebAPI",
                    Version = "1.0"
                });

                //Locate the XML file being generated by ASP.NET and tell swagger to use those XML comments
                var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
                x.IncludeXmlComments(xmlPath);
            });


        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ApplicationContext dbContext, IServiceProvider services)
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
            app.UseAuthentication(); //Important to add this here!
            app.UseMvc();

            dbContext.Database.Migrate();
            CreateRoles(services).Wait(); //Create roles if they are not already defined
        }

        private string GetConnectionString()
        {
            //return Configuration["ConnectionString"]; //will look in secrets.json
            return @"data source=.\sqlexpress;initial catalog=SmartCabDev;integrated security=true;";
        }

        private async Task CreateRoles(IServiceProvider services)
        {
            var roleManager = services.GetRequiredService<RoleManager<IdentityRole>>();

            var roles = new[] { "Customer", "TaxiCompany" };

            foreach (var role in roles)
            {
                var roleExist = await roleManager.RoleExistsAsync(role);

                if (!roleExist)
                {
                    await roleManager.CreateAsync(new IdentityRole(role));
                }
            }
        }
    }
}
