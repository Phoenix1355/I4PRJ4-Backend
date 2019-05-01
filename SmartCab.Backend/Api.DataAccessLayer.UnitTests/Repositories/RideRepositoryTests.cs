using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Api.DataAccessLayer.Interfaces;
using Api.DataAccessLayer.Models;
using Api.DataAccessLayer.Repositories;
using Api.DataAccessLayer.Statuses;
using Api.DataAccessLayer.UnitOfWork;
using Api.DataAccessLayer.UnitTests.Factories;
using CustomExceptions;
using NSubstitute;
using NUnit.Framework;

namespace Api.DataAccessLayer.UnitTests.Repositories
{
    class RideRepositoryTests
    {

        #region Setup

        private IUnitOfWork _uut;
        private InMemorySqlLiteContextFactory _factory;

        [SetUp]
        public void SetUp()
        {
            _factory = new InMemorySqlLiteContextFactory();
            var identityRepository = Substitute.For<IIdentityUserRepository>();
            _uut = new UnitOfWork.UnitOfWork(_factory.CreateContext(), identityRepository);
        }

        [TearDown]
        public void TearDown()
        {
            _factory.Dispose();
        }

        #endregion

        #region SetAllRidesToAccepted

        [Test]
        public async Task SetAllRidesToAccepted_AllRidesHasStatusWaitingForAccept_UpdateChangesRideToAccepted()
        {
            //Add customer to ensure constraints holds
            var customer = new Customer();


            //Create list
            List<Ride> rides = new List<Ride>()
            {
                new Ride()
                {
                    CustomerId = customer.Id,
                    DepartureTime = DateTime.Now,
                    ConfirmationDeadline = DateTime.Now,
                    PassengerCount = 0,
                    CreatedOn = DateTime.Now,
                    Price = 100,
                    Status = RideStatus.WaitingForAccept,
                    EndDestination = new Address("City", 8200, "Street", 21),
                    StartDestination = new Address("City", 8200, "Street", 21)
                },
                new Ride()
                {
                    CustomerId = customer.Id,
                    DepartureTime = DateTime.Now,
                    ConfirmationDeadline = DateTime.Now,
                    PassengerCount = 0,
                    CreatedOn = DateTime.Now,
                    Price = 100,
                    Status = RideStatus.WaitingForAccept,
                    EndDestination = new Address("City", 8200, "Street", 21),
                    StartDestination = new Address("City", 8200, "Street", 21)
                }
            };

            using (var context = _factory.CreateContext())
            {
                context.Customers.Add(customer);
                context.SaveChanges();
            }

            _uut.RideRepository.SetAllRidesToAccepted(rides);
            await _uut.SaveChangesAsync();
            using (var context = _factory.CreateContext())
            {
                //Dummy expression to get all as a list
                foreach (var ride in context.Rides.Where(x=>x.Price!=-1))
                {
                   Assert.That(ride.Status,Is.EqualTo(RideStatus.Accepted));
                }
            }
        }

        [Test]
        public async Task SetAllRidesToAccepted_NotAllRidesHasWaitingForAccept_ThrowsException()
        {
            //Add customer to ensure constraints holds
            var customer = new Customer();


            //Create list
            List<Ride> rides = new List<Ride>()
            {
                new Ride()
                {
                    CustomerId = customer.Id,
                    DepartureTime = DateTime.Now,
                    ConfirmationDeadline = DateTime.Now,
                    PassengerCount = 0,
                    CreatedOn = DateTime.Now,
                    Price = 100,
                    Status = RideStatus.WaitingForAccept,
                    EndDestination = new Address("City", 8200, "Street", 21),
                    StartDestination = new Address("City", 8200, "Street", 21)
                },
                new Ride()
                {
                    CustomerId = customer.Id,
                    DepartureTime = DateTime.Now,
                    ConfirmationDeadline = DateTime.Now,
                    PassengerCount = 0,
                    CreatedOn = DateTime.Now,
                    Price = 100,
                    Status = RideStatus.Accepted,
                    EndDestination = new Address("City", 8200, "Street", 21),
                    StartDestination = new Address("City", 8200, "Street", 21)
                }
            };

            using (var context = _factory.CreateContext())
            {
                context.Customers.Add(customer);
                context.SaveChanges();
            }

            Assert.Throws<UnexpectedStatusException>(()=>_uut.RideRepository.SetAllRidesToAccepted(rides));
        }


        #endregion


        #region SetAllRidesToDebited

        [Test]
        public async Task SetAllRidesToDebited_AllRidesHasStatusAccepted_UpdateChangesRideToDebited()
        {
            //Add customer to ensure constraints holds
            var customer = new Customer();


            //Create list
            List<Ride> rides = new List<Ride>()
            {
                new Ride()
                {
                    CustomerId = customer.Id,
                    DepartureTime = DateTime.Now,
                    ConfirmationDeadline = DateTime.Now,
                    PassengerCount = 0,
                    CreatedOn = DateTime.Now,
                    Price = 100,
                    Status = RideStatus.Accepted,
                    EndDestination = new Address("City", 8200, "Street", 21),
                    StartDestination = new Address("City", 8200, "Street", 21)
                },
                new Ride()
                {
                    CustomerId = customer.Id,
                    DepartureTime = DateTime.Now,
                    ConfirmationDeadline = DateTime.Now,
                    PassengerCount = 0,
                    CreatedOn = DateTime.Now,
                    Price = 100,
                    Status = RideStatus.Accepted,
                    EndDestination = new Address("City", 8200, "Street", 21),
                    StartDestination = new Address("City", 8200, "Street", 21)
                }
            };

            using (var context = _factory.CreateContext())
            {
                context.Customers.Add(customer);
                context.SaveChanges();
            }

            _uut.RideRepository.SetAllRidesToDebited(rides);
            await _uut.SaveChangesAsync();
            using (var context = _factory.CreateContext())
            {
                //Dummy expression to get all as a list
                foreach (var ride in context.Rides.Where(x => x.Price != -1))
                {
                    Assert.That(ride.Status, Is.EqualTo(RideStatus.Debited));
                }
            }
        }

        [Test]
        public async Task SetAllRidesToDebited_NotAllRidesHasAcceptedStatus_ThrowsException()
        {
            //Add customer to ensure constraints holds
            var customer = new Customer();


            //Create list
            List<Ride> rides = new List<Ride>()
            {
                new Ride()
                {
                    CustomerId = customer.Id,
                    DepartureTime = DateTime.Now,
                    ConfirmationDeadline = DateTime.Now,
                    PassengerCount = 0,
                    CreatedOn = DateTime.Now,
                    Price = 100,
                    Status = RideStatus.Debited,
                    EndDestination = new Address("City", 8200, "Street", 21),
                    StartDestination = new Address("City", 8200, "Street", 21)
                },
                new Ride()
                {
                    CustomerId = customer.Id,
                    DepartureTime = DateTime.Now,
                    ConfirmationDeadline = DateTime.Now,
                    PassengerCount = 0,
                    CreatedOn = DateTime.Now,
                    Price = 100,
                    Status = RideStatus.Accepted,
                    EndDestination = new Address("City", 8200, "Street", 21),
                    StartDestination = new Address("City", 8200, "Street", 21)
                }
            };

            using (var context = _factory.CreateContext())
            {
                context.Customers.Add(customer);
                context.SaveChanges();
            }

            Assert.Throws<UnexpectedStatusException>(() => _uut.RideRepository.SetAllRidesToAccepted(rides));
        }

        #endregion
        #region FindUnmatchedSharedRides

        [Test]
        public async Task FindUnmatchedSharedRides_NoRides_ReturnsEmptyList()
        {
            var rides = await _uut.RideRepository.FindUnmatchedSharedRides();
            Assert.That(rides,Is.Empty);
        }

        [Test]
        public async Task FindUnmatchedSharedRides_RideWithDifferentStatuss_ReturnsOnlyRidesWithLookingForMatchStatus()
        {
            var customer = new Customer();
            var soloRide1 = new SoloRide()
            {
                CustomerId = customer.Id,
                DepartureTime = DateTime.Now,
                ConfirmationDeadline = DateTime.Now,
                PassengerCount = 0,
                CreatedOn = DateTime.Now,
                Price = 100,
                Status = RideStatus.Accepted,
                EndDestination = new Address("City", 8200, "Street", 21),
                StartDestination = new Address("City", 8200, "Street", 21)
            };
            var soloRide2 = new SoloRide()
            {
                CustomerId = customer.Id,
                DepartureTime = DateTime.Now,
                ConfirmationDeadline = DateTime.Now,
                PassengerCount = 0,
                CreatedOn = DateTime.Now,
                Price = 100,
                Status = RideStatus.LookingForMatch,
                EndDestination = new Address("City", 8200, "Street", 21),
                StartDestination = new Address("City", 8200, "Street", 21)
            };
            var soloRide3 = new SoloRide()
            {
                CustomerId = customer.Id,
                DepartureTime = DateTime.Now,
                ConfirmationDeadline = DateTime.Now,
                PassengerCount = 0,
                CreatedOn = DateTime.Now,
                Price = 100,
                Status = RideStatus.Debited,
                EndDestination = new Address("City", 8200, "Street", 21),
                StartDestination = new Address("City", 8200, "Street", 21)
            };
            var soloRide4 = new SoloRide()
            {
                CustomerId = customer.Id,
                DepartureTime = DateTime.Now,
                ConfirmationDeadline = DateTime.Now,
                PassengerCount = 0,
                CreatedOn = DateTime.Now,
                Price = 100,
                Status = RideStatus.Expired,
                EndDestination = new Address("City", 8200, "Street", 21),
                StartDestination = new Address("City", 8200, "Street", 21)
            };
            var soloRide5 = new SoloRide()
            {
                CustomerId = customer.Id,
                DepartureTime = DateTime.Now,
                ConfirmationDeadline = DateTime.Now,
                PassengerCount = 0,
                CreatedOn = DateTime.Now,
                Price = 100,
                Status = RideStatus.LookingForMatch,
                EndDestination = new Address("City", 8200, "Street", 21),
                StartDestination = new Address("City", 8200, "Street", 21)
            };
            using (var context = _factory.CreateContext())
            {
                context.Add(customer);
                context.Add(soloRide1);
                context.Add(soloRide2);
                context.Add(soloRide3);
                context.Add(soloRide4);
                context.Add(soloRide5);
                context.SaveChanges();
            }
            var rides = await _uut.RideRepository.FindUnmatchedSharedRides();
            foreach (var ride in rides)
            {
                Assert.That(ride.Status,Is.EqualTo(RideStatus.LookingForMatch));
            }
            
        }

        [Test]
        public async Task FindUnmatchedSharedRides_RideWithDifferentStatuss_ReturnsCountCorrect()
        {
            var customer = new Customer();
            var soloRide1 = new SoloRide()
            {
                CustomerId = customer.Id,
                DepartureTime = DateTime.Now,
                ConfirmationDeadline = DateTime.Now,
                PassengerCount = 0,
                CreatedOn = DateTime.Now,
                Price = 100,
                Status = RideStatus.Accepted,
                EndDestination = new Address("City", 8200, "Street", 21),
                StartDestination = new Address("City", 8200, "Street", 21)
            };
            var soloRide2 = new SoloRide()
            {
                CustomerId = customer.Id,
                DepartureTime = DateTime.Now,
                ConfirmationDeadline = DateTime.Now,
                PassengerCount = 0,
                CreatedOn = DateTime.Now,
                Price = 100,
                Status = RideStatus.LookingForMatch,
                EndDestination = new Address("City", 8200, "Street", 21),
                StartDestination = new Address("City", 8200, "Street", 21)
            };
            var soloRide3 = new SoloRide()
            {
                CustomerId = customer.Id,
                DepartureTime = DateTime.Now,
                ConfirmationDeadline = DateTime.Now,
                PassengerCount = 0,
                CreatedOn = DateTime.Now,
                Price = 100,
                Status = RideStatus.Debited,
                EndDestination = new Address("City", 8200, "Street", 21),
                StartDestination = new Address("City", 8200, "Street", 21)
            };
            var soloRide4 = new SoloRide()
            {
                CustomerId = customer.Id,
                DepartureTime = DateTime.Now,
                ConfirmationDeadline = DateTime.Now,
                PassengerCount = 0,
                CreatedOn = DateTime.Now,
                Price = 100,
                Status = RideStatus.Expired,
                EndDestination = new Address("City", 8200, "Street", 21),
                StartDestination = new Address("City", 8200, "Street", 21)
            };
            var soloRide5 = new SoloRide()
            {
                CustomerId = customer.Id,
                DepartureTime = DateTime.Now,
                ConfirmationDeadline = DateTime.Now,
                PassengerCount = 0,
                CreatedOn = DateTime.Now,
                Price = 100,
                Status = RideStatus.LookingForMatch,
                EndDestination = new Address("City", 8200, "Street", 21),
                StartDestination = new Address("City", 8200, "Street", 21)
            };
            using (var context = _factory.CreateContext())
            {
                context.Add(customer);
                context.Add(soloRide1);
                context.Add(soloRide2);
                context.Add(soloRide3);
                context.Add(soloRide4);
                context.Add(soloRide5);
                context.SaveChanges();
            }
            var rides = await _uut.RideRepository.FindUnmatchedSharedRides();
            Assert.That(rides.Count, Is.EqualTo(2));
        }
        #endregion
    }
}
