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
    class OrderRepositoryTests
    {
        /*
        #region Setup

        private OrderRepository _uut;
        private InMemorySqlLiteContextFactory _factory;

        [SetUp]
        public void SetUp()
        {
            _factory = new InMemorySqlLiteContextFactory();
            _uut = new OrderRepository(_factory.CreateContext());
        }

        [TearDown]
        public void TearDown()
        {
            _factory.Dispose();
        }

        #endregion
        
        #region GetOpenOrdersAsync


        [Test]
        public void GetOpenOrdersAsync_SingleMatchedRideInData_Returns1Ride()
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

            var orders = _uut.GetOpenOrdersAsync().Result;
            
            Assert.That(orders.Count, Is.EqualTo(1));
        }

        [Test]
        public void GetOpenOrdersAsync_MultipleMatchedRideInData_ReturnsMultipleRides()
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

            var orders = _uut.GetOpenOrdersAsync().Result;

            Assert.That(orders.Count, Is.EqualTo(5));
        }

        [Test]
        public void GetOpenOrdersAsync_SingleMatchedRideInData_HasSameId()
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

            var orders = _uut.GetOpenOrdersAsync().Result;

            Assert.That(orders.First().Id, Is.EqualTo(order.Id));
        }

        [Test]
        public void GetOpenOrdersAsync_NoOrder_Returns0()
        {
            var orders = _uut.GetOpenOrdersAsync().Result;

            Assert.That(orders.Count, Is.EqualTo(0));
        }

        [Test]
        public void GetOpenOrdersAsync_ContainsOnlyAcceptedRides_Returns0Ride()
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

            var orders = _uut.GetOpenOrdersAsync().Result;

            Assert.That(orders.Count, Is.EqualTo(0));
        }

        [TestCase(OrderStatus.Accepted)] 
        public void GetOpenOrdersAsync_RideStatusCodeIsNotLookingForMatch_Returns0Ride(OrderStatus status)
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

            var orders = _uut.GetOpenOrdersAsync().Result;

            Assert.That(orders.Count, Is.EqualTo(0));
        }

        [Test]
        public void GetOpenOrdersAsync_SomeRideWithStatusAndSomeWithout_Returns2Ride()
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

            var orders = _uut.GetOpenOrdersAsync().Result;

            Assert.That(orders.Count, Is.EqualTo(2));
        }

        [Test]
        public void GetOpenOrdersAsync_Only1Ride_ContainsExpectedPrice()
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

            var orders = _uut.GetOpenOrdersAsync().Result;

            Assert.That(orders.FirstOrDefault().Price, Is.EqualTo(100));
        }

        [Test]
        public void GetOpenOrdersAsync_Only1Ride_Contains1Ride()
        {
            var orderInDB = CreateTestOrderWithSoloRideInDatabase();

            var orders = _uut.GetOpenOrdersAsync().Result;

            Assert.That(orders.FirstOrDefault().Rides.Count, Is.EqualTo(1));
        }

        [Test]
        public void GetOpenOrdersAsync_Only1Ride_ContainsOfTypeSoloRide()
        {
            var order = CreateTestOrderWithSoloRideInDatabase();

            var orders = _uut.GetOpenOrdersAsync().Result;

            var rideInOrder = orders.FirstOrDefault().Rides.FirstOrDefault();

            using (var context = _factory.CreateContext())
            {
                var rideInDB = context.SoloRides.Find(rideInOrder.Id);
                Assert.That(rideInOrder.Id,Is.EqualTo(rideInDB.Id));
            }
        }

        [Test]
        public void GetOpenOrdersAsync_SharedRide_Contains2Ride()
        {
            var orderInDB = CreateTestOrderWithSharedRideInDatabase();

            var orders = _uut.GetOpenOrdersAsync().Result;

            Assert.That(orders.FirstOrDefault().Rides.Count, Is.EqualTo(2));
        }

        private Order CreateTestOrderWithSoloRideInDatabase()
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
                    Status = RideStatus.WaitingForAccept,
                    EndDestination = new Address("City", 8200, "Street", 21),
                    StartDestination = new Address("City", 8200, "Street", 21)
                };
                context.SoloRides.Add(soloRide);
                Order order = new Order()
                {
                    Status = OrderStatus.WaitingForAccept,
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
    */
    }
}
