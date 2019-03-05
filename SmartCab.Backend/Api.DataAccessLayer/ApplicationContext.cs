using System;
using Api.DataAccessLayer.Models;
using Microsoft.EntityFrameworkCore;

namespace Api.DataAccessLayer
{
    public class ApplicationContext : DbContext
    {
        public DbSet<Ride> Rides { get; set; }

        public ApplicationContext(DbContextOptions<ApplicationContext> context) : base(context)
        {

        }
    }
}
