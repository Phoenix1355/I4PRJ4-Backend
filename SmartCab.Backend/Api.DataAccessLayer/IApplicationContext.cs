using Api.DataAccessLayer.Models;
using Microsoft.EntityFrameworkCore;

namespace Api.DataAccessLayer
{
    public interface IApplicationContext
    {
        DbSet<Ride> Rides { get; set; }
        DbSet<Customer> Customers { get; set; }
        DbSet<TaxiCompany> TaxiCompanies { get; set; }
    }
}