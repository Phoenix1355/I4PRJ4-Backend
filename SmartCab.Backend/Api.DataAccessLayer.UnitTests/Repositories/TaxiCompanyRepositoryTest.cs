using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Linq.Expressions;
using System.Net.Http;
using System.Text;
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
    public class TaxiCompanyRepositoryTests
    {
        private TaxiCompanyRepository _uut;
        private InMemorySqlLiteContextFactory _factory;

        // Creating fakes
        private FakeUserManager _mockUserManager;
        private FakeSignInManager _mockSignManager;

        [SetUp]
        public void SetUp()
        {
            _factory = new InMemorySqlLiteContextFactory();
            _mockSignManager = new FakeSignInManager();
            _mockUserManager = new FakeUserManager();
            IdentityUserRepository identityUserRepository = new IdentityUserRepository(_mockUserManager, _mockSignManager);
            _uut = new TaxiCompanyRepository(_factory.CreateContext(), identityUserRepository);
        }

        // Testing Taxi Company Name
        [Test]
        public void GetTaxiCompanyAsync_TaxiCompanyNameInDatabase_ReturnsTaxiCompanyName()
        {
            TaxiCompany taxicompanyAddedToDatabase = new TaxiCompany
            {
                Email = "valid@email.com",
                Name = "DanTaxi",
                PhoneNumber = "45612378",
            };

            using (var content = _factory.CreateContext())
            {
                content.TaxiCompanies.Add(taxicompanyAddedToDatabase);
                content.SaveChanges();
            }


            var taxicompanyReturned = _uut.GetTaxiCompanyAsync(taxicompanyAddedToDatabase.Email).Result;
            Assert.That(taxicompanyReturned.Name, Is.EqualTo("DanTaxi"));
        }

        // Testing Taxi Company Email
        [Test]
        public void GetTaxiCompanyAsync_TaxiCompanyEmail_ReturnsTaxiCompanyEmail()
        {
            TaxiCompany taxicompanyAddedToDatabase = new TaxiCompany
            {
                Email = "valid@email.com",
                Name = "TaxiCompanyName",
                PhoneNumber = "45612378",
            };

            using (var content = _factory.CreateContext())
            {
                content.TaxiCompanies.Add(taxicompanyAddedToDatabase);
                content.SaveChanges();
            }


            var taxicompanyReturned = _uut.GetTaxiCompanyAsync(taxicompanyAddedToDatabase.Email).Result;
            Assert.That(taxicompanyReturned.Email, Is.EqualTo("valid@email.com"));
        }

        //Testing Taxi Company Number
        [Test]
        public void GetTaxiCompanyAsync_TaxiCompanyPhoneNumber_ReturnsTaxiCompanyPhoneNumber()
        {
            TaxiCompany taxicompanyAddedToDatabase = new TaxiCompany
            {
                Email = "valid@email.com",
                Name = "TaxiCompanyName",
                PhoneNumber = "45612378",
            };

            using (var content = _factory.CreateContext())
            {
                content.TaxiCompanies.Add(taxicompanyAddedToDatabase);
                content.SaveChanges();
            }


            var taxicompanyReturned = _uut.GetTaxiCompanyAsync(taxicompanyAddedToDatabase.Email).Result;
            Assert.That(taxicompanyReturned.PhoneNumber, Is.EqualTo("45612378"));
        }

        [TearDown]
        public void TearDown()
        {
            _factory.Dispose();
        }

        [Test]
        public void GetTaxiCompanyAsync_NoTaxiCompany_ThrowsExceptionWithMessage()
        {
            try
            {
                _uut.GetTaxiCompanyAsync("DanTaxi@mail.com"); 

            }
            catch(ArgumentException e)
            {
                Assert.That(e.Message, Is.EqualTo("TaxiCompany does not exist."));
            }
        }

        [Test]
        public void GetTaxiCompanyAsync_NoTaxiCompany_ThrowsNotFound()
        {
            Assert.ThrowsAsync<UserIdInvalidException>(() => _uut.GetTaxiCompanyAsync("DanTaxi@mail.com"));
        }
    }
}