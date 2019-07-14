using System;
using Api.BusinessLogicLayer.Interfaces;
using Api.BusinessLogicLayer.Services;
using Api.DataAccessLayer.Models;
using NUnit.Framework;

namespace Api.BusinessLogicLayer.UnitTests.Services
{
    [TestFixture]
    public class MatchServiceTests
    {

        private IMatchService _matchService;


        [SetUp]
        public void Setup()
        {
            _matchService = new MatchService();
        }

        [TestCase(10,10,10,10,1, ExpectedResult = true)]
        //10 km
        [TestCase(10.1, 10, 10, 10, 10, ExpectedResult = false)]
        [TestCase(10, 10.1, 10, 10, 10, ExpectedResult = false)]
        [TestCase(10, 10, 10.1, 10, 10, ExpectedResult = false)]
        [TestCase(10, 10, 10, 10.1, 10, ExpectedResult = false)]
        //20 km
        [TestCase(10.1, 10, 10, 10, 20, ExpectedResult = true)]
        [TestCase(10, 10.1, 10, 10, 20, ExpectedResult = true)]
        [TestCase(10, 10, 10.1, 10, 20, ExpectedResult = true)]
        [TestCase(10, 10, 10, 10.1, 20, ExpectedResult = true)]
        //1 km
        [TestCase(10.005, 10, 10, 10, 1, ExpectedResult = true)]
        [TestCase(10, 10.005, 10, 10, 1, ExpectedResult = true)]
        [TestCase(10, 10, 10.005, 10, 1, ExpectedResult = true)]
        [TestCase(10, 10, 10, 10.005, 1, ExpectedResult = true)]
        [TestCase(10.005, 10.005, 10, 10, 1, ExpectedResult = true)]
        [TestCase(10.005, 10.005, 10, 10, 1, ExpectedResult = true)]
        [TestCase(10, 10, 10.005, 10.005, 1, ExpectedResult = true)]
        [TestCase(10, 10, 10.005, 10.005, 1, ExpectedResult = true)]
        public bool Match_WhenEndDestinationsChanges_TheBoolDescribesIfThisIsValid(double lat1, double lat2, double lon1, double lon2, int maxDistance)
        {
            var ride1 = new Ride()
            {
                EndDestination = new Address("Dummy", 0, "Dummy", 0)
                {
                    Lat = lat1,
                    Lng = lon1
                },
                StartDestination = new Address("Dummy", 0, "Dummy", 0)
                {
                Lat = 10,
                Lng = 10
                }
            };
            var ride2 = new Ride()
            {
                EndDestination = new Address("Dummy", 0, "Dummy", 0)
                {
                    Lat = lat2,
                    Lng = lon2
                },
                StartDestination = new Address("Dummy", 0, "Dummy", 0)
                {
                    Lat = 10,
                    Lng = 10
                }
            };
            return _matchService.Match(ride1, ride2, maxDistance);
        }
        [TestCase(10, 10, 10, 10, 1, ExpectedResult = true)]
        //10 km
        [TestCase(10.1, 10, 10, 10, 10, ExpectedResult = false)]
        [TestCase(10, 10.1, 10, 10, 10, ExpectedResult = false)]
        [TestCase(10, 10, 10.1, 10, 10, ExpectedResult = false)]
        [TestCase(10, 10, 10, 10.1, 10, ExpectedResult = false)]
        //20 km
        [TestCase(10.1, 10, 10, 10, 20, ExpectedResult = true)]
        [TestCase(10, 10.1, 10, 10, 20, ExpectedResult = true)]
        [TestCase(10, 10, 10.1, 10, 20, ExpectedResult = true)]
        [TestCase(10, 10, 10, 10.1, 20, ExpectedResult = true)]
        //1 km
        [TestCase(10.005, 10, 10, 10, 1, ExpectedResult = true)]
        [TestCase(10, 10.005, 10, 10, 1, ExpectedResult = true)]
        [TestCase(10, 10, 10.005, 10, 1, ExpectedResult = true)]
        [TestCase(10, 10, 10, 10.005, 1, ExpectedResult = true)]
        [TestCase(10.005, 10.005, 10, 10, 1, ExpectedResult = true)]
        [TestCase(10.005, 10.005, 10, 10, 1, ExpectedResult = true)]
        [TestCase(10, 10, 10.005, 10.005, 1, ExpectedResult = true)]
        [TestCase(10, 10, 10.005, 10.005, 1, ExpectedResult = true)]
        public bool Match_WhenStartDestinationsChanges_TheBoolDescribesIfThisIsValid(double lat1, double lat2, double lon1, double lon2, int maxDistance)
        {
            var ride1 = new Ride()
            {
                EndDestination = new Address("Dummy", 0, "Dummy", 0)
                {
                    Lat = 10,
                    Lng = 10
                },
                StartDestination = new Address("Dummy", 0, "Dummy", 0)
                {
                    Lat = lat1,
                    Lng = lon1
                }
            };
            var ride2 = new Ride()
            {
                EndDestination = new Address("Dummy", 0, "Dummy", 0)
                {
                    Lat = 10,
                    Lng = 10
                },
                StartDestination = new Address("Dummy", 0, "Dummy", 0)
                {
                    Lat = lat2,
                    Lng = lon2
                }
            };
            return _matchService.Match(ride1, ride2, maxDistance);
        }



        [TestCase(0,0,ExpectedResult = true)]
        [TestCase(1,0, ExpectedResult = true)]
        [TestCase(-1,0, ExpectedResult = true)]
        [TestCase(-14,0, ExpectedResult = true)]
        [TestCase(-16, 0, ExpectedResult = false)]
        [TestCase(14, 0, ExpectedResult = true)]
        [TestCase(16, 0, ExpectedResult = false)]
        [TestCase(0, 0, ExpectedResult = true)]
        [TestCase(0, 1, ExpectedResult = true)]
        [TestCase(0, -1, ExpectedResult = true)]
        [TestCase(0, -14, ExpectedResult = true)]
        [TestCase(0, -16, ExpectedResult = false)]
        [TestCase(0, 14, ExpectedResult = true)]
        [TestCase(0, 16, ExpectedResult = false)]
        public bool Match_WhenDepartureTimeIsCloseEnough_ReturnsExpectedResult(int minutesToAdd1, int minutesToAdd2)
        {
            var ride1 = new Ride()
            {
                DepartureTime = DateTime.Now.AddMinutes(minutesToAdd1),
                EndDestination = new Address("Dummy", 0, "Dummy", 0)
                {
                    Lat = 10,
                    Lng = 10
                },
                StartDestination = new Address("Dummy", 0, "Dummy", 0)
                {
                    Lat = 10,
                    Lng = 10
                }
            };
            var ride2 = new Ride()
            {
                DepartureTime = DateTime.Now.AddMinutes(minutesToAdd2),
                EndDestination = new Address("Dummy", 0, "Dummy", 0)
                {
                    Lat = 10,
                    Lng = 10
                },
                StartDestination = new Address("Dummy", 0, "Dummy", 0)
                {
                    Lat = 10,
                    Lng = 10
                }
            };
            return _matchService.Match(ride1, ride2, 1);
        }
    }
}