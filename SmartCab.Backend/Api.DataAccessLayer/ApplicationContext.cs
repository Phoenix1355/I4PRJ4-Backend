using System;
using Api.DataAccessLayer.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Api.DataAccessLayer
{
    public class ApplicationContext : IdentityDbContext<ApplicationUser>, IApplicationContext
    {
        public DbSet<Ride> Rides { get; set; }
        public DbSet<SoloRide> SoloRides { get; set; }
        public DbSet<Customer> Customers { get; set; }
        public DbSet<TaxiCompany> TaxiCompanies { get; set; }

        public DbSet<CustomerRides> CustomerRideses { get; set; }

        public DbSet<SharedOpenRide> SharedOpenRides { get; set; }

        public ApplicationContext(DbContextOptions<ApplicationContext> context) : base(context)
        {
            //Setup is done in the API projects Startup.cs
        }
        protected override void OnModelCreating(ModelBuilder modelbuilder)
        {
            base.OnModelCreating(modelbuilder);

            modelbuilder.Entity<Ride>()
                .HasOne(ride=> ride.StartDestination)
                .WithMany()
                .OnDelete(DeleteBehavior.ClientSetNull); // no ON DELETE

            modelbuilder.Entity<Ride>()
                .HasOne(ride => ride.SlutDestination)
                .WithMany()
                .OnDelete(DeleteBehavior.ClientSetNull); // no ON DELETE
        }
    }
}
