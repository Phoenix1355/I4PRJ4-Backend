using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Api.DataAccessLayer.Models;
using Api.DataAccessLayer.Repositories;
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
        private ApplicationContext content;
        private DbConnection connection;

        [SetUp]
        void SetUp()
        {
            DbConnection connection = new SqliteConnection("DataSource=:memory:");
            connection.Open();
            var options = new DbContextOptionsBuilder<ApplicationContext>()
                .UseSqlite(connection)
                .Options;

            content = new ApplicationContext(options);
            uut = new CustomerRepository(content);
        }

        [TearDown]
        void TearDown() => connection.Dispose();

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

            var customerInserted = content.Customers.FirstOrDefault(Customer => Customer.ApplicationUserId.Equals("ID"));

            Assert.That(customerInserted.Name,Is.EqualTo("Name"));

        }
        
    }
}
