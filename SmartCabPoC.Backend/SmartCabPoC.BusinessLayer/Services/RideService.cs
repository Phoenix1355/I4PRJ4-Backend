using System.Collections.Generic;
using System.Threading.Tasks;
using SmartCabPoC.BusinessLayer.Abstractions;
using SmartCabPoC.DataLayer.Abstractions;
using SmartCabPoC.DataLayer.Models;
using SmartCabPoC.DataLayer.Repositories;

namespace SmartCabPoC.BusinessLayer.Services
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

        public async Task AddRideAsync(Ride ride)
        {
            await _rideRepository.AddRide(ride);
        }
    }
}