using System;
using Backend.DataAccessLayer.Models;
using Microsoft.EntityFrameworkCore;

namespace Backend.DataAccessLayer
{
    public class ApplicationContext : DbContext
    {
        public DbSet<Ride> Rides { get; set; }

        public ApplicationContext(DbContextOptions<ApplicationContext> context) : base(context)
        {
            
        }
    }
}
