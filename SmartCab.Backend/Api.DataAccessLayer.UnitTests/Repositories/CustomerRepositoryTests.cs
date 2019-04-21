using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Api.DataAccessLayer.Models;
using Api.DataAccessLayer.Repositories;
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
            IdentityUserRepository identityUserRepository = new IdentityUserRepository(_mockUserManager, _mockSignManager);
            _uut = new CustomerRepository(_factory.CreateContext(), identityUserRepository);
        }

        [TearDown]
        public void TearDown()
        {
            _factory.Dispose();
        }

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


            Assert.ThrowsAsync<IdentityException>(async () => await _uut.AddCustomerAsync(customerToAddToDatabase, "Qwer111!"));
        }

        [Test]
        public async Task GetCustomerAsync_CustomerInDatabase_ReturnsCustomerWithName()
        {
            Customer customerAddedToDatabase = new Customer
            {
                Email = "valid@email.com",
                Name = "Hans Petersen",
                PhoneNumber = "12345678",
            };

            using (var context = _factory.CreateContext())
            {
                context.Customers.Add(customerAddedToDatabase);
                context.SaveChanges();
            }


            var customerReturned = await _uut.GetCustomerAsync(customerAddedToDatabase.Email);
            Assert.That(customerReturned.Name, Is.EqualTo("Hans Petersen"));
        }

        [Test]
        public async Task GetCustomerAsync_CustomerInDatabase_ReturnsCustomerWithEmail()
        {
            Customer customerAddedToDatabase = new Customer
            {
                Email = "HansAU@email.com",
                Name = "Hans",
                PhoneNumber = "12345678",
            };

            using (var context = _factory.CreateContext())
            {
                context.Customers.Add(customerAddedToDatabase);
                context.SaveChanges();
            }


            var customerReturned = await _uut.GetCustomerAsync(customerAddedToDatabase.Email);
            Assert.That(customerReturned.Email, Is.EqualTo("HansAU@email.com"));
        }

        [Test]
        public async Task GetCustomerAsync_CustomerInDatabase_ReturnsCustomerWithPhoneNumber()
        {
            Customer customerAddedToDatabase = new Customer
            {
                Email = "HansAU@email.com",
                Name = "Hans",
                PhoneNumber = "66665555",
            };

            using (var context = _factory.CreateContext())
            {
                context.Customers.Add(customerAddedToDatabase);
                context.SaveChanges();
            }


            var customerReturned = await _uut.GetCustomerAsync(customerAddedToDatabase.Email);
            Assert.That(customerReturned.PhoneNumber, Is.EqualTo("66665555"));
        }

        [Test]
        public void GetCustomerAsync_NoCustomer_ThrowsNotFound()
        {
            Assert.ThrowsAsync<UserIdInvalidException>(async () => await _uut.GetCustomerAsync("Not valid Id"));
        }

        [Test]
        public async Task GetCustomerAsync_NoCustomer_ThrowsContainMessage()
        {
            try
            {
                await _uut.GetCustomerAsync("Not valid Id");
            }
            catch (UserIdInvalidException m)
            {
                Assert.That(m.Message, Is.EqualTo("Customer does not exist."));
            }
        }


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
                Assert.That(context.Customers.FirstOrDefault().Balance, Is.EqualTo(deposit));
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

        [Test]
        public void Dispose_DisposeOfObject_Disposes()
        {
            _uut.Dispose();
        }
    }
}
