using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Api.DataAccessLayer.Models;
using Api.DataAccessLayer.Repositories;
using Api.DataAccessLayer.Statuses;
using Api.DataAccessLayer.UnitTests.Factories;
using Api.DataAccessLayer.UnitTests.Fakes;
using CustomExceptions;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Data.Sqlite;
using NUnit.Framework;
using Microsoft.EntityFrameworkCore;

namespace Api.DataAccessLayer.UnitTests.Repositories
{
    [TestFixture]
    public class CustomerRepositoryTests
    {
        #region Setup
        private CustomerRepository _uut;
        private InMemorySqlLiteContextFactory _factory;
        private FakeSignInManager _mockSignManager;
        private FakeUserManager _mockUserManager;

        [SetUp]
        public void SetUp()
        {
            _factory = new InMemorySqlLiteContextFactory();
            _mockSignManager = new FakeSignInManager();
            _mockUserManager = new FakeUserManager();
            IdentityUserRepository identityUserRepository = new IdentityUserRepository(_mockUserManager,_mockSignManager);
            _uut = new CustomerRepository(_factory.CreateContext(), identityUserRepository); 
        }

        [TearDown]
        public void TearDown()
        {
            _factory.Dispose();
        }

        #endregion

        #region  AddCustomerAsync


        [Test]
        public async Task AddCustomerAsync_CustomerValid_CustomerExistsInDatabase()
        {
            Customer customer = new Customer
            {
                Name = "Name",
                PhoneNumber = "12345678",
            };
            //As function now relies on Identity framework, insert it manually. 
            using (var context = _factory.CreateContext())
            {
                context.Customers.Add(customer);
                context.SaveChanges();
            }


            await _uut.AddCustomerAsync(customer, "Qwer111!");



            using (var context = _factory.CreateContext())
            {
                var customerFromDatabase = context.Customers.FirstOrDefault(customerFromDB => customer.Id.Equals(customerFromDB.Id));

                Assert.That(customerFromDatabase.Name, Is.EqualTo("Name"));
            }
            
        }

        [Test]
        public void AddCustomerAsync_CustomerInvalid_CustomerAlreadyExistsInDatabase()
        {
            Customer customerToAddToDatabase = new Customer
            {
                Name = "Name",
                PhoneNumber = "12345678",
            };

            _mockUserManager.AddToRoleAsyncReturn = IdentityResult.Failed();

            using (var context = _factory.CreateContext())
            {
                context.Customers.Add(customerToAddToDatabase);
                context.SaveChanges();
            }


            Assert.ThrowsAsync<IdentityException>(async ()=>await _uut.AddCustomerAsync(customerToAddToDatabase, "Qwer111!"));
        }

        #endregion

        #region GetCustomerAsync

        [Test]
        public async Task GetCustomerAsync_CustomerInDatabase_ReturnsCustomer()
        {
            Customer customerAddedToDatabase = new Customer
            {
                Email = "valid@email.com",
                Name = "Name",
                PhoneNumber = "12345678",
            };

            using (var context = _factory.CreateContext())
            {
                context.Customers.Add(customerAddedToDatabase);
                context.SaveChanges();
            }


            var customerReturned = await _uut.GetCustomerAsync(customerAddedToDatabase.Email);
            Assert.That(customerReturned.Name, Is.EqualTo("Name"));
        }

        [Test]
        public void GetCustomerAsync_NoCustomer_ThrowsNotFound()
        {
            Assert.ThrowsAsync<UserIdInvalidException>( async () => await _uut.GetCustomerAsync("Not valid Id"));
        }

        [Test]
        public async Task GetCustomerAsync_NoCustomer_ThrowsContainsMessage()
        {
            try
            {
                await _uut.GetCustomerAsync("Not valid Id");
            }
            catch (UserIdInvalidException e)
            {
                Assert.That(e.Message, Is.EqualTo("Customer does not exist."));
            }
        }
        #endregion

        #region DepositAsync
        [Test]
        public async Task DepositAsync_NoCustomer_ThrowsContainsMessage()
        {
            try
            {
                await _uut.DepositAsync("Not valid Id", 1);

            }
            catch (UserIdInvalidException e)
            {
                Assert.That(e.Message, Is.EqualTo("Customer does not exist."));
            }

        }

        [Test]
        public void DepositAsync_NoCustomer_ThrowsUserIdInvalidException()
        {
            Assert.ThrowsAsync<UserIdInvalidException>(async () => await _uut.GetCustomerAsync("Not valid Id"));
        }

        [TestCase(1)]
        [TestCase(100)]
        [TestCase(100000)]
        public async Task DepositAsync_DepositAmounts_CustomerAccountHasReceivedExpectedBalanace(decimal deposit)
        {
            Customer customerAddedToDatabase = new Customer
            {
                Email = "valid@email.com",
                Name = "Name",
                PhoneNumber = "12345678",
            };

            using (var context = _factory.CreateContext())
            {
                context.Customers.Add(customerAddedToDatabase);
                context.SaveChanges();
            }

            await _uut.DepositAsync(customerAddedToDatabase.Id, deposit);

            using (var context = _factory.CreateContext())
            {
                Assert.That(context.Customers.FirstOrDefault().Balance,Is.EqualTo(deposit));
            }
        }

        [TestCase(0)]
        [TestCase(-100)]
        [TestCase(-100000)]
        public async Task DepositAsync_DepositAmountsNegativeAmount_ThrowsNegativeDepositException(decimal deposit)
        {
            Assert.ThrowsAsync<NegativeDepositException>(async () => await _uut.DepositAsync("No Id as validation occurs first", deposit));
        }

        [Test]
        public async Task DepositAsync_DepositAmountTwice_CustomerAccountHasReceivedExpectedAmount()
        {
            Customer customerAddedToDatabase = new Customer
            {
                Email = "valid@email.com",
                Name = "Name",
                PhoneNumber = "12345678",
            };

            using (var context = _factory.CreateContext())
            {
                context.Customers.Add(customerAddedToDatabase);
                context.SaveChanges();
            }

            await _uut.DepositAsync(customerAddedToDatabase.Id, 100);
            await _uut.DepositAsync(customerAddedToDatabase.Id, 200);

            using (var context = _factory.CreateContext())
            {
                Assert.That(context.Customers.Find(customerAddedToDatabase.Id).Balance, Is.EqualTo(300));
            }
        }

        #endregion

        #region GetRidesAsync

        [Test]
        public async Task GetRidesAsync_ParamterNull_ThrowsException()
        {
            Assert.ThrowsAsync<UserIdInvalidException>(async ()=> await _uut.GetRidesAsync(null));
        }

        [Test]
        public async Task GetRidesAsync_ParameterEmpty_ThrowsException()
        {
            Assert.ThrowsAsync<UserIdInvalidException>(async () => await _uut.GetRidesAsync(""));
        }

        [Test]
        public async Task GetRidesAsync_CustomerExistButNoRides_ReturnsEmptyList()
        {
            var customer = new Customer
            {
                Email = "valid@email.com",
                Name = "Name",
                PhoneNumber = "12345678",
            };
            using (var context = _factory.CreateContext())
            {

                context.Customers.Add(customer);
                context.SaveChanges();
            }

            var response = await _uut.GetRidesAsync(customer.Id);
            Assert.That(response,Is.Empty);
        }

        [Test]
        public async Task GetRidesAsync_CustomerExistButNoRides_ReturnsListWithOne()
        {
            var customer = new Customer
            {
                Email = "valid@email.com",
                Name = "Name",
                PhoneNumber = "12345678",
            };

            customer.Rides = new List<Ride>();
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
                context.Customers.Add(customer);
                context.SaveChanges();
            }

            var response = await _uut.GetRidesAsync(customer.Id);
            Assert.That(response.Count, Is.EqualTo(1));
        }

        [Test]
        public async Task GetRidesAsync_TwoCustomerExistWithRides_ReturnExpectedCustomer()
        {
            var customer = new Customer
            {
                Email = "valid@email.com",
                Name = "ExpectedCustomer",
                PhoneNumber = "12345678",
            };

            customer.Rides = new List<Ride>();
            var soloRide = new SoloRide()
            {
                CustomerId = customer.Id,
                DepartureTime = DateTime.Now,
                ConfirmationDeadline = DateTime.Now,
                PassengerCount = 0,
                CreatedOn = DateTime.Now,
                Price = 200,
                Status = RideStatus.WaitingForAccept,
                EndDestination = new Address("City", 8200, "Street", 21),
                StartDestination = new Address("City", 8200, "Street", 21)
            };

            var customer2 = new Customer
            {
                Email = "valid2@email.com",
                Name = "Name",
                PhoneNumber = "12345678",
            };
            customer2.Rides = new List<Ride>();
            customer.Rides = new List<Ride>();
            var soloRide2 = new SoloRide()
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
            customer2.Rides.Add(soloRide2);

            customer.Rides.Add(soloRide);
            using (var context = _factory.CreateContext())
            {
                context.Customers.Add(customer);
                context.Customers.Add(customer2);
                context.SaveChanges();
            }

            var response = await _uut.GetRidesAsync(customer.Id);
            Assert.That(response.First().Customer.Name, Is.EqualTo("ExpectedCustomer"));
        }

        [Test]
        public async Task GetRidesAsync_TwoCustomerExistWithRides_ReturnsListWithOneForRightCustomer()
        {
            var customer = new Customer
            {
                Email = "valid@email.com",
                Name = "ExpectedCustomer",
                PhoneNumber = "12345678",
            };

            customer.Rides = new List<Ride>();
            var soloRide = new SoloRide()
            {
                CustomerId = customer.Id,
                DepartureTime = DateTime.Now,
                ConfirmationDeadline = DateTime.Now,
                PassengerCount = 0,
                CreatedOn = DateTime.Now,
                Price = 200,
                Status = RideStatus.WaitingForAccept,
                EndDestination = new Address("City", 8200, "Street", 21),
                StartDestination = new Address("City", 8200, "Street", 21)
            };

            var customer2 = new Customer
            {
                Email = "valid2@email.com",
                Name = "Name",
                PhoneNumber = "12345678",
            };
            customer2.Rides = new List<Ride>();
            customer.Rides = new List<Ride>();
            var soloRide2 = new SoloRide()
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
            customer2.Rides.Add(soloRide2);

            customer.Rides.Add(soloRide);
            using (var context = _factory.CreateContext())
            {
                context.Customers.Add(customer);
                context.Customers.Add(customer2);
                context.SaveChanges();
            }

            var response = await _uut.GetRidesAsync(customer.Id);
            Assert.That(response.Count, Is.EqualTo(1));
        }
        #endregion



        [Test]
        public void Dispose_DisposeOfObject_Disposes()
        {
            _uut.Dispose();
        }
    }
}
