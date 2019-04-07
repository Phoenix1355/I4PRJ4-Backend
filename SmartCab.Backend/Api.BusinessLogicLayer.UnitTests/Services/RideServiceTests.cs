using System.Threading.Tasks;
using Api.BusinessLogicLayer.Interfaces;
using Api.BusinessLogicLayer.Requests;
using Api.BusinessLogicLayer.Responses;
using Api.BusinessLogicLayer.Services;
using Api.DataAccessLayer.Interfaces;
using Api.DataAccessLayer.Models;
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
        private RideService _rideService;

        private Address _anAddress; //An address object to be reused throughout the tests

        [SetUp]
        public void Setup()
        {
            _mapper = Substitute.For<IMapper>();
            _googleMapsApiService = Substitute.For<IGoogleMapsApiService>();
            _rideRepository = Substitute.For<IRideRepository>();
            _rideService = new RideService(_rideRepository, _mapper, _googleMapsApiService);
            _anAddress = new Address("city", 1000, "street", 1);
        }

        [TestCase(false, 0.1, 1)]
        [TestCase(false, 1, 10)]
        [TestCase(false, 5, 50)]
        [TestCase(false, 10.9, 109)]
        [TestCase(false, 50, 500)]
        [TestCase(false, 1000.1, 10001)]
        [TestCase(true, 0.1, 0.75)]
        [TestCase(true, 1, 7.5)]
        [TestCase(true, 5, 37.5)]
        [TestCase(true, 10.9, 81.75)]
        [TestCase(true, 50, 375)]
        [TestCase(true, 1000.1, 7500.75)]
        public async Task CalculatePriceAsync_WhenCalled_CalculatesTheCorrectPrice(bool isShared, decimal distance, decimal expectedPrice)
        {
            _googleMapsApiService.GetDistanceInKmAsync(Arg.Any<string>(), Arg.Any<string>()).ReturnsForAnyArgs(distance);

            var price = await _rideService.CalculatePriceAsync(_anAddress, _anAddress, isShared);

            Assert.That(price, Is.EqualTo(expectedPrice));
        }

        [TestCase(false, 0.1, 1)]
        [TestCase(false, 1, 10)]
        [TestCase(false, 5, 50)]
        [TestCase(false, 10.9, 109)]
        [TestCase(false, 50, 500)]
        [TestCase(false, 1000.1, 10001)]
        //[TestCase(true, 0.1, 0.75)] //sharedrideasync is not implemented yet
        //[TestCase(true, 1, 7.5)]
        //[TestCase(true, 5, 37.5)]
        //[TestCase(true, 10.9, 81.75)]
        //[TestCase(true, 50, 375)]
        //[TestCase(true, 1000.1, 7500.75)]
        public async Task AddRideAsync_WhenCalled_ReturnsACreateRideResponseContainingTheCorrectPrice(bool isShared, decimal distance, decimal expectedPrice)
        {
            _googleMapsApiService.GetDistanceInKmAsync(Arg.Any<string>(), Arg.Any<string>()).ReturnsForAnyArgs(distance);
            var expectedResponse = new CreateRideResponse {Price = expectedPrice};
            _mapper.Map<CreateRideResponse>(null).ReturnsForAnyArgs(expectedResponse);
            _mapper.Map<SoloRide>(null).ReturnsForAnyArgs(new SoloRide {StartDestination = _anAddress, EndDestination = _anAddress});
            var request = new CreateRideRequest
            {
                StartDestination = _anAddress,
                EndDestination = _anAddress,
                IsShared = isShared
            };

            var response = await _rideService.AddRideAsync(request, "aCustomerId");

            Assert.That(response.Price, Is.EqualTo(expectedPrice));
        }
    }
}