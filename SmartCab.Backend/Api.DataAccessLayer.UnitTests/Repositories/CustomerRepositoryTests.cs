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
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Data.Sqlite;
using NSubstitute;
using NUnit.Framework;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace Api.DataAccessLayer.UnitTests.Repositories
{
    public class FakeSignInManager : SignInManager<ApplicationUser>
    {
        public FakeSignInManager()
            : base(Substitute.For<FakeUserManager>(),
                 Substitute.For<IHttpContextAccessor>(),
                 Substitute.For<IUserClaimsPrincipalFactory<ApplicationUser>>(),
                 Substitute.For<IOptions<IdentityOptions>>(),
                 Substitute.For<ILogger<SignInManager<ApplicationUser>>>(),
                 Substitute.For<IAuthenticationSchemeProvider>())
        { }
    }



    public class FakeUserManager : UserManager<ApplicationUser>
    {
        public FakeUserManager()
            : base( Substitute.For<IUserStore<ApplicationUser>>(),
                 Substitute.For<IOptions<IdentityOptions>>(),
                 Substitute.For<IPasswordHasher<ApplicationUser>>(),
                new IUserValidator<ApplicationUser>[0],
                new IPasswordValidator<ApplicationUser>[0],
                 Substitute.For<ILookupNormalizer>(),
                 Substitute.For<IdentityErrorDescriber>(),
                 Substitute.For<IServiceProvider>(),
                 Substitute.For<ILogger<UserManager<ApplicationUser>>>())
        { }

        public override Task<IdentityResult> CreateAsync(ApplicationUser user, string password)
        {
            return Task.FromResult(IdentityResult.Success);
        }

        public override Task<IdentityResult> AddToRoleAsync(ApplicationUser user, string role)
        {
            return Task.FromResult(IdentityResult.Success);
        }

    }

    [TestFixture]
    public class CustomerRepositoryTests
    {
        private CustomerRepository _uut;
        private ApplicationContextFactory _factory;

        [SetUp]
        public void SetUp()
        {
            _factory = new ApplicationContextFactory();
            var mockSignManager = new FakeSignInManager();
            var mockUserManager = new FakeUserManager();
            //mockUserManager.AddToRoleAsync(null, null).ReturnsForAnyArgs<IdentityResult>(IdentityResult.Success);

            ApplicationUserRepository applicationUserRepository = new ApplicationUserRepository(mockUserManager,mockSignManager);
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
