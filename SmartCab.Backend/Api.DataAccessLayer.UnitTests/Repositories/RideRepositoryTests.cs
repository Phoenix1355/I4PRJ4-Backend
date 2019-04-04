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
            SoloRide ride = CreateSoloRide(customer.Id);

            ride = await _uut.AddSoloRideAsync(ride);
            
            using (var context = _factory.CreateContext())
            {
                Assert.That(context.Orders.Where(o=>o.Rides.Contains(ride)).Count, Is.EqualTo(1));
            }
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
