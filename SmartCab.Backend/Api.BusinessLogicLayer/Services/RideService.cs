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

        /// <summary>
        /// Constructor for this class.
        /// </summary>
        /// <param name="rideRepository">Repository used to query the database when working with rides.</param>
        /// <param name="mapper">Used to map between domain classes and request/response/dto classes.</param>
        public RideService(IRideRepository rideRepository, IMapper mapper)
        {
            _rideRepository = rideRepository;
            _mapper = mapper;
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
            var ride = _mapper.Map<SoloRide>(request); //TODO: Should map to a solo ride
            ride.Price = await CalculatePrice(ride.StartDestination, ride.EndDestination, request.IsShared);
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
        /// Calculates the distance between two addresses by using the Google Map API and returns the distance.
        /// </summary>
        /// <param name="first">The first address.</param>
        /// <param name="second">The second address.</param>
        /// <returns>The distance between the two addresses.</returns>
        private async Task<decimal> GetDistanceInKilometersAsync(Address first, Address second)
        {
            string[] originAddresses = { $"{first.CityName} {first.PostalCode} {first.StreetName} {first.StreetNumber}" };
            string[] destinationAddresses = { $"{second.CityName} {second.PostalCode} {second.StreetName} {second.StreetNumber}" };

            var googleApi = new GoogleDistanceMatrixService(originAddresses, destinationAddresses);
            var response = await googleApi.GetResponse();

            var distanceInKm = Convert.ToDecimal(response.Rows.FirstOrDefault()?.Elements.FirstOrDefault()?.Distance.Value/1000.0);

            return distanceInKm;
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
        public async Task<decimal> CalculatePrice(Address first, Address second, bool isShared)
        {
            const decimal multiplier = 10;
            const decimal discount = (decimal) 0.75;

            var distance = await GetDistanceInKilometersAsync(first, second);
            var price = distance * multiplier;

            if (isShared)
            {
                price *= discount;
            }

            return price;
        }
    }
}
