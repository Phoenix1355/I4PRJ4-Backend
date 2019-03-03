using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using Backend.BusinessLogicLayer.DataTransferObjects;
using Backend.BusinessLogicLayer.Interfaces;
using Backend.DataAccessLayer.Interfaces;
using Backend.DataAccessLayer.Models;
using Microsoft.EntityFrameworkCore.Storage;

namespace Backend.BusinessLogicLayer.Services
{
    /// <summary>
    /// This class contains business logic related to "Rides".
    /// Fairly redundant at the moment, but its here to show the intent with the business logic layer.
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

        public async Task<List<Ride>> GetAllRidesAsync()
        {
            return await _rideRepository.GetAllRidesAsync();
        }

        public async Task<Ride> GetRideByIdAsync(int id)
        {
            return await _rideRepository.GetRideByIdAsync(id);
        }


        public async Task<Ride> AddRideAsync(RideDTO rideDTO)
        {
            var ride = _mapper.Map<RideDTO, Ride>(rideDTO);
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