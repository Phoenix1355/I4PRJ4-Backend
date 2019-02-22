using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using SmartCabPoC.DataLayer.Models;

namespace SmartCabPoC.DataLayer
{
    /// <summary>
    /// This class is the direct link to the sql database
    /// </summary>
    public class SmartCabContext : DbContext
    {
        private readonly string _connectionString;
        public DbSet<Ride> Rides { get; set; }

        public SmartCabContext(DbContextOptions<SmartCabContext> options)
            : base(options)
        { }
    }
}