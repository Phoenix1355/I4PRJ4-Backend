using System;
using System.Collections.Generic;
using System.Net.Http;
using Api.DataAccessLayer.UnitTests.Factories;
using Microsoft.EntityFrameworkCore;
using NUnit.Framework;
using SmartCabPoc.Integration.Test;

namespace Api.IntegrationTests.Customer
{
    public class CustomerSetup
    {
        protected HttpClient _client;
        protected SqlConnectionFactory _connectionFactory;
        protected ApplicationContextFactory _applicationContextFactory;

        [OneTimeSetUp]
        public void OneTimeSetup()
        {
            _connectionFactory = new SqlConnectionFactory();
            _applicationContextFactory = new ApplicationContextFactory(_connectionFactory.Connection);
            _applicationContextFactory.CreateContext().Database.Migrate();
        }

        [SetUp]
        public void Setup()
        {
            var webFactory = new EmptyDB_WebApplicationFactory<Startup>(_connectionFactory.Connection);
            _client = webFactory.CreateClient();
        }


        [TearDown]
        public void TearDown()
        {
            //Cleaning up all entries in database.
            using (var content = _applicationContextFactory.CreateContext())
            {
                //All relevant tables for deletion.
                var tablesToDelete = new List<String>()
                {
                    "Customers", "AspNetUsers", "Rides", "MatchedRides", "TaxiCompanies", "CustomerRides",
                    "AspNetUserRoles", "AspNetRoles"
                };

                foreach (var table in tablesToDelete)
                {
                    string command = $"Delete from {table}";
                    content.Database.ExecuteSqlCommand(command);
                }
            }
        }

        [OneTimeTearDown]
        public void OneTimeTearDown()
        {

            _connectionFactory.Dispose();
        }
    }
}