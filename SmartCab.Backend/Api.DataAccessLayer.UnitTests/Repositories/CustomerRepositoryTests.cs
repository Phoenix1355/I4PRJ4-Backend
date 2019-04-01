using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Net.Http;
using System.Text;
using Api.DataAccessLayer.Models;
using Api.DataAccessLayer.Repositories;
using Api.DataAccessLayer.UnitTests.Factories;
using Api.DataAccessLayer.UnitTests.Fakes;
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
        public void AddCustomerAsync_CustomerValid_CustomerExistsInDatabase()
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


            _uut.AddCustomerAsync(customer, "Qwer111!").Wait();



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


            Assert.ThrowsAsync<ArgumentException>(()=>_uut.AddCustomerAsync(customerToAddToDatabase, "Qwer111!"));
        }

        [Test]
        public void GetCustomerAsync_CustomerInDatabase_ReturnsCustomer()
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


            var customerReturned = _uut.GetCustomerAsync(customerAddedToDatabase.Email).Result;
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
