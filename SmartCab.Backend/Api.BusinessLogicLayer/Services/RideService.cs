using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Api.BusinessLogicLayer.DataTransferObjects;
using Api.BusinessLogicLayer.Interfaces;
using Api.BusinessLogicLayer.Responses;
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

        public async Task<OpenRidesResponse> GetAllOpenRidesAsync()
        {
            throw new NotImplementedException();
            ////Get solo and shared rides in parallel
            //var openSoloRidesTask = GetAllOpenSoloRidesAsync();
            //var openSharedRidesTask = GetAllOpenSharedRidesAsync();
            //await Task.WhenAll(openSoloRidesTask, openSharedRidesTask);

            ////.Result is not blocking since both tasks completed using WhenAll (see above)
            //var openSoloRides = openSoloRidesTask.Result;
            //var openSharedRides = openSharedRidesTask.Result;

            ////Map to dto's
            //var openSoloRidesDtos = _mapper.Map<List<SoloRide>, List<SoloRideDto>>(openSoloRides);
            ////Todo map shared rides to dto's

            ////Wrap dto's in a response
            //var response = new OpenRidesResponse
            //{
            //    OpenSoloRides = openSoloRidesDtos,
            //    //OpenSharedRides = openSharedRides //TODO: Add shared ride dto's to the response
            //};

            //return response;
        }

        private Task<List<SoloRide>> GetAllOpenSoloRidesAsync()
        {
            return _rideRepository.GetOpenSoloRidesAsync();
        }

        //private Task<List<SharedOpenRide>> GetAllOpenSharedRidesAsync()
        //{
        //    //TODO: Replace with a call to a repository method
        //    return Task.Run(() => new List<SharedOpenRide>());
        //}
    }
}
