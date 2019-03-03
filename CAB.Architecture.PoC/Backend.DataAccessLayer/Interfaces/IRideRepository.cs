using System.Collections.Generic;
using System.Threading.Tasks;
using Backend.DataAccessLayer.Models;

namespace Backend.DataAccessLayer.Interfaces
{
    /// <summary>
    /// Defines the interface to the database in relation to "Rides".
    /// </summary>
    public interface IRideRepository
    {
        Task<List<Ride>> GetAllRidesAsync();
        Task<Ride> GetRideByIdAsync(int id);
        Task<Ride> AddRideAsync(Ride ride);
        Task<Ride> UpdateRideAsync(Ride ride);
        Task<Ride> DeleteRideAsync(int id);
    }
}