using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using Api.BusinessLogicLayer.DataTransferObjects;
using Api.BusinessLogicLayer.Interfaces;
using Api.BusinessLogicLayer.Requests;
using Api.BusinessLogicLayer.Responses;
using Api.DataAccessLayer.Interfaces;
using Api.DataAccessLayer.Models;
using AutoMapper;
using CustomExceptions;
using Microsoft.IdentityModel.Tokens;

namespace Api.BusinessLogicLayer.Services
{
    /// <summary>
    /// This class contains business logic related to "Rides".
    /// </summary>
    public class RideService : IRideService
    {
        private readonly IRideRepository _rideRepository;
        private readonly IMapper _mapper;
        private readonly IGoogleMapsApiService _googleMapsApiService;

        /// <summary>
        /// Constructor for this class.
        /// </summary>
        /// <param name="rideRepository">Repository used to query the database when working with rides.</param>
        /// <param name="mapper">Used to map between domain classes and request/response/dto classes.</param>
        public RideService(IRideRepository rideRepository, IMapper mapper, IGoogleMapsApiService googleMapsApiService)
        {
            _rideRepository = rideRepository;
            _mapper = mapper;
            _googleMapsApiService = googleMapsApiService;
        }

        /// <summary>
        /// Adds a ride to the database.
        /// </summary>
        /// <param name="request">The data used to create the ride.</param>
        /// <param name="customerId">The id of the customer that has requested to create the ride.</param>
        /// <returns>A response object containing information about the created ride.</returns>
        public Task<CreateRideResponse> AddRideAsync(CreateRideRequest request, string customerId)
        {
            if (request.IsShared)
            {
                return AddSharedRideAsync(request, customerId);
            }

            return AddSoloRideAsync(request, customerId);
        }

        /// <summary>
        /// Adds a solo ride to the database.
        /// </summary>
        /// <param name="request">The data used to create the ride.</param>
        /// <param name="customerId">The id of the customer that has requested to create the ride.</param>
        /// <returns>A response object containing information about the created ride.</returns>
        private async Task<CreateRideResponse> AddSoloRideAsync(CreateRideRequest request, string customerId)
        {
            var ride = _mapper.Map<SoloRide>(request);
            ride.Price = await CalculatePriceAsync(ride.StartDestination, ride.EndDestination, false);
            ride.CustomerId = customerId;
            ride = await _rideRepository.AddSoloRideAsync(ride);
            var response = _mapper.Map<CreateRideResponse>(ride);
            return response;
        }

        /// <summary>
        /// Adds a shared ride to the database.
        /// </summary>
        /// <param name="request">The data used to create the ride.</param>
        /// <param name="customerId">The id of the customer that has requested to create the ride.</param>
        /// <returns>A response object containing information about the created ride.</returns>
        private async Task<CreateRideResponse> AddSharedRideAsync(CreateRideRequest request, string customerId)
        {
            throw new NotImplementedException("Not currently implemented.");
            //Follows the same flow as when creating a solo ride.
            //The match is made by the system later on and not in this method
            //The match should be done by the system continuously 
        }

        /// <summary>
        /// Calculates the price of a ride between two addresses.
        /// <remarks>
        /// The algorithm deducts a discount if the ride is shared.<br/>
        /// A solo ride costs the full price.
        /// </remarks>
        /// </summary>
        /// <param name="first">The first address</param>
        /// <param name="second">The second address</param>
        /// <param name="isShared">True if it is a shared ride, false if it is a solo ride.</param>
        /// <returns>The price of the ride.</returns>
        public async Task<decimal> CalculatePriceAsync(Address first, Address second, bool isShared)
        {
            //Business rule: Price should be 10x the distance in kilometers
            const decimal multiplier = 10;

            //Business rule: A shared ride gives the customer a discount of 25%
            const decimal discount = (decimal)0.75;

            var distance = await GetDistanceInKilometersAsync(first, second);
            var price = distance * multiplier;

            if (isShared)
            {
                price *= discount;
            }

            return price;
        }

        /// <summary>
        /// Calculates the distance between two addresses and returns the distance.
        /// </summary>
        /// <param name="first">The first address.</param>
        /// <param name="second">The second address.</param>
        /// <returns>The distance between the two addresses.</returns>
        private async Task<decimal> GetDistanceInKilometersAsync(Address first, Address second)
        {
            //Validate the addresses
            var validateOrigin = _googleMapsApiService.ValidateAddress(first.ToString());
            var validateDestination = _googleMapsApiService.ValidateAddress(second.ToString());
            await Task.WhenAll(validateOrigin, validateDestination); 

            //Validation ok (otherwise an exception would be thrown above)
            var distanceInKm = await _googleMapsApiService.GetDistanceInKmAsync(first.ToString(), second.ToString());

            return distanceInKm;
        }
    }
}
