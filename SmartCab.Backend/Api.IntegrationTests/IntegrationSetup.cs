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
        protected InMemoryApplicationFactory<Startup> _factory;

        [SetUp]
        public void Setup()
        {
            string guid = Guid.NewGuid().ToString();
            
            _factory = new InMemoryApplicationFactory<Startup>(guid);
            
            _client = _factory.CreateClient();
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

        protected T GetObject<T>(HttpResponseMessage response)
        {
            return JsonConvert.DeserializeObject<T>(response.Content.ReadAsStringAsync().Result);
        }
    }
}