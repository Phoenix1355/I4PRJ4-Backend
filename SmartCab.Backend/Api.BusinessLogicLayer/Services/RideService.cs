using System.Collections.Generic;
using System.Threading.Tasks;
using Api.BusinessLogicLayer.Interfaces;
using Api.DataAccessLayer.Interfaces;
using Api.DataAccessLayer.Models;

namespace Api.BusinessLogicLayer.Services
{
    /// <summary>
    /// This class contains business logic related to "Rides".
    /// Fairly redundant at the moment, but its here to show the intent with the business logic layer.
    /// </summary>
    public class RideService : IRideService
    {
        private readonly IRideRepository _rideRepository;

        public RideService(IRideRepository rideRepository)
        {
            _rideRepository = rideRepository;
        }

        public async Task<List<Ride>> GetAllRidesAsync()
        {
            return await _rideRepository.GetAllRidesAsync();
        }

        public async Task<Ride> GetRideByIdAsync(int id)
        {
            return await _rideRepository.CreateSoloRideAsync(id);
        }

        public async Task<Ride> AddRideAsync(Ride ride)
        {
            return await _rideRepository.AddRideAsync(ride);
        }

        public async Task<Ride> UpdateRideAsync(Ride ride)
        {
            return await _rideRepository.UpdateRideAsync(ride);
        }

        public async Task<Ride> DeleteRideAsync(int id)
        {
            return await _rideRepository.DeleteRideAsync(id);
        }
    }
}