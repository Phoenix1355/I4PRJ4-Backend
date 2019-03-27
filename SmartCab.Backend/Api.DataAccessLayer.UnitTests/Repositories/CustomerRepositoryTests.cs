using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Net.Http;
using System.Text;
using Api.DataAccessLayer.Models;
using Api.DataAccessLayer.Repositories;
using Api.DataAccessLayer.UnitTests.Factories;
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
        private ApplicationContextFactory _factory;
        private FakeSignInManager _mockSignManager;
        private FakeUserManager _mockUserManager;

        [SetUp]
        public void SetUp()
        {
            _factory = new ApplicationContextFactory();
            _mockSignManager = new FakeSignInManager();
            _mockUserManager = new FakeUserManager();
            ApplicationUserRepository applicationUserRepository = new ApplicationUserRepository(_mockUserManager,_mockSignManager);
            _uut = new CustomerRepository(_factory.CreateContext(), applicationUserRepository); 
        }

        [TearDown]
        public void TearDown()
        {
            _factory.Dispose();
        }

        [Test]
        public void AddCustomerAsync_ApplicationUserValid_CustomerExistsInDatabase()
        {
            ApplicationUser user = new ApplicationUser();
            using (var content = _factory.CreateContext())
            {
                content.ApplicationUsers.Add(user);
                content.SaveChanges();
            }


            Customer customerToAddToDatabase = new Customer
            {
                ApplicationUserId = user.Id,
                Name = "Name",
                PhoneNumber = "12345678",
            };

            _uut.AddCustomerAsync(user, customerToAddToDatabase, "Qwer111!").Wait();
            using (var content = _factory.CreateContext())
            {
                var customerFromDatabase = content.Customers.FirstOrDefault(customer => customer.ApplicationUserId.Equals(user.Id));

                Assert.That(customerFromDatabase.Name, Is.EqualTo("Name"));
            }
            
        }

        [Test]
        public void AddCustomerAsync_ApplicationUserInvalid_CustomerAlreadyExistsInDatabase()
        {
            ApplicationUser user = new ApplicationUser();
            Customer customerToAddToDatabase = new Customer
            {
                ApplicationUserId = user.Id,
                Name = "Name",
                PhoneNumber = "12345678",
            };

            _mockUserManager.AddToRoleAsyncReturn = IdentityResult.Failed();

            using (var content = _factory.CreateContext())
            {
                content.ApplicationUsers.Add(user);
                content.Customers.Add(customerToAddToDatabase);
                content.SaveChanges();
            }


            Assert.ThrowsAsync<ArgumentException>(()=>_uut.AddCustomerAsync(user, customerToAddToDatabase, "Qwer111!"));
        }

        [Test]
        public void GetCustomerAsyncc_CustomerInDatabase_ReturnsCustomer()
        {
            ApplicationUser user = new ApplicationUser();
            user.Email = "valid@email.com";
            Customer customerAddedToDatabase = new Customer
            {
                ApplicationUserId = user.Id,
                Name = "Name",
                PhoneNumber = "12345678",
            };

            using (var content = _factory.CreateContext())
            {
                content.ApplicationUsers.Add(user);
                content.Customers.Add(customerAddedToDatabase);
                content.SaveChanges();
            }


            var customerReturned = _uut.GetCustomerAsync(user.Email).Result;
            Assert.That(customerReturned.Name, Is.EqualTo("Name"));

        }

        [Test]
        public void GetCustomerAsyncc_NoCustomer_ThrowsNotFound()
        {
            Assert.ThrowsAsync<ArgumentNullException>( () =>  _uut.GetCustomerAsync("NoEmail@mail.com"));
        }

        [Test]
        public void GetCustomerAsyncc_NoCustomer_ThrowsContainsMessage()
        {
            try
            {
                _uut.GetCustomerAsync("NoEmail@mail.com");

            }
            catch (ArgumentNullException e)
            {
                Assert.That(e.Message, Is.EqualTo("Customer does not exist."));
            }
        }

        [Test]
        public void Dispose_DisposeOfObject_Disposes()
        {
            _uut.Dispose();
        }
    }
}
