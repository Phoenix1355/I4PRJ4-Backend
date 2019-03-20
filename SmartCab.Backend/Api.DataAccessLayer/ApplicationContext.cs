using System;
using Api.DataAccessLayer.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Api.DataAccessLayer
{
    public class ApplicationContext : IdentityDbContext<ApplicationUser>, IApplicationContext
    {
        public DbSet<Ride> Rides { get; set; }
        public DbSet<Customer> Customers { get; set; }
        public DbSet<TaxiCompany> TaxiCompanies { get; set; }

        public ApplicationContext(DbContextOptions<ApplicationContext> context) : base(context)
        {
            //Setup is done in the API projects Startup.cs
        }
    }
}
