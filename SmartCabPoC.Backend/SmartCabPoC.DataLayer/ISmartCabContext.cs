using Microsoft.EntityFrameworkCore;
using SmartCabPoC.DataLayer.Models;

namespace SmartCabPoC.DataLayer
{
    /// <summary>
    /// This interface is added to be able to inject the context into RideRepository using dependency injection.
    /// Most likely a hack, but currently the only solution that works.
    /// </summary>
    public interface ISmartCabContext
    {
        DbSet<Ride> Rides { get; set; }
    }
}