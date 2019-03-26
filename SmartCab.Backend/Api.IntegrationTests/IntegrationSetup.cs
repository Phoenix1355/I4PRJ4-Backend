using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Api.BusinessLogicLayer.Requests;
using Api.DataAccessLayer.UnitTests.Factories;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using NUnit.Framework;
using SmartCabPoc.Integration.Test;

namespace Api.IntegrationTests
{
    public class IntegrationSetup
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

        protected LoginRequest getLoginRequest(string email = "test12@gmail.com",
            string password = "Qwer111!")
        {
            return new LoginRequest()
            {
                Email = email,
                Password = password
            };
        }

        protected RegisterRequest getRegisterRequest(string email = "test12@gmail.com",
            string phonenumber = "12345678",
            string password = "Qwer111!",
            string passwordRepeated = "Qwer111!")
        {
            return new RegisterRequest
            {
                Email = email,
                Password = password,
                PasswordRepeated = passwordRepeated,
                Name = "TestUser",
                PhoneNumber = phonenumber
            };
        }


        protected async Task<HttpResponseMessage> PostAsync(string endPointUrl, object data)
        {
            var json = JsonConvert.SerializeObject(data);
            var response = await _client.PostAsync(endPointUrl, new StringContent(json, Encoding.UTF8, "application/json"));
            return response;
        }
    }
}