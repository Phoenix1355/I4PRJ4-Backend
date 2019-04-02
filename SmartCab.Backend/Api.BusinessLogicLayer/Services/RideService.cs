using System.Collections.Generic;
using System.Threading.Tasks;
using Api.BusinessLogicLayer.DataTransferObjects;
using Api.BusinessLogicLayer.Interfaces;
using Api.DataAccessLayer.Interfaces;
using Api.DataAccessLayer.Models;
using AutoMapper;

namespace Api.BusinessLogicLayer.Services
{
    /// <summary>
    /// This class contains business logic related to "Rides".
    /// </summary>
    public class RideService : IRideService
    {
        private readonly IRideRepository _rideRepository;
        private readonly IMapper _mapper;

        public RideService(IRideRepository rideRepository, IMapper mapper)
        {
            _rideRepository = rideRepository;
            _mapper = mapper;
        }

        public async Task<List<SoloRideDto>> GetAllOpenSoloRidesAsync()
        {
            var openSoloRides = await _rideRepository.GetOpenSoloRidesAsync();
            var openSoloRidesDtos = _mapper.Map <List<SoloRide>, List<SoloRideDto>>(openSoloRides);
            return openSoloRidesDtos;
        }
    }
}
