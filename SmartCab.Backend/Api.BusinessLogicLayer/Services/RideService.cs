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
using Api.DataAccessLayer.Statuses;
using AutoMapper;
using CustomExceptions;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

namespace Api.BusinessLogicLayer.Services
{
    /// <summary>
    /// This class contains business logic related to "Rides".
    /// </summary>
    public class RideService : IRideService
    {
        private readonly IMapper _mapper;
        private readonly IGoogleMapsApiService _googleMapsApiService;
        private readonly IPriceStrategyFactory _priceStrategyFactory;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMatchService _matchService;
        /// <summary>
        /// Constructor for this class.
        /// </summary>

        /// <param name="mapper">Used to map between domain classes and request/response/dto classes.</param>
        /// <param name="googleMapsApiService">Used to send requests to the Google Maps Api</param>
        /// <param name="priceStrategyFactory">Used to get the correct strategy for price calculations</param>
        /// <param name="unitOfWork">Used to access the database repositories</param>
        public RideService(
            IMapper mapper,
            IGoogleMapsApiService googleMapsApiService, 
            IPriceStrategyFactory priceStrategyFactory,
            IUnitOfWork unitOfWork,
            IMatchService matchService)
        {
            _mapper = mapper;
            _googleMapsApiService = googleMapsApiService;
            _priceStrategyFactory = priceStrategyFactory;
            _unitOfWork = unitOfWork;
            _matchService = matchService;
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

            //Reserve the money, create the order and return the created order
            await _unitOfWork.CustomerRepository.ReservePriceFromCustomerAsync(ride.CustomerId, ride.Price);
            ride =  (SoloRide) _unitOfWork.RideRepository.Add(ride);
            
            var order =  _unitOfWork.OrderRepository.Add(new Order());
            await _unitOfWork.OrderRepository.AddRideToOrderAsync(ride, order);
            await _unitOfWork.SaveChangesAsync();

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
            //Map request to ride
            var ride = _mapper.Map<SharedRide>(request);

            //Calculate price and attach customer
            ride.Price = await CalculatePriceAsync(ride.StartDestination, ride.EndDestination, request.RideType);
            ride.CustomerId = customerId;

            //Reserve the money and save the data to the database. 
            await _unitOfWork.CustomerRepository.ReservePriceFromCustomerAsync(ride.CustomerId, ride.Price);
            _unitOfWork.RideRepository.Add(ride);
            await _unitOfWork.SaveChangesAsync();

            //Check for matches
            await CheckForMatches(ride);

            //No match
            var response = _mapper.Map<CreateRideResponse>(ride);
            return response;
        }

        /// <summary>
        /// Checks against the database to see if any rides match. 
        /// </summary>
        /// <param name="ride"></param>
        /// <returns></returns>
        private async Task CheckForMatches(Ride ride)
        {
            var openSharedRides = await _unitOfWork.RideRepository.FindUnmatchedSharedRides();

            //Match against original
            foreach (var openSharedRide in openSharedRides)
            {
                //Check if it's self first or same customer
                if (ride.Id == openSharedRide.Id || ride.CustomerId == openSharedRide.CustomerId)
                {
                    continue;
                }

                //If it matches close enough.
                if (_matchService.Match(ride, openSharedRide,1))
                {
                    //Opdate statuses of rides
                    await CreateOrderForMatchedRide(ride, openSharedRide);
                }
            }
        }

        /// <summary>
        /// Opdate the given rides to WaitinForAccept and creates the order. 
        /// </summary>
        /// <param name="ride1"></param>
        /// <param name="ride2"></param>
        /// <returns></returns>
        private async Task CreateOrderForMatchedRide(Ride ride1, Ride ride2)
        {
            ride1.Status = RideStatus.WaitingForAccept;
            _unitOfWork.RideRepository.Update(ride1);
            ride2.Status = RideStatus.WaitingForAccept;
            _unitOfWork.RideRepository.Update(ride2);

            //Create order for ride
            var order = _unitOfWork.OrderRepository.Add(new Order());
            await _unitOfWork.OrderRepository.AddRideToOrderAsync(ride1, order);
            await _unitOfWork.OrderRepository.AddRideToOrderAsync(ride2, order);
            await _unitOfWork.SaveChangesAsync();
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
            var validateOrigin = _googleMapsApiService.ValidateAddressAsync(first);
            var validateDestination = _googleMapsApiService.ValidateAddressAsync(second);
            await Task.WhenAll(validateOrigin, validateDestination);

            //Validation ok (otherwise an exception would be thrown above)
            var distanceInKm = await _googleMapsApiService.GetDistanceInKmAsync(firstAsString, secondAsString);

            return distanceInKm;
        }


    }
}