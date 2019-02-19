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
            //_connectionString = configuration.GetConnectionString("SmartCabPoC");
            _connectionString = configuration;
        }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            //var username = "smartcabadmin";
            //var password = "I4PRJ4gruppe7";

            //optionsBuilder.UseSqlServer(
            //    $"Server=tcp:boostersacademy.database.windows.net,1433;Initial Catalog=SmartCabPoC.PoC;Persist Security Info=False;User ID={username};Password={password};MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;"
            //    );

            optionsBuilder.UseSqlServer(_connectionString);
        }
    }
}