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
        
        protected ApplicationContextFactory _applicationContextFactory;

        [OneTimeSetUp]
        public void OneTimeSetup()
        {
            
            
            
            //_applicationContextFactory.CreateContext().Database.Migrate();
        }

        [SetUp]
        public void Setup()
        {
            string guid = Guid.NewGuid().ToString();
            _applicationContextFactory = new ApplicationContextFactory(guid);
            var webFactory = new EmptyDB_WebApplicationFactory<Startup>(guid);
            _client = webFactory.CreateClient();
        }


        [TearDown]
        public void TearDown()
        {
        }

        [OneTimeTearDown]
        public void OneTimeTearDown()
        {

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