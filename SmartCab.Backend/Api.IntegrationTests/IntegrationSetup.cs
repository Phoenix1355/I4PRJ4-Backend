using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using Api.BusinessLogicLayer.Enums;
using Api.BusinessLogicLayer.Requests;
using Api.BusinessLogicLayer.Responses;
using Api.DataAccessLayer.Models;
using Api.DataAccessLayer.UnitTests.Factories;
using Api.Requests;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using NUnit.Framework;
using SmartCabPoc.Integration.Test;

namespace Api.IntegrationTests
{
    public class IntegrationSetup
    {
        protected HttpClient _client;
        protected InMemoryApplicationFactory<FakeStartup> _factory;

        [SetUp]
        public void Setup()
        {
            string guid = Guid.NewGuid().ToString();
            
            _factory = new InMemoryApplicationFactory<FakeStartup>(guid);
            
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
        protected EditCustomerRequest getEditRequest(string name = "Test Tester",
            string email = "test@gmail.com",
            string password = "Qwer11122!",
            string passwordRepeated = "Qwer11122!",
            string oldPassword = "Qwer111!",
            string phoneNumber = "99999999",
            bool changePassword = false)
        {
            return new EditCustomerRequest
            {
                Email = email,
                Password = password,
                Name = name,
                RepeatedPassword = passwordRepeated,
                OldPassword = oldPassword,
                PhoneNumber = phoneNumber,
                ChangePassword = changePassword
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

        protected CreateRideRequest getCreateRideRequest(RideType type = RideType.SoloRide, int minutes = 0)
        {
            return new CreateRideRequest()
            {
                ConfirmationDeadline = DateTime.Now.AddSeconds(1).AddMinutes(-minutes), //added one second because those dates must be in the future
                DepartureTime = DateTime.Now.AddSeconds(1),
                StartDestination = new Address("City", 8000, "Street", 21),
                EndDestination = new Address("City", 8000, "Street", 21),
                RideType = type,
                PassengerCount = 2
            };
        }

        protected PriceRequest getPriceRequest(RideType type = RideType.SoloRide)
        {
            return new PriceRequest()
            {
                StartAddress = new Address("Aarhus", 8000, "Søgade", 20),
                EndAddress = new Address("Åbyhøj", 8230, "Søren Frichs Vej", 20),
                RideType = type,
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

        protected async Task LoginOnTaxiCompanyAccount(string email = "test12@gmail.com")
        {

            var registerRequest = getRegisterRequest(email);
            var response = await PostAsync("/api/taxicompany/register", registerRequest);
            
            //Login on customer
            var loginRequest = getLoginRequest(email);
            var loginResponse = await PostAsync("/api/taxicompany/login", loginRequest);

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

        protected async Task CreateRide(RideType type = RideType.SoloRide, int minutes = 0)
        {
            var request = getCreateRideRequest(type, minutes);

            //Make request
            var response = await PostAsync("api/rides/create", request);
        }

        protected async Task CreateRideWithLogin(int deposit = 1000,string email = "test12@gmail.com", RideType type = RideType.SoloRide)
        {
            await LoginOnCustomerAccount(email);
            await DepositToCustomer(deposit);
            await CreateRide(type);
        }

        protected void ClearHeaders()
        {
            _client.DefaultRequestHeaders.Clear();
        }


    }

}