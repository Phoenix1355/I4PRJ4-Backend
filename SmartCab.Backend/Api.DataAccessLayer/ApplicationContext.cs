using System;
using Api.DataAccessLayer.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Internal;

namespace Api.DataAccessLayer
{
    public class ApplicationContext : IdentityDbContext<IdentityUser>
    {
        public DbSet<Ride> Rides { get; set; }
        public DbSet<SoloRide> SoloRides { get; set; }
        public DbSet<Customer> Customers { get; set; }
        public DbSet<TaxiCompany> TaxiCompanies { get; set; }

        public DbSet<CustomerRides> CustomerRides { get; set; }
        public DbSet<MatchedRides> MatchedRides { get; set; }
        public DbSet<SharedOpenRide> SharedOpenRides { get; set; }
        public DbSet<Order> Orders { get; set; }

        public ApplicationContext(DbContextOptions<ApplicationContext> context) : base(context)
        {
            //Setup is done in the API projects Startup.cs
        }
    }
}
