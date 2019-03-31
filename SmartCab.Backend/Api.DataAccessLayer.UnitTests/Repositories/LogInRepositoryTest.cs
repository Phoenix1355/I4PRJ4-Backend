using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Net.Http;
using System.Runtime.CompilerServices;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using Api.DataAccessLayer.Models;
using Api.DataAccessLayer.Repositories;
using Api.DataAccessLayer.UnitTests.Factories;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Data.Sqlite;
using NSubstitute;
using NUnit.Framework;
using Microsoft.EntityFrameworkCore;

namespace Api.DataAccessLayer.UnitTests.Repositories
{
    [TestFixture]
    public class ApplicationUserRepository
    {
        private ApplicationUserRepository _uut;
        private ApplicationContextFactory _factory;

        [SetUp]
        public void Setup()
        {
            _factory = new ApplicationContextFactory();
            _uut = new ApplicationUserRepository(_factory.CreateContext());
        }

        [TearDown]
        public void TearDown()
        {
            _factory.Dispose();
        }







        /*
        [Test]
        public void GetCustomerAsyncc_NoCustomer_ThrowsNotFound()
        {
            Assert.ThrowsAsync<ArgumentException>(() =>
                _uut.GetCustomerAsync("NoEmail@mail.com"));
        }

        [Test]
        public void GetCustomerAsyncc_NoCustomer_ThroesContainsMessage()
        {
            try
            {
                _uut.ReturnsForAnyArgs("NoEmail@mail.com");
                //_uut.GetCustomerAsync("NoEmail@mail.com");
            }
            catch (ArgumentNullException e)
            {
                Assert.That(e.Message, Is.EqualTo("Customer does not exist."));
            }
        }
        */

        [Test]
        public void Dispose_DisposeOfObject_Disposes()
        {
            _uut.Dispose();
        }
    }
}
