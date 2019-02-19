using Microsoft.EntityFrameworkCore;
using SmartCabPoC.DataLayer.Models;

namespace SmartCabPoC.DataLayer
{
    public interface ISmartCabContext
    {
        DbSet<Ride> Rides { get; set; }
    }
}