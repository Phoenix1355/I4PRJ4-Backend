using System.Collections.Generic;
using System.Threading.Tasks;
using Api.DataAccessLayer.Models;

namespace Api.DataAccessLayer.Interfaces
{
    /// <summary>
    /// Defines the interface to the database in relation to "Rides".
    /// </summary>
    public interface IRideRepository
    {
        Task<List<Ride>> GetAllRidesAsync();
        Task<SoloRide> AddSoloRideAsync(SoloRide id);
        Task<Ride> UpdateRideAsync(Ride ride);
        Task<Ride> DeleteRideAsync(int id);
        Task<List<SoloRide>> GetOpenSoloRidesAsync();


    }
}