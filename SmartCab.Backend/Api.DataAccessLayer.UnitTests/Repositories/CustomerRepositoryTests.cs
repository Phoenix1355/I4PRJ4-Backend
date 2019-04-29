using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Net.Http;
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
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Data.Sqlite;
using NUnit.Framework;
using Microsoft.EntityFrameworkCore;
using NSubstitute;

namespace Api.DataAccessLayer.UnitTests.Repositories
{
    [TestFixture]
    public class CustomerRepositoryTests
    {
        #region Setup

        private IUoW _uut;
        private InMemorySqlLiteContextFactory _factory;

        [SetUp]
        public void SetUp()
        {
            _factory = new InMemorySqlLiteContextFactory();
            var identityRepository = Substitute.For<IIdentityUserRepository>();
            _uut = new UoW(_factory.CreateContext(), identityRepository);
        }

        [TearDown]
        public void TearDown()
        {
            _factory.Dispose();
        }

        #endregion

        #region Helper functions

        

        
        private Customer addCustomerToTestDatabase(int balance = 0)
        {
            Customer customer = new Customer
            {
                Email = "valid@email.com",
                Name = "Name",
                PhoneNumber = "12345678",
                Balance = balance
            };

            using (var context = _factory.CreateContext())
            {
                context.Customers.Add(customer);
                context.SaveChanges();
            }

            return customer;
        }
        #endregion

        #region DepositAsync
        [Test]
        public async Task DepositAsync_NoCustomer_ThrowsContainsMessage()
        {
            try
            {
                await _uut.CustomerRepository.DepositAsync("Not valid Id", 1);
            }
            catch (UserIdInvalidException e)
            {
                Assert.That(e.Message, Is.EqualTo("No entity with given id"));
            }
        }

       
        
        [TestCase(1)]
        [TestCase(100)]
        [TestCase(100000)]
        public async Task DepositAsync_DepositAmounts_CustomerAccountHasReceivedExpectedBalanace(decimal deposit)
        {
            var customer = addCustomerToTestDatabase();

            await _uut.CustomerRepository.DepositAsync(customer.Id, deposit);
            _uut.SaveChanges();

            using (var context = _factory.CreateContext())
            {
                Assert.That(context.Customers.FirstOrDefault().Balance, Is.EqualTo(deposit));
            }
        }

        [TestCase(0)]
        [TestCase(-100)]
        [TestCase(-100000)]
        public async Task DepositAsync_DepositAmountsNegativeAmount_ThrowsNegativeDepositException(decimal deposit)
        {
            Assert.ThrowsAsync<NegativeDepositException>(async () => await _uut.CustomerRepository.DepositAsync("No Id as validation occurs first", deposit));
        }

        [Test]
        public async Task DepositAsync_DepositAmountTwice_CustomerAccountHasReceivedExpectedAmount()
        {
            var customer = addCustomerToTestDatabase();

            await _uut.CustomerRepository.DepositAsync(customer.Id, 100);
            await _uut.CustomerRepository.DepositAsync(customer.Id, 200);
            _uut.SaveChanges();

            using (var context = _factory.CreateContext())
            {
                Assert.That(context.Customers.Find(customer.Id).Balance, Is.EqualTo(300));
            }
        }

        #endregion

        #region ReservePriceFromCustomer

        [Test]
        public async Task ReservePriceFromCustomer_ReservesAmountAfterSaveChanges_AmountExpectedIsReserved()
        {
            var customer = addCustomerToTestDatabase(1000);

            var amountToReserve = 100;

            _uut.CustomerRepository.ReservePriceFromCustomer(customer.Id, amountToReserve);
            _uut.SaveChanges();

            using (var context = _factory.CreateContext())
            {
                Assert.That(context.Customers.Find(customer.Id).ReservedAmount,Is.EqualTo(amountToReserve));
            }
        }

        [Test]
        public async Task ReservePriceFromCustomer_DoesNotReservesAmountWithoutSaveChanges_AmountExpectedIsNotReserved()
        {
            var customer = addCustomerToTestDatabase(1000);

            var amountToReserve = 100;

            _uut.CustomerRepository.ReservePriceFromCustomer(customer.Id, amountToReserve);
            

            using (var context = _factory.CreateContext())
            {
                Assert.That(context.Customers.Find(customer.Id).ReservedAmount, Is.EqualTo(0));
            }
        }

        [Test]
        public async Task ReservePriceFromCustomer_NotEnoughFundsToReserve_ThrowsException()
        {
            var customer = addCustomerToTestDatabase(0);

            var amountToReserve = 100;

            Assert.Throws<InsufficientFundsException>(()=> _uut.CustomerRepository.ReservePriceFromCustomer(customer.Id, amountToReserve));
        }

        [Test]
        public async Task ReservePriceFromCustomer_CustomerDoesNotExist_ThrowsException()
        {
            var amountToReserve = 100;

            Assert.Throws<UserIdInvalidException>(() => _uut.CustomerRepository.ReservePriceFromCustomer("Invalid Id", amountToReserve));
        }


        #endregion

        #region FindByEmail

        [Test]
        public void FindByEmail_CustomerExist_ReturnsCustomer()
        {
            var customer = addCustomerToTestDatabase();

            Assert.That(customer.Name,Is.EqualTo(_uut.CustomerRepository.FindByEmail(customer.Email).Name));
        }

        [Test]
        public void FindByEmail_CustomerDoesNotExist_ThrowsException()
        {
            Assert.Throws<UserIdInvalidException>(()=>_uut.CustomerRepository.FindByEmail("ValidEmail@ButNoCustomerInDatabase.com"));
        }

        #endregion

        #region FindCustomerRides

        [Test]
        public void FindCustomerRides_CustomerExistWithNoRides_ReturnsEmptyListOfRides()
        {
            var customer = addCustomerToTestDatabase();

            var rides = _uut.CustomerRepository.FindCustomerRides(customer.Id);

            Assert.IsEmpty(rides);
        }

        [Test]
        public void FindCustomerRides_CustomerExistWith1Ride_ReturnsListOfRidesContaining1()
        {
            var customer = addCustomerToTestDatabase();

            var soloRide = new SoloRide()
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

            customer.Rides.Add(soloRide);
            using (var context = _factory.CreateContext())
            {
                context.Customers.Update(customer);
                context.SaveChanges();
            }

            var rides = _uut.CustomerRepository.FindCustomerRides(customer.Id);

            Assert.That(rides.Count,Is.EqualTo(1));
        }

        [Test]
        public void FindCustomerRides_TwoCustomersExist_ReturnsOnlyRidesOfExpectedCustomer()
        {
            var customer = addCustomerToTestDatabase();
            var customer2 = addCustomerToTestDatabase();

            var soloRide = new SoloRide()
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

            customer.Rides.Add(soloRide);
            using (var context = _factory.CreateContext())
            {
                context.Customers.Update(customer);
                context.SaveChanges();
            }

            var rides = _uut.CustomerRepository.FindCustomerRides(customer2.Id);

            Assert.That(rides.Count, Is.EqualTo(0));
        }

        [Test]
        public void FindCustomerRides_CustomerDoesNotExist_ThrowsException()
        {
            Assert.Throws<UserIdInvalidException>(() => _uut.CustomerRepository.FindCustomerRides("Invalid ID"));
        }

        #endregion
    }
}
