using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using Api.BusinessLogicLayer.Enums;
using Api.BusinessLogicLayer.Helpers;
using Api.BusinessLogicLayer.Interfaces;
using Api.BusinessLogicLayer.Requests;
using Api.BusinessLogicLayer.Responses;
using Api.BusinessLogicLayer.Services;
using Api.DataAccessLayer.Interfaces;
using Api.DataAccessLayer.Models;
using Api.DataAccessLayer.UnitOfWork;
using Api.DataAccessLayer.Statuses;
using AutoMapper;
using NSubstitute;
using NUnit.Framework;

namespace Api.BusinessLogicLayer.UnitTests.Services
{
    [TestFixture]
    public class RideServiceTests
    {
        private IMapper _mapper;
        private IGoogleMapsApiService _googleMapsApiService;
        private IRideRepository _rideRepository;
        private IPriceStrategy _soloRidePriceStrategy;
        private IPriceStrategy _sharedRidePriceStrategy;
        private IPriceStrategyFactory _priceStrategyFactory;
        private RideService _rideService;
        private IUnitOfWork _unitOfWork;
        private Address _anAddress; //An address object to be reused throughout the tests

        [SetUp]
        public void Setup()
        {
            _mapper = Substitute.For<IMapper>();
            _googleMapsApiService = Substitute.For<IGoogleMapsApiService>();
            _rideRepository = Substitute.For<IRideRepository>();
            _soloRidePriceStrategy = Substitute.For<IPriceStrategy>();
            _sharedRidePriceStrategy = Substitute.For<IPriceStrategy>();
            _priceStrategyFactory = Substitute.For<IPriceStrategyFactory>();
            _unitOfWork = Substitute.For<IUnitOfWork>();
            _rideService = new RideService(_mapper, _googleMapsApiService, _priceStrategyFactory, _unitOfWork);
            _anAddress = new Address("city", 1000, "street", 1);
        }

        [Test]
        public async Task CalculatePriceAsync_WhenRideIsASoloRide_ReturnsTheCorrectPrice()
        {
            decimal distance = 10;
            decimal calculatedPrice = 100;

            _googleMapsApiService.GetDistanceInKmAsync(Arg.Any<string>(), Arg.Any<string>())
                .ReturnsForAnyArgs(distance);

            _soloRidePriceStrategy.CalculatePrice(distance).Returns(calculatedPrice);
            _priceStrategyFactory.GetPriceStrategy(Arg.Any<RideType>()).Returns(_soloRidePriceStrategy);

            decimal price = await _rideService.CalculatePriceAsync(_anAddress, _anAddress, RideType.SoloRide);

            Assert.That(price, Is.EqualTo(calculatedPrice));
        }

        [Test]
        public async Task CalculatePriceAsync_WhenRideIsASharedRide_ReturnsTheCorrectPrice()
        {
            decimal distance = 10;
            decimal calculatedPrice = 75;

            _googleMapsApiService.GetDistanceInKmAsync(Arg.Any<string>(), Arg.Any<string>())
                .ReturnsForAnyArgs(distance);

            _sharedRidePriceStrategy.CalculatePrice(distance).Returns(calculatedPrice);
            _priceStrategyFactory.GetPriceStrategy(Arg.Any<RideType>()).Returns(_sharedRidePriceStrategy);

            decimal price = await _rideService.CalculatePriceAsync(_anAddress, _anAddress, RideType.SharedRide);

            Assert.That(price, Is.EqualTo(calculatedPrice));
        }

        [Test]
        public async Task AddRideAsync_WhenRideIsASoloRide_ReturnsACreateRideResponseContainingTheCorrectPrice()
        {
            decimal distance = 10;
            decimal calculatedPrice = 100;

            _googleMapsApiService.GetDistanceInKmAsync(Arg.Any<string>(), Arg.Any<string>())
                .ReturnsForAnyArgs(distance);
            _soloRidePriceStrategy.CalculatePrice(distance).Returns(calculatedPrice); //Stub the solo strategy
            var expectedResponse = new CreateRideResponse {Price = calculatedPrice };
            _mapper.Map<CreateRideResponse>(null).ReturnsForAnyArgs(expectedResponse);
            _mapper.Map<SoloRide>(null)
                .ReturnsForAnyArgs(new SoloRide {StartDestination = _anAddress, EndDestination = _anAddress});
            var request = new CreateRideRequest
            {
                StartDestination = _anAddress,
                EndDestination = _anAddress,
                RideType = RideType.SoloRide
            };

            var response = await _rideService.AddRideAsync(request, "aCustomerId");

            Assert.That(response.Price, Is.EqualTo(calculatedPrice));
        }

        //TODO: Adding a shared ride is not implemented yet, but the test will most likely not change
        //[Test]
        //public async Task AddRideAsync_WhenRideIsASharedRide_ReturnsACreateRideResponseContainingTheCorrectPrice()
        //{
        //    decimal distance = 10;
        //    decimal calculatedPrice = 100;

        //    _googleMapsApiService.GetDistanceInKmAsync(Arg.Any<string>(), Arg.Any<string>())
        //        .ReturnsForAnyArgs(distance);
        //    _sharedRidePriceStrategy.CalculatePrice(distance).Returns(calculatedPrice);
        //    var expectedResponse = new CreateRideResponse { Price = calculatedPrice };
        //    _mapper.Map<CreateRideResponse>(null).ReturnsForAnyArgs(expectedResponse);
        //    _mapper.Map<SoloRide>(null)
        //        .ReturnsForAnyArgs(new SoloRide { StartDestination = _anAddress, EndDestination = _anAddress });
        //    var request = new CreateRideRequest
        //    {
        //        StartDestination = _anAddress,
        //        EndDestination = _anAddress,
        //        IsShared = true
        //    };

        //    var response = await _rideService.AddRideAsync(request, "aCustomerId");

        //    Assert.That(response.Price, Is.EqualTo(calculatedPrice));
        //}
    }
}