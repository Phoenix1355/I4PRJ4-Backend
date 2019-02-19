using System.Collections.Generic;
using System.Threading.Tasks;
using Ride = SmartCabPoC.DataLayer.Models.Ride;

namespace SmartCabPoC.DataLayer.Abstractions
{
    /// <summary>
    /// Defines the interface to the database in relation to "Rides".
    /// </summary>
    public interface IRideRepository
    {
        Task<List<Ride>> GetAllRidesAsync();
        Task AddRide(Ride ride);
    }
}