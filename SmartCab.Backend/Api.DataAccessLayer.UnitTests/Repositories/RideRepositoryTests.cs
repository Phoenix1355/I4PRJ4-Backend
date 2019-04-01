using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Api.DataAccessLayer.Models;
using Api.DataAccessLayer.Repositories;
using Api.DataAccessLayer.Statuses;
using Api.DataAccessLayer.UnitTests.Factories;
using Api.DataAccessLayer.UnitTests.Fakes;
using NUnit.Framework;
using NUnit.Framework.Internal;

namespace Api.DataAccessLayer.UnitTests.Repositories
{
    [TestFixture]
    class RideRepositoryTests
    {
        #region Setup

        private RideRepository _uut;
        private InMemorySqlLiteContextFactory _factory;

        [SetUp]
        public void SetUp()
        {
            _factory = new InMemorySqlLiteContextFactory();
            _uut = new RideRepository(_factory.CreateContext());
        }

        [TearDown]
        public void TearDown()
        {
            _factory.Dispose();
        }
        //GetOpenSoloRide
        #endregion

        #region MatchedRides


        [Test]
        public void GetOpenMatchedRides_SingleMatchedRideInData_Returns1Ride()
        {
            using (var context = _factory.CreateContext())
            {
                MatchedRides matchedRide = new MatchedRides()
                {
                    RideStatus = RideStatus.WaitingForAccept
                };
                context.MatchedRides.Add(matchedRide);
                context.SaveChanges();
            }

            var rides = _uut.GetOpenMatchedRides();
            
            Assert.That(rides.Count, Is.EqualTo(1));
        }

        [Test]
        public void GetOpenMatchedRides_MultipleMatchedRideInData_ReturnsMultipleRides()
        {
            using (var context = _factory.CreateContext())
            {
                for (int x = 0; x < 5; x++)
                {
                    MatchedRides matchedRide = new MatchedRides()
                    {
                        RideStatus = RideStatus.WaitingForAccept
                    };
                    context.MatchedRides.Add(matchedRide);
                    context.SaveChanges();
                }
            }

            var rides = _uut.GetOpenMatchedRides();

            Assert.That(rides.Count, Is.EqualTo(5));
        }

        [Test]
        public void GetOpenMatchedRides_SingleMatchedRideInData_HasSameId()
        {
            MatchedRides matchedRide = new MatchedRides()
            {
                RideStatus = RideStatus.WaitingForAccept
            };
            using (var context = _factory.CreateContext())
            {
                context.MatchedRides.Add(matchedRide);
                context.SaveChanges();
            }

            var rides = _uut.GetOpenMatchedRides();

            Assert.That(rides.First().Id, Is.EqualTo(matchedRide.Id));
        }

        [Test]
        public void GetOpenMatchedRides_NoMatchedRides_Returns0()
        {
            var rides = _uut.GetOpenMatchedRides();

            Assert.That(rides.Count, Is.EqualTo(0));
        }

        [Test]
        public void GetOpenMatchedRides_RideHaveLookingForMatch_Returns0Ride()
        {
            using (var context = _factory.CreateContext())
            {
                MatchedRides matchedRide = new MatchedRides()
                {
                    RideStatus = RideStatus.LookingForMatch
                };
                context.MatchedRides.Add(matchedRide);
                context.SaveChanges();
            }

            var rides = _uut.GetOpenMatchedRides();

            Assert.That(rides.Count, Is.EqualTo(0));
        }

        [TestCase(RideStatus.LookingForMatch)]
        [TestCase(RideStatus.Accepted)]
        [TestCase(RideStatus.Debited)]
        [TestCase(RideStatus.Expired)]
        public void GetOpenMatchedRides_RideStatusCodeIsNotLookingForMatch_Returns0Ride(RideStatus rideStatus)
        {
            using (var context = _factory.CreateContext())
            {
                MatchedRides matchedRide = new MatchedRides()
                {
                    RideStatus = rideStatus
                };
                context.MatchedRides.Add(matchedRide);
                context.SaveChanges();
            }

            var rides = _uut.GetOpenMatchedRides();

            Assert.That(rides.Count, Is.EqualTo(0));
        }

        [Test]
        public void GetOpenMatchedRides_SomeRideWithStatusAndSomeWithout_Returns2Ride()
        {
            using (var context = _factory.CreateContext())
            {
                MatchedRides matchedRide1 = new MatchedRides()
                {
                    RideStatus = RideStatus.LookingForMatch
                };
                MatchedRides matchedRide2 = new MatchedRides()
                {
                    RideStatus = RideStatus.Expired
                };

                MatchedRides matchedRide3 = new MatchedRides()
                {
                    RideStatus = RideStatus.Accepted
                };
                MatchedRides matchedRide4 = new MatchedRides()
                {
                    RideStatus = RideStatus.WaitingForAccept
                };
                MatchedRides matchedRide5 = new MatchedRides()
                {
                    RideStatus = RideStatus.WaitingForAccept
                };
                context.MatchedRides.Add(matchedRide1);
                context.MatchedRides.Add(matchedRide2);
                context.MatchedRides.Add(matchedRide3);
                context.MatchedRides.Add(matchedRide4);
                context.MatchedRides.Add(matchedRide5);
                context.SaveChanges();
            }

            var rides = _uut.GetOpenMatchedRides();

            Assert.That(rides.Count, Is.EqualTo(2));
        }

        #endregion

        #region SoloRides

        [Test]
        public void GetOpenSoloRides_1SoloRideInDatabase_Returns1Ride()
        {
            using (var context = _factory.CreateContext())
            {
                SoloRide soloRide = getSoloRide();
                context.SoloRides.Add(soloRide);
                context.SaveChanges();
            }

            var rides = _uut.GetOpenSoloRides();

            Assert.That(rides.Count, Is.EqualTo(1));
        }

        [Test]
        public void GetOpenSoloRides_0SoloRideInDatabase_Returns0Ride()
        {
            var rides = _uut.GetOpenSoloRides();

            Assert.That(rides.Count, Is.EqualTo(0));
        }

        [Test]
        public void GetOpenSoloRides_5SoloRideInDatabase_Returns5Ride()
        {
            using (var context = _factory.CreateContext())
            {
                for (int x = 0; x < 5; x++)
                {
                    SoloRide soloRide = getSoloRide();
                    context.SoloRides.Add(soloRide);
                    context.SaveChanges();
                }

            }

            var rides = _uut.GetOpenSoloRides();

            Assert.That(rides.Count, Is.EqualTo(5));
        }

        [TestCase(RideStatus.LookingForMatch)]
        [TestCase(RideStatus.Accepted)]
        [TestCase(RideStatus.Debited)]
        [TestCase(RideStatus.Expired)]
        public void GetOpenSoloRides_RideStatusCodeIsNotLookingForMatch_Returns0Ride(RideStatus rideStatus)
        {
            using (var context = _factory.CreateContext())
            {
                SoloRide soloRide = getSoloRide(rideStatus);
                context.SoloRides.Add(soloRide);
                context.SaveChanges();
            }

            var rides = _uut.GetOpenSoloRides();

            Assert.That(rides.Count, Is.EqualTo(0));
        }

        [Test]
        public void GetOpenSoloRides_1SoloRideInDatabase_ReturnsRightIdOfRide()
        {
            SoloRide soloRide = getSoloRide();
            using (var context = _factory.CreateContext())
            {
                    
                    context.SoloRides.Add(soloRide);
                    context.SaveChanges();
            }

            var rides = _uut.GetOpenSoloRides();

            Assert.That(rides.First().Id, Is.EqualTo(soloRide.Id));
        }

        public void GetOpenSoloRides_ContainsRides_SpecificRideWithIdCanBeFound()
        {
            SoloRide soloRideForIdCheck = getSoloRide();
            using (var context = _factory.CreateContext())
            {
                context.SoloRides.Add(soloRideForIdCheck);
                context.SaveChanges();
                for (int x = 0; x < 5; x++)
                {
                    SoloRide soloRide = getSoloRide();
                    context.SoloRides.Add(soloRide);
                    context.SaveChanges();
                }

            }

            var rides = _uut.GetOpenSoloRides();

            Assert.That(rides.Where(x=>soloRideForIdCheck.Id == x.Id).Count, Is.EqualTo(1));
        }


        private SoloRide getSoloRide(RideStatus rideStatus = RideStatus.WaitingForAccept)
        {
            return new SoloRide()
            {
                RideStatus = rideStatus,
                Price = 100,
                CountPassengers = 2,
                CreatedAt = DateTime.Now,
                DepartureTime = DateTime.Now,
                LatestConfirmed = DateTime.Now,
                StartDestination = new Address("City ", 8210, "Street", 23),
                SlutDestination = new Address("City ", 8210, "Street", 23)
            };
        }
        #endregion
    }
}
