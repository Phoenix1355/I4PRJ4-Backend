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
            IdentityUserRepository identityUserRepository = new IdentityUserRepository(_mockUserManager,_mockSignManager);
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

            using (var content = _factory.CreateContext())
            {
                content.Customers.Add(customerToAddToDatabase);
                content.SaveChanges();
            }


            Assert.ThrowsAsync<IdentityException>(async ()=>await _uut.AddCustomerAsync(customerToAddToDatabase, "Qwer111!"));
        }

        [Test]
        public async Task GetCustomerAsync_CustomerInDatabase_ReturnsCustomer()
        {
            Customer customerAddedToDatabase = new Customer
            {
                Email = "valid@email.com",
                Name = "Name",
                PhoneNumber = "12345678",
            };

            using (var content = _factory.CreateContext())
            {
                content.Customers.Add(customerAddedToDatabase);
                content.SaveChanges();
            }


            var customerReturned = await _uut.GetCustomerAsync(customerAddedToDatabase.Email);
            Assert.That(customerReturned.Name, Is.EqualTo("Name"));
        }

        [Test]
        public void GetCustomerAsyncc_NoCustomer_ThrowsNotFound()
        {
            Assert.ThrowsAsync<UserIdInvalidException>( async () => await _uut.GetCustomerAsync("NoEmail@mail.com"));
        }

        [Test]
        public async Task GetCustomerAsyncc_NoCustomer_ThrowsContainsMessage()
        {
            try
            {
                await _uut.GetCustomerAsync("NoEmail@mail.com");

            }
            catch (UserIdInvalidException e)
            {
                Assert.That(e.Message, Is.EqualTo("Customer does not exist."));
            }
        }


        [Test]
        public async Task DepositAsync_NoCustomer_ThrowsContainsMessage()
        {
            try
            {
                await _uut.DepositAsync("NoEmail@mail.com", 0);

            }
            catch (UserIdInvalidException e)
            {
                Assert.That(e.Message, Is.EqualTo("Customer does not exist."));
            }

        }


        [Test]
        public void DepositAsync_NoCustomer_ThrowsUserIdInvalidException()
        {
            Assert.ThrowsAsync<UserIdInvalidException>(async () => await _uut.GetCustomerAsync("NoEmail@mail.com"));
        }

        [Test]
        public async Task DepositAsync_DepositAmounts_CustomerAccountHasReceivedExpectedBalanace(decimal deposit)
        {
            Customer customerAddedToDatabase = new Customer
            {
                Email = "valid@email.com",
                Name = "Name",
                PhoneNumber = "12345678",
            };

            using (var content = _factory.CreateContext())
            {
                content.Customers.Add(customerAddedToDatabase);
                content.SaveChanges();
            }

            await _uut.DepositAsync("NoEmail@mail.com", deposit);
        }

        [Test]
        public void Dispose_DisposeOfObject_Disposes()
        {
            _uut.Dispose();
        }
    }
}
