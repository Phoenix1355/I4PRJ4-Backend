using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using Api.BusinessLogicLayer.DataTransferObjects;
using Api.BusinessLogicLayer.Enums;
using Api.BusinessLogicLayer.Helpers;
using Api.BusinessLogicLayer.Interfaces;
using Api.BusinessLogicLayer.Requests;
using Api.BusinessLogicLayer.Responses;
using Api.DataAccessLayer.Interfaces;
using Api.DataAccessLayer.Models;
using Api.DataAccessLayer.UnitOfWork;
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
        private readonly IPriceStrategyFactory _priceStrategyFactory;
        private readonly ICreateRideUOW _createRideUOW;

        /// <summary>
        /// Constructor for this class.
        /// </summary>
        /// <param name="rideRepository">Repository used to query the database when working with rides.</param>
        /// <param name="mapper">Used to map between domain classes and request/response/dto classes.</param>
        /// <param name="googleMapsApiService">Used to send requests to the Google Maps Api</param>
        /// <param name="priceStrategyFactory">Used to get the correct strategy for price calculations</param>
        public RideService(
            IRideRepository rideRepository,
            IMapper mapper,
            IGoogleMapsApiService googleMapsApiService, 
            IPriceStrategyFactory priceStrategyFactory, 
            ICreateRideUOW createRideUow)
        {
            _rideRepository = rideRepository;
            _mapper = mapper;
            _googleMapsApiService = googleMapsApiService;
            _priceStrategyFactory = priceStrategyFactory;
            _createRideUOW = createRideUow;
        }

        /// <summary>
        /// Adds a ride to the database.
        /// </summary>
        /// <param name="request">The data used to create the ride.</param>
        /// <param name="customerId">The id of the customer that has requested to create the ride.</param>
        /// <returns>A response object containing information about the created ride.</returns>
        public Task<CreateRideResponse> AddRideAsync(CreateRideRequest request, string customerId)
        {
            if (request.RideType == RideType.SharedRide)
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
            ride.Price = await CalculatePriceAsync(ride.StartDestination, ride.EndDestination, request.RideType);
            ride.CustomerId = customerId;

            //New segment
            _createRideUOW.TransactionWrapper(() =>
            {
            //Now wrapped in transaction
            _createRideUOW.ReservePriceFromCustomer(customerId,ride.Price);
            ride = (SoloRide) _createRideUOW.AddRide(ride);
            var order = _createRideUOW.CreateOrder();
            _createRideUOW.AddRideToOrder(ride, order);
            _createRideUOW.SaveChanges();
            });
            //Notice the two save changes

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
        /// <param name="type">The type of the ride</param>
        /// <returns>The price of the ride.</returns>
        public async Task<decimal> CalculatePriceAsync(Address first, Address second, RideType type)
        {
            var distance = await GetDistanceInKilometersAsync(first, second);
            var priceStrategy = _priceStrategyFactory.GetPriceStrategy(type);
            return priceStrategy.CalculatePrice(distance);
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