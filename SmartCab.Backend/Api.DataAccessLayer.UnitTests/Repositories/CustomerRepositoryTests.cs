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
        public async Task TearDown()
        {
            _factory.Dispose();
        }

        #endregion

        #region Helper functions

        

        
        private Customer addCustomerToTestDatabase(int balance = 0, int reservedAmount = 0)
        {
            Customer customer = new Customer
            {
                Email = "valid@email.com",
                Name = "Name",
                PhoneNumber = "12345678",
                Balance = balance,
                ReservedAmount = reservedAmount
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
            _uut.SaveChangesAsync();

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
            _uut.SaveChangesAsync();

            using (var context = _factory.CreateContext())
            {
                Assert.That(context.Customers.Find(customer.Id).Balance, Is.EqualTo(300));
            }
        }

        #endregion

        #region DebitAsync
        [Test]
        public async Task DebitAsync_NoCustomer_ThrowsContainsMessage()
        {
            try
            {
                await _uut.CustomerRepository.DebitAsync("Not valid Id", 1);
            }
            catch (UserIdInvalidException e)
            {
                Assert.That(e.Message, Is.EqualTo("No entity with given id"));
            }
        }



        [TestCase(1)]
        [TestCase(100)]
        [TestCase(1000)]
        public async Task DebitAsync_DebitsAmounts_CustomerAccountHasExpectedBalance(decimal debit)
        {
            var customer = addCustomerToTestDatabase(1000,1000);

            await _uut.CustomerRepository.DebitAsync(customer.Id, debit);
            _uut.SaveChangesAsync();

            using (var context = _factory.CreateContext())
            {
                Assert.That(context.Customers.FirstOrDefault().Balance, Is.EqualTo(1000-debit));
            }
        }

        [TestCase(100,1500)]
        [TestCase(1500, 100)]
        [TestCase(999, 100)]
        [TestCase(100, 999)]
        [TestCase(999, 999)]
        public async Task DebitAsync_DebitsAmounts_CustomerAccountHasInsufficientFundsThrowsException(int balance, int reserved)
        {
            var customer = addCustomerToTestDatabase(balance, reserved);
            
            Assert.ThrowsAsync<InsufficientFundsException>(async ()=>await _uut.CustomerRepository.DebitAsync(customer.Id, 1000));
        }

        [TestCase(0)]
        [TestCase(-100)]
        [TestCase(-100000)]
        public async Task DebitAsync_DepositAmountsNegativeAmount_ThrowsNegativeDepositException(decimal debit)
        {
            Assert.ThrowsAsync<NegativeDepositException>(async () => await _uut.CustomerRepository.DebitAsync("No Id as validation occurs first", debit));
        }

        [Test]
        public async Task DebitAsyncc_DepositAmountTwice_CustomerAccountHasReceivedExpectedAmount()
        {
            var customer = addCustomerToTestDatabase(1000,1000);

            await _uut.CustomerRepository.DebitAsync(customer.Id, 100);
            await _uut.CustomerRepository.DebitAsync(customer.Id, 200);
            _uut.SaveChangesAsync();

            using (var context = _factory.CreateContext())
            {
                Assert.That(context.Customers.Find(customer.Id).Balance, Is.EqualTo(700));
            }
        }

        [Test]
        public async Task DebitAsyncc_DepositAmountTwice_CustomerAccountHasReservedExpectedAmount()
        {
            var customer = addCustomerToTestDatabase(400,400);

            await _uut.CustomerRepository.DebitAsync(customer.Id, 100);
            await _uut.CustomerRepository.DebitAsync(customer.Id, 200);
            _uut.SaveChangesAsync();

            using (var context = _factory.CreateContext())
            {
                Assert.That(context.Customers.Find(customer.Id).ReservedAmount, Is.EqualTo(100));
            }
        }

        #endregion


        #region ReservePriceFromCustomerAsync

        [Test]
        public async Task ReservePriceFromCustomer_ReservesAmountAfterSaveChanges_AmountExpectedIsReserved()
        {
            var customer = addCustomerToTestDatabase(1000);

            var amountToReserve = 100;

            await _uut.CustomerRepository.ReservePriceFromCustomerAsync(customer.Id, amountToReserve);
            _uut.SaveChangesAsync();

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

            await _uut.CustomerRepository.ReservePriceFromCustomerAsync(customer.Id, amountToReserve);
            

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

            Assert.ThrowsAsync<InsufficientFundsException>(async()=> await _uut.CustomerRepository.ReservePriceFromCustomerAsync(customer.Id, amountToReserve));
        }

        [Test]
        public async Task ReservePriceFromCustomer_CustomerDoesNotExist_ThrowsException()
        {
            var amountToReserve = 100;

            Assert.ThrowsAsync<UserIdInvalidException>(async() => await _uut.CustomerRepository.ReservePriceFromCustomerAsync("Invalid Id", amountToReserve));
        }


        #endregion

        #region FindByEmailAsync

        [Test]
        public async Task FindByEmail_CustomerExist_ReturnsCustomer()
        {
            var customer = addCustomerToTestDatabase();

            var customerDB = await _uut.CustomerRepository.FindByEmailAsync(customer.Email);

            Assert.That(customer.Name,Is.EqualTo(customerDB.Name));
        }

        [Test]
        public async Task FindByEmail_CustomerDoesNotExist_ThrowsException()
        {
            Assert.ThrowsAsync<UserIdInvalidException>(()=>_uut.CustomerRepository.FindByEmailAsync("ValidEmail@ButNoCustomerInDatabase.com"));
        }

        #endregion

        #region FindCustomerRides

        [Test]
        public async Task FindCustomerRides_CustomerExistWithNoRides_ReturnsEmptyListOfRides()
        {
            var customer = addCustomerToTestDatabase();

            var rides = await _uut.CustomerRepository.FindCustomerRidesAsync(customer.Id);

            Assert.IsEmpty(rides);
        }

        [Test]
        public async Task FindCustomerRides_CustomerExistWith1Ride_ReturnsListOfRidesContaining1()
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

            var rides = await _uut.CustomerRepository.FindCustomerRidesAsync(customer.Id);

            Assert.That(rides.Count,Is.EqualTo(1));
        }

        [Test]
        public async Task FindCustomerRides_TwoCustomersExist_ReturnsOnlyRidesOfExpectedCustomer()
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

            var rides = await _uut.CustomerRepository.FindCustomerRidesAsync(customer2.Id);

            Assert.That(rides.Count, Is.EqualTo(0));
        }

        #region Dispose
        [Test]
        public async Task FindCustomerRides_CustomerDoesNotExist_ThrowsException()
        {
            Assert.ThrowsAsync<UserIdInvalidException>(async() => await _uut.CustomerRepository.FindCustomerRidesAsync("Invalid ID"));
        }

        #endregion
        #endregion

    }
}
