using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
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
        private readonly IPriceCalculator _soloRidePriceCalculator;
        private readonly IPriceCalculator _sharedRidePriceCalculator;

        /// <summary>
        /// Constructor for this class.
        /// </summary>
        /// <param name="rideRepository">Repository used to query the database when working with rides.</param>
        /// <param name="mapper">Used to map between domain classes and request/response/dto classes.</param>
        /// <param name="googleMapsApiService">Used to send requests to the Google Maps Api</param>
        /// <param name="priceCalculators">A collection that must contain two calculators. The first one for solo ride, the second one for shared rides</param>
        public RideService(
            IRideRepository rideRepository,
            IMapper mapper,
            IGoogleMapsApiService googleMapsApiService,
            IEnumerable<IPriceCalculator> priceCalculators)
        {
            _rideRepository = rideRepository;
            _mapper = mapper;
            _googleMapsApiService = googleMapsApiService;

            //The strategy pattern is used to define different strategies for the price calculation
            //Our DI container does not support named instances, so this is a workaround.
            //Source: https://www.stevejgordon.co.uk/asp-net-core-dependency-injection-registering-multiple-implementations-interface
            _soloRidePriceCalculator = priceCalculators.ElementAt(0);
            _sharedRidePriceCalculator = priceCalculators.ElementAt(1);
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
            var distance = await GetDistanceInKilometersAsync(first, second);

            if (isShared)
            {
                return _sharedRidePriceCalculator.CalculatePrice(distance);
            }
            else
            {
                return _soloRidePriceCalculator.CalculatePrice(distance);
            }
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
            var firstAsString = first.ToString();
            var secondAsString = second.ToString();
            var validateOrigin = _googleMapsApiService.ValidateAddressAsync(firstAsString);
            var validateDestination = _googleMapsApiService.ValidateAddressAsync(firstAsString);
            await Task.WhenAll(validateOrigin, validateDestination);

            //Validation ok (otherwise an exception would be thrown above)
            var distanceInKm = await _googleMapsApiService.GetDistanceInKmAsync(firstAsString, secondAsString);

            return distanceInKm;
        }
    }
}