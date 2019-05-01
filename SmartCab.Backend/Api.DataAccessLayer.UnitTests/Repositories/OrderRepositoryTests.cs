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
using Api.DataAccessLayer.UnitTests.Fakes;
using CustomExceptions;
using NSubstitute;
using NUnit.Framework;
using NUnit.Framework.Internal;

namespace Api.DataAccessLayer.UnitTests.Repositories
{
    [TestFixture]
    class OrderRepositoryTests
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

        #region AddRideToOrderAsync

        [Test]
        public async Task AddRideToOrder_OrderAndRideExists_1RideAddedToOrder()
        {
            Customer customer = new Customer();
            Order order = new Order();
            using (var context = _factory.CreateContext())
            {
                context.Customers.Add(customer);
                context.Orders.Add(order);
                context.SaveChanges();
            }

            SoloRide soloRide = new SoloRide()
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
            };
            await _uut.OrderRepository.AddRideToOrderAsync(soloRide, order);
            await _uut.SaveChangesAsync();

            using (var context = _factory.CreateContext())
            {
                Assert.That(context.Orders.Find(order.Id).Rides.Count, Is.EqualTo(1));
            }
        }

        [Test]
        public async Task AddRideToOrder_RideAlreadyAddedToOrder_ThrowsException()
        {
            Customer customer = new Customer();
            Order order = new Order();
            using (var context = _factory.CreateContext())
            {
                context.Customers.Add(customer);
                context.Orders.Add(order);
                context.SaveChanges();
            }

            SoloRide soloRide = new SoloRide()
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
            };
            await _uut.OrderRepository.AddRideToOrderAsync(soloRide, order);
            await _uut.SaveChangesAsync();
            Assert.ThrowsAsync<MultipleOrderException>(()=>_uut.OrderRepository.AddRideToOrderAsync(soloRide,order));
        }

        [Test]
        public async Task AddRideToOrder_OrderAndRideExistsNotSaved_RideNotAddedToOrder()
        {
            Customer customer = new Customer();
            Order order = new Order();
            using (var context = _factory.CreateContext())
            {
                context.Customers.Add(customer);
                context.Orders.Add(order);
                context.SaveChanges();
            }

            SoloRide soloRide = new SoloRide()
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
            };

            await _uut.OrderRepository.AddRideToOrderAsync(soloRide, order);

            using (var context = _factory.CreateContext())
            {
                Assert.That(context.Orders.Find(order.Id).Rides.Count, Is.EqualTo(0));
            }
            ;
        }


        #endregion


        #region GetOpenOrdersAsync


        [Test]
        public async Task GetOpenOrdersAsync_SingleMatchedRideInData_Returns1Ride()
        {
            using (var context = _factory.CreateContext())
            {
                Order order = new Order()
                {
                    Status = OrderStatus.WaitingForAccept
                };
                context.Orders.Add(order);
                context.SaveChanges();
            }

            var orders = await _uut.OrderRepository.FindOpenOrdersAsync();
            
            Assert.That(orders.Count, Is.EqualTo(1));
        }

        [Test]
        public async Task GetOpenOrdersAsync_MultipleMatchedRideInData_ReturnsMultipleRides()
        {
            using (var context = _factory.CreateContext())
            {
                for (int x = 0; x < 5; x++)
                {
                    Order order = new Order()
                    {
                        Status = OrderStatus.WaitingForAccept
                    };
                    context.Orders.Add(order);
                    context.SaveChanges();
                }
            }

            var orders = await _uut.OrderRepository.FindOpenOrdersAsync();

            Assert.That(orders.Count, Is.EqualTo(5));
        }

        [Test]
        public async Task GetOpenOrdersAsync_SingleMatchedRideInData_HasSameId()
        {
            Order order = new Order()
            {
                Status = OrderStatus.WaitingForAccept
            };
            using (var context = _factory.CreateContext())
            {
                context.Orders.Add(order);
                context.SaveChanges();
            }

            var orders = await _uut.OrderRepository.FindOpenOrdersAsync();

            Assert.That(orders.First().Id, Is.EqualTo(order.Id));
        }

        [Test]
        public async Task GetOpenOrdersAsync_NoOrder_Returns0()
        {
            var orders = await _uut.OrderRepository.FindOpenOrdersAsync();

            Assert.That(orders.Count, Is.EqualTo(0));
        }

        [Test]
        public async Task GetOpenOrdersAsync_ContainsOnlyAcceptedRides_Returns0Ride()
        {
            using (var context = _factory.CreateContext())
            {
                Order order = new Order()
                {
                    Status = OrderStatus.Accepted
                };
                context.Orders.Add(order);
                context.SaveChanges();
            }

            var orders = await _uut.OrderRepository.FindOpenOrdersAsync();

            Assert.That(orders.Count, Is.EqualTo(0));
        }

        [TestCase(OrderStatus.Accepted)] 
        public async Task GetOpenOrdersAsync_RideStatusCodeIsNotLookingForMatch_Returns0Ride(OrderStatus status)
        {
            using (var context = _factory.CreateContext())
            {
                Order order = new Order()
                {
                    Status = status
                };
                context.Orders.Add(order);
                context.SaveChanges();
            }

            var orders = await _uut.OrderRepository.FindOpenOrdersAsync();

            Assert.That(orders.Count, Is.EqualTo(0));
        }

        [Test]
        public async Task GetOpenOrdersAsync_SomeRideWithStatusAndSomeWithout_Returns2Ride()
        {
            using (var context = _factory.CreateContext())
            {
                Order order1 = new Order()
                {
                    Status = OrderStatus.WaitingForAccept
                };
                Order order2 = new Order()
                {
                    Status = OrderStatus.Accepted
                };

                Order order3 = new Order()
                {
                    Status = OrderStatus.Accepted
                };
                Order order4 = new Order()
                {
                    Status = OrderStatus.WaitingForAccept
                };
                context.Orders.Add(order1);
                context.Orders.Add(order2);
                context.Orders.Add(order3);
                context.Orders.Add(order4);
                context.SaveChanges();
            }

            var orders = await _uut.OrderRepository.FindOpenOrdersAsync();

            Assert.That(orders.Count, Is.EqualTo(2));
        }

        [Test]
        public async Task GetOpenOrdersAsync_Only1Ride_ContainsExpectedPrice()
        {
            using (var context = _factory.CreateContext())
            {
                Order matchedRide = new Order()
                {
                    Status = OrderStatus.WaitingForAccept,
                    Price = 100,
                };
                context.Orders.Add(matchedRide);
                context.SaveChanges();
            }

            var orders = await _uut.OrderRepository.FindOpenOrdersAsync();

            Assert.That(orders.FirstOrDefault().Price, Is.EqualTo(100));
        }

        [Test]
        public async Task GetOpenOrdersAsync_Only1Ride_Contains1Ride()
        {
            var orderInDB = CreateTestOrderWithSoloRideInDatabase();

            var orders = await _uut.OrderRepository.FindOpenOrdersAsync();

            Assert.That(orders.FirstOrDefault().Rides.Count, Is.EqualTo(1));
        }

        [Test]
        public async Task GetOpenOrdersAsync_Only1Ride_ContainsOfTypeSoloRide()
        {
            var order = CreateTestOrderWithSoloRideInDatabase();

            var orders = await _uut.OrderRepository.FindOpenOrdersAsync();

            var rideInOrder = orders.FirstOrDefault().Rides.FirstOrDefault();

            using (var context = _factory.CreateContext())
            {
                var rideInDB = context.SoloRides.Find(rideInOrder.Id);
                Assert.That(rideInOrder.Id,Is.EqualTo(rideInDB.Id));
            }
        }

        [Test]
        public async Task GetOpenOrdersAsync_SharedRide_Contains2Ride()
        {
            var orderInDB = CreateTestOrderWithSharedRideInDatabase();

            var orders = await _uut.OrderRepository.FindOpenOrdersAsync();

            Assert.That(orders.FirstOrDefault().Rides.Count, Is.EqualTo(2));
        }

        private Order CreateTestOrderWithSoloRideInDatabase(RideStatus rideStatus = RideStatus.WaitingForAccept, OrderStatus orderStatus = OrderStatus.WaitingForAccept)
        {
            using (var context = _factory.CreateContext())
            {
                Customer customer = new Customer()
                {
                    Name = "Name"
                };
                context.Customers.Add(customer);
                SoloRide soloRide = new SoloRide()
                {
                    CustomerId = customer.Id,
                    DepartureTime = DateTime.Now,
                    ConfirmationDeadline = DateTime.Now,
                    PassengerCount = 0,
                    CreatedOn = DateTime.Now,
                    Price = 100,
                    Status = rideStatus,
                    EndDestination = new Address("City", 8200, "Street", 21),
                    StartDestination = new Address("City", 8200, "Street", 21)
                };
                context.SoloRides.Add(soloRide);
                Order order = new Order()
                {
                    Status = orderStatus,
                    Price = 100,
                    Rides = new List<Ride>()
                };
                order.Rides.Add(soloRide);
                context.Orders.Add(order);
                context.SaveChanges();
                return order;
            }
        }

        private Order CreateTestOrderWithSharedRideInDatabase()
        {
            using (var context = _factory.CreateContext())
            {
                Customer customer = new Customer()
                {
                    Name = "Name"
                };
                context.Customers.Add(customer);
                SharedRide sharedRide1 = new SharedRide()
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
                };

                SharedRide sharedRide2 = new SharedRide()
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
                };
                context.SharedRides.Add(sharedRide1);
                context.SharedRides.Add(sharedRide2);
                Order order = new Order()
                {
                    Status = OrderStatus.WaitingForAccept,
                    Price = 100,
                    Rides = new List<Ride>()
                };
                order.Rides.Add(sharedRide1);
                order.Rides.Add(sharedRide2);
                context.Orders.Add(order);
                context.SaveChanges();
                return order;
            }
        }

        #endregion


        #region SetOrderToAccepted

        [Test]
        public async Task SetOrderToAccepted_OrderExistsSoloRideWaitingForAcceptStatus_OrderIsAccepted()
        {
            var taxi = new TaxiCompany();
            using (var context = _factory.CreateContext())
            {
                context.TaxiCompanies.Add(taxi);
                context.SaveChanges();
            }

            var orderCreated = CreateTestOrderWithSoloRideInDatabase();
            var order =  _uut.OrderRepository.SetOrderToAccepted(orderCreated, taxi.Id); ;
            Assert.That(order.Status,Is.EqualTo(OrderStatus.Accepted));
        }

        [Test]
        public async Task SetOrderToAccepted_OrderExistsSoloRideWaitingForAcceptStatus_OrderIsAcceptedByExpectedId()
        {
            var taxi = new TaxiCompany();
            using (var context = _factory.CreateContext())
            {
                context.TaxiCompanies.Add(taxi);
                context.SaveChanges();
            }

            var orderCreated = CreateTestOrderWithSoloRideInDatabase();
            var order =  _uut.OrderRepository.SetOrderToAccepted(orderCreated, taxi.Id); ;
            await _uut.SaveChangesAsync();
            using (var context = _factory.CreateContext())
            {
                Assert.That(context.Orders.Find(order.Id).TaxiCompanyId, Is.EqualTo(taxi.Id));
            }
        }

        [Test]
        public async Task AcceptOrder_OrderExistsSoloRideNotWaitingForAcceptstatus_ThrowsException()
        {
            var taxi = new TaxiCompany();
            using (var context = _factory.CreateContext())
            {
                context.TaxiCompanies.Add(taxi);
                context.SaveChanges();
            }

            var order = new Order()
            {
                Status = OrderStatus.Accepted
            };
            Assert.ThrowsAsync<UnexpectedStatusException>(async()=>  _uut.OrderRepository.SetOrderToAccepted(order, taxi.Id)); ;
        }
        #endregion

        #region SetOrderToDebited


        [Test]
        public async Task SetOrderToDebited_OrderExistsSoloRideAccepted_OrderIsDebited()
        {
            var taxi = new TaxiCompany();
            using (var context = _factory.CreateContext())
            {
                context.TaxiCompanies.Add(taxi);
                context.SaveChanges();
            }

            var orderCreated = CreateTestOrderWithSoloRideInDatabase();
            orderCreated.Status = OrderStatus.Accepted;
            _uut.OrderRepository.SetOrderToDebited(orderCreated);
            Assert.That(orderCreated.Status, Is.EqualTo(OrderStatus.Debited));
        }

        [Test]
        public async Task SetOrderToDebited_OrderExistsSoloRideAcceptedSaveChangesNotCalled_OrderIsStillAccepted()
        {
            var taxi = new TaxiCompany();
            using (var context = _factory.CreateContext())
            {
                context.TaxiCompanies.Add(taxi);
                context.SaveChanges();
            }

            var orderCreated = CreateTestOrderWithSoloRideInDatabase();
            using (var context = _factory.CreateContext())
            {
                orderCreated.Status = OrderStatus.Accepted;
                context.Orders.Update(orderCreated);
                context.SaveChanges();
            }

            
            _uut.OrderRepository.SetOrderToDebited(orderCreated);
            await _uut.SaveChangesAsync();
            //Assert against database

            using (var context = _factory.CreateContext())
            {
                Assert.That(context.Orders.Find(orderCreated.Id).Status, Is.EqualTo(OrderStatus.Debited));
            }
        }

        [Test]
        public async Task SetOrderToDebited_OrderExistsSoloRideNotWaitingForAcceptstatus_ThrowsException()
        {
            var taxi = new TaxiCompany();
            using (var context = _factory.CreateContext())
            {
                context.TaxiCompanies.Add(taxi);
                context.SaveChanges();
            }

            var order = new Order()
            {
                Status = OrderStatus.Debited
            };
            Assert.ThrowsAsync<UnexpectedStatusException>(async () => _uut.OrderRepository.SetOrderToAccepted(order, taxi.Id)); ;
        }

        #endregion

    }
}
