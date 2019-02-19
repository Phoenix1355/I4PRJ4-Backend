using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using SmartCabPoC.DataLayer.Models;

namespace SmartCabPoC.DataLayer
{
    public class SmartCabContext : DbContext, ISmartCabContext
    {
        private readonly string _connectionString;
        public DbSet<Ride> Rides { get; set; }

        public SmartCabContext(string configuration)
        {
            _connectionString = configuration;
        }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(_connectionString);
        }
    }
}