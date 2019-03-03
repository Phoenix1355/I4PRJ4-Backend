using System.Collections.Generic;
using System.Threading.Tasks;
using Backend.BusinessLogicLayer.DataTransferObjects;
using Backend.DataAccessLayer.Models;

namespace Backend.BusinessLogicLayer.Interfaces
{
    public interface IRideService
    {
        Task<List<Ride>> GetAllRidesAsync();
        Task<Ride> GetRideByIdAsync(int id);
        Task<Ride> AddRideAsync(RideDTO rideDTO);
        Task<Ride> UpdateRideAsync(Ride ride);
        Task<Ride> DeleteRideAsync(int id);
    }
}