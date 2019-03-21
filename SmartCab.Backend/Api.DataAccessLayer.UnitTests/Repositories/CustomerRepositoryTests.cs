using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Api.DataAccessLayer.Models;
using Api.DataAccessLayer.Repositories;
using Api.DataAccessLayer.UnitTests.Factories;
using Microsoft.Data.Sqlite;
using NSubstitute;
using NUnit.Framework;
using Microsoft.EntityFrameworkCore;

namespace Api.DataAccessLayer.UnitTests.Repositories
{
    [TestFixture]
    class CustomerRepositoryTests
    {
        private CustomerRepository uut;
        private ApplicationContextFactory factory;

        [SetUp]
        void SetUp()
        {
            factory = new ApplicationContextFactory();
            uut = new CustomerRepository(factory.CreateContext());
        }

        [TearDown]
        void TearDown()
        {
            factory.Dispose();
        }

        [Test]
        void firstTest()
        {
            Customer testCustomer = new Customer
            {
                ApplicationUserId = "ID",
                Name = "Name",
                PhoneNumber = "22447682",
            };

            uut.AddCustomerAsync(testCustomer).Wait();

            using (var content = factory.CreateContext())
            {
                var customerInserted = content.Customers.FirstOrDefault(Customer => Customer.ApplicationUserId.Equals("ID"));

                Assert.That(customerInserted.Name, Is.EqualTo("Name"));
            }
        }
        
    }
}
