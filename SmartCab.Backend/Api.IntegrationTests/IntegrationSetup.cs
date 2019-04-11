using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using Api.BusinessLogicLayer.Requests;
using Api.BusinessLogicLayer.Responses;
using Api.DataAccessLayer.Models;
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

        protected async Task<HttpResponseMessage> PutAsync(string endPointUrl, object data)
        {
            var json = JsonConvert.SerializeObject(data);
            var response = await _client.PutAsync(endPointUrl, new StringContent(json, Encoding.UTF8, "application/json"));
            return response;
        }

        protected T GetObject<T>(HttpResponseMessage response)
        {
            return JsonConvert.DeserializeObject<T>(response.Content.ReadAsStringAsync().Result);
        }

        protected CreateRideRequest getCreateRideRequest()
        {
            return new CreateRideRequest()
            {
                ConfirmationDeadline = DateTime.Now.AddSeconds(1), //added one second because those dates must be in the future
                DepartureTime = DateTime.Now.AddSeconds(1),
                StartDestination = new Address("City", 8000, "Street", 21),
                EndDestination = new Address("City", 8000, "Street", 21),
                IsShared = false,
                PassengerCount = 2
            };
        }


        protected async Task LoginOnCustomerAccount(string email = "test12@gmail.com")
        {
            //Create customer
            var registerRequest = getRegisterRequest(email);
            await PostAsync("/api/customer/register", registerRequest);

            //Login on customer
            var loginRequest = getLoginRequest(email);
            var loginResponse = await PostAsync("/api/customer/login", loginRequest);

            //Map login returned to object
            var loginResponseObject = GetObject<LoginResponse>(loginResponse);

            //Get Token
            var token = loginResponseObject.Token;

            //Default header authentication setup.
            
            
           _client.DefaultRequestHeaders.Add("authorization", "Bearer " + token);
        }

        protected async Task DepositToCustomer(int amount)
        {
            DepositRequest request = new DepositRequest()
            {
                Deposit = amount
            };

            var response = await PutAsync("/api/customer/deposit", request);
        }

        protected async Task CreateRide()
        {
            var request = getCreateRideRequest();

            //Make request
            var response = await PostAsync("api/rides/create", request);
        }

        protected async Task CreateRideWithLogin()
        {
            await LoginOnCustomerAccount();
            await DepositToCustomer(1000);
            await CreateRide();
        }
    }

}