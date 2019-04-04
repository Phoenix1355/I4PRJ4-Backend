using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Api.DataAccessLayer.Models;
using Api.DataAccessLayer.Repositories;
using Api.DataAccessLayer.Statuses;
using Api.DataAccessLayer.UnitTests.Factories;
using NUnit.Framework;

namespace Api.DataAccessLayer.UnitTests.Repositories
{
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

        #endregion

        #region AddSoloRideAsync

        [Test]
        public async Task CreateSoloRideAsync_ValidRideAndCustomerWithFunds_RideIsCreated()
        {
            var customer = SeedDatabaseWithCustomer();
            SoloRide ride = CreateSoloRide(customer.Id);

            ride = await _uut.AddSoloRideAsync(ride);

            using (var context = _factory.CreateContext())
            {
                Assert.That(context.SoloRides.Find(ride.Id).Id,Is.EqualTo(ride.Id));
            }
        }

        [Test]
        public async Task CreateSoloRideAsync_ValidRideAndCustomerWithFunds_OrderIsCreated()
        {
            var customer = SeedDatabaseWithCustomer();
            Ride ride = CreateSoloRide(customer.Id);

            ride = await _uut.AddSoloRideAsync((SoloRide)ride);
            
            using (var context = _factory.CreateContext())
            {
                Assert.That(context.Orders.Count(o=>o.Rides.Contains(ride)), Is.EqualTo(1));
                
            }
        }

        [Test]
        public async Task CreateSoloRideAsync_ValidRideAndCustomerWithFunds_OrderHasRightPrice()
        {
            var customer = SeedDatabaseWithCustomer();
            Ride ride = CreateSoloRide(customer.Id);

            ride = await _uut.AddSoloRideAsync((SoloRide)ride);

            using (var context = _factory.CreateContext())
            {
                
                Assert.That(context.Orders.Where(o => o.Rides.Contains(ride)).FirstOrDefault().Price, Is.EqualTo(ride.Price));
            }
        }

        [Test]
        public async Task CreateSoloRideAsync_ValidRideAndCustomerWithFunds_CustomerReservedRightAmount()
        {
            var customer = SeedDatabaseWithCustomer();
            Ride ride = CreateSoloRide(customer.Id);

            ride = await _uut.AddSoloRideAsync((SoloRide)ride);

            using (var context = _factory.CreateContext())
            {
                Assert.That(context.Customers.Find(customer.Id).ReservedAmount, Is.EqualTo(100));
            }
        }

        [Test]
        public async Task CreateSoloRideAsync_ValidRideAndCustomerWithFunds_ThrowsException()
        {
            var customer = SeedDatabaseWithCustomer();
            Ride ride = CreateSoloRide(customer.Id);

            ride = await _uut.AddSoloRideAsync((SoloRide)ride);
            ;
            Assert.ThrowsAsync<ArgumentException>(async ()=>await _uut.AddSoloRideAsync((SoloRide)ride));
        }

        [Test]
        public async Task CreateSoloRideAsync_ValidRideAndCustomerWithOutFunds_ThrowsException()
        {
            var customer = SeedDatabaseWithCustomer(0,0);
            Ride ride = CreateSoloRide(customer.Id);;
            Assert.ThrowsAsync<ArgumentOutOfRangeException>(async () => await _uut.AddSoloRideAsync((SoloRide)ride));
        }

        [Test]
        public async Task CreateSoloRideAsync_ValidRideAndCustomerWithJustEnoughFunds_DoesNotThrow()
        {
            var customer = SeedDatabaseWithCustomer(100, 0);
            Ride ride = CreateSoloRide(customer.Id); ;
            Assert.DoesNotThrowAsync(async () => await _uut.AddSoloRideAsync((SoloRide)ride));
        }

        [Test]
        public async Task CreateSoloRideAsync_ValidRideAndCustomerWithJustNotEnoughFunds_DoesThrow()
        {
            var customer = SeedDatabaseWithCustomer(99, 0);
            Ride ride = CreateSoloRide(customer.Id); ;
            Assert.ThrowsAsync<ArgumentOutOfRangeException>(async () => await _uut.AddSoloRideAsync((SoloRide)ride));
        }
        #endregion


        #region Helper Methods
        private Customer SeedDatabaseWithCustomer(decimal balance=1000, decimal reserved = 0)
        {
            var customer = new Customer()
            {
                Name = "Name",
                Balance = balance,
                ReservedAmount = reserved
            };
            using (var context = _factory.CreateContext())
            {
                context.Customers.Add(customer);
                context.SaveChanges();
            }

            return customer;
        }

        private SoloRide CreateSoloRide(string customerId)
        {
            return new SoloRide()
            {
                CustomerId = customerId,
                DepartureTime = DateTime.Now,
                ConfirmationDeadline = DateTime.Now,
                PassengerCount = 0,
                CreatedOn = DateTime.Now,
                Price = 100,
                Status = RideStatus.WaitingForAccept,
                EndDestination = new Address("City", 8200, "Street", 21),
                StartDestination = new Address("City", 8200, "Street", 21)
            };
        }
        #endregion
       
    }
}
