using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.IO;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Api.BusinessLogicLayer.DataTransferObjects;
using Api.BusinessLogicLayer.Interfaces;
using Api.BusinessLogicLayer.Services;
using Api.BusinessLogicLayer;
using Api.BusinessLogicLayer.Factories;
using Api.BusinessLogicLayer.Helpers;
using Api.BusinessLogicLayer.Requests;
using Api.BusinessLogicLayer.Responses;
using Api.DataAccessLayer;
using Api.DataAccessLayer.Interfaces;
using Api.DataAccessLayer.Models;
using Api.DataAccessLayer.Repositories;
using Api.DataAccessLayer.UnitOfWork;
using Api.ErrorHandling;
using Api.Requests;
using AutoMapper;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis.Options;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
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
            AddMvcAndExceptionHandling(services);
            AddAutoMapper(services);
            AddDbContext(services);
            AddIdentityFramework(services);
            AddJsonWebTokens(services);
            AddDependencyInjection(services);
            AddSwagger(services);
        }

        /// <summary>
        /// This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        /// </summary>
        /// <param name="app">The application builder for the application</param>
        /// <param name="env">The environment for the application</param>
        /// <param name="dbContext">The DbContext used by the application</param>
        /// <param name="services">The service container for the application</param>
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
            app.UseAuthentication(); //Important to add this before "app.UseMvc" otherwise authentication won't work
            app.UseMvc();

            //Create database if it does not exist and apply pending migrations, then create role if needed
            //dbContext.Database.Migrate();
            dbContext.Database.EnsureCreated();

            CreateRoles(services).Wait();
        }

        /// <summary>
        /// Configures the use of MVC and adds an exception filter to the middleware.
        /// </summary>
        /// <remarks>
        /// The filter makes it possible to avoid lots of try/catch clauses<br/>
        /// in the code (more specifically in the controllers).<br/>
        /// Source: https://docs.microsoft.com/en-us/aspnet/core/mvc/controllers/filters?view=aspnetcore-2.2 <br/>
        /// Source: https://www.talkingdotnet.com/global-exception-handling-in-aspnet-core-webapi/
        /// </remarks>
        /// <param name="services">The container to register to.</param>
        private void AddMvcAndExceptionHandling(IServiceCollection services)
        {
            services.AddMvc(config =>
                    {
                        config.Filters.Add(typeof(CustomExceptionFilter));
                    })
                    .SetCompatibilityVersion(CompatibilityVersion.Version_2_2);
        }

        /// <summary>
        /// Configures AutoMapper for the application.
        /// </summary>
        /// <param name="services">The container to register to.</param>
        private void AddAutoMapper(IServiceCollection services)
        {
            services.AddAutoMapper(mapper =>
            {
                mapper.CreateMap<CreateRideRequest, Ride>()
                      .ReverseMap(); //Setup two way map for CreateRideRequest

                mapper.CreateMap<Customer, CustomerDto>();
                mapper.CreateMap<SoloRide, SoloRideDto>();
                mapper.CreateMap<CreateRideRequest, SoloRide>();
                mapper.CreateMap<CreateRideRequest, SharedRide>();
                mapper.CreateMap<SoloRide, CreateRideResponse>();
                mapper.CreateMap<SharedRide, CreateRideResponse>();
                mapper.CreateMap<Ride, CreateRideResponse>(); //TODO: Only here because data-access layer currently uses ride and not soloride and sharedrides when adding new rides to the DB
                mapper.CreateMap<Ride, RideDto>();
                mapper.CreateMap<TaxiCompany, TaxiCompanyDto>();

                //Maps enum to their name, instead of integer value.
                mapper.CreateMap<Enum, String>().ConvertUsing(e => e.ToString());
            });
        }

        /// <summary>
        /// Configures the DbContext for the application.
        /// </summary>
        /// <param name="services">The container to register to.</param>
        private void AddDbContext(IServiceCollection services)
        {
            services.AddDbContext<ApplicationContext>(options =>
            {
                options.UseLazyLoadingProxies();
                options.UseSqlServer(GetConnectionString());
            });
        }

        /// <summary>
        /// Configures Identity Framework for the application.
        /// </summary>
        /// <param name="services">The container to register to.</param>
        private void AddIdentityFramework(IServiceCollection services)
        {
            services.AddIdentity<IdentityUser, IdentityRole>()
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
        }

        /// <summary>
        /// Configures the use of JSON Web Tokens for the application.
        /// </summary>
        /// <param name="services">The container to register to.</param>
        private void AddJsonWebTokens(IServiceCollection services)
        {
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
                    cfg.RequireHttpsMetadata = true;
                    cfg.SaveToken =
                        true; //https://stackoverflow.com/questions/49302473/what-is-beareroption-savetoken-property-used-for
                    cfg.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuer = false,   //We are not using the issuer feature
                        ValidateAudience = false, //We are not using the audience feature
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Configuration["JwtKey"])),
                        ClockSkew = TimeSpan.Zero // remove delay of token when expire
                    };
                });
        }

        /// <summary>
        /// Configures additional dependency injected that are not covered by other helper methods.
        /// </summary>
        /// <param name="services">The container to register to.</param>
        private void AddDependencyInjection(IServiceCollection services)
        {
            services.AddScoped<IPriceStrategyFactory, PriceStrategyFactory>();
            services.AddScoped<IOrderService, OrderService>();
            services.AddScoped<IOrderRepository, OrderRepository>();
            services.AddScoped<IHttpClient, TestableHttpClient>();
            services.AddScoped<IGoogleMapsApiService, GoogleMapsApiService>();
            services.AddScoped<IRideService, RideService>();
            services.AddScoped<IRideRepository, RideRepository>();
            services.AddScoped<IJwtService, JwtService>();
            services.AddScoped<ICustomerService, CustomerService>();
            services.AddScoped<IIdentityUserRepository, IdentityUserRepository>();
            services.AddScoped<ICustomerRepository, CustomerRepository>();
            services.AddScoped<ITaxiCompanyService, TaxiCompanyService>();
            services.AddScoped<ITaxiCompanyRepository, TaxiCompanyRepository>();
            services.AddScoped<IUnitOfWork,UnitOfWork>();
        }

        /// <summary>
        /// Configures Swagger for the application.
        /// </summary>
        /// <param name="services">The container to register to.</param>
        private void AddSwagger(IServiceCollection services)
        {
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

        /// <summary>
        /// Returns a connection string that points to the database.
        /// Running the project locally will point to a local sql express database.
        /// When deploying to Azure the connection string will be supplied by Azure (this is setup in the api's "app settings" in azure).
        /// </summary>
        /// <returns>The connection string to the database.</returns>
        private string GetConnectionString()
        {
            var connectionString = Configuration.GetConnectionString("ConnectionString"); //will look online and then in secrets.json
            return connectionString;
        }

        /// <summary>
        /// Creates a number of roles in the database if they do not already exist.
        /// </summary>
        /// <param name="services">The container to register to.</param>
        private async Task CreateRoles(IServiceProvider services)
        {
            var roleManager = services.GetRequiredService<RoleManager<IdentityRole>>();

            var roles = new[] { nameof(Customer), nameof(TaxiCompany) };

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
