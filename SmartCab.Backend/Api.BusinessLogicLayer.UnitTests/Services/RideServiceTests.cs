using System.Threading.Tasks;
using Api.BusinessLogicLayer.Interfaces;
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

        [SetUp]
        public void Setup()
        {
            _mapper = Substitute.For<IMapper>();
            _googleMapsApiService = Substitute.For<IGoogleMapsApiService>();
            

            _rideRepository = Substitute.For<IRideRepository>();
            _rideService = new RideService(_rideRepository, _mapper, _googleMapsApiService);
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

            var startDestination = new Address("city", 1000, "street", 1);
            var endDestination = new Address("another city", 9999, "another street", 9);

            var price = await _rideService.CalculatePriceAsync(startDestination, endDestination, isShared);

            Assert.That(price, Is.EqualTo(expectedPrice));
        }
    }
}