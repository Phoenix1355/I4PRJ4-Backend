using System;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Api;
using Api.BusinessLogicLayer.Enums;
using Api.BusinessLogicLayer.Requests;
using Api.BusinessLogicLayer.Responses;
using Api.DataAccessLayer.Models;
using Newtonsoft.Json;
using NUnit.Framework;
using SmartCabPoc.Integration.Test;
using TechTalk.SpecFlow;

namespace API.IntegrationsTests.Gherkin
{
    [Binding]
    public class ApiSteps
    {
        protected HttpClient _client;
        protected InMemoryApplicationFactory<FakeStartup> _factory;

        private string _defaultPassword = "Asdf123¤";

        [Given(@"The server is online")]
        public void GivenTheServerIsOnline()
        {
            string guid = Guid.NewGuid().ToString();

            _factory = new InMemoryApplicationFactory<FakeStartup>(guid);

            _client = _factory.CreateClient();
        }


        [Given(@"I have registered a customer with email '(.*)'")]
        [When(@"I have registered a customer with email '(.*)'")]
        public async Task GivenIHaveRegisteredACustomerWithEmail(string customerEmail)
        {
            var registerRequest = new RegisterRequest()
            {
                Email = customerEmail,
                Name = "TestCustomer",
                Password = _defaultPassword,
                PasswordRepeated = _defaultPassword,
                PhoneNumber = "12345678"
            };
            await PostAsync("api/customer/register", registerRequest);
        }


        [Then(@"the database contains a customer with email '(.*)'")]
        public void ThenTheDatabaseContainsACustomerWithEmail(string customerEmail)
        {
            using (var context = _factory.CreateContext())
            {
                Assert.That(context.Customers.Count(customer => customer.Email == customerEmail), Is.EqualTo(1));
            }
        }

        [Then(@"I can login with the email '(.*)'")]
        public async Task ThenICanLoginWithTheEmail(string customerEmail)
        {
            var response = await LoginOnCustomer(customerEmail);
            Assert.That(response.StatusCode,Is.EqualTo(HttpStatusCode.OK));
        }

        [When(@"I have logged in with email '(.*)'")]
        public async Task WhenIHaveLoggedInWithEmail(string customerEmail)
        {
            await LoginOnCustomerAndAttachedToken(customerEmail);
        }


        [When(@"I have deposited (.*) kr")]
        public async Task WhenIHaveDepositedKr(int amount)
        {
            DepositRequest request = new DepositRequest()
            {
                Deposit = 1000
            };

            await PutAsync("/api/customer/deposit", request);
        }


        [Then(@"I can create a ride")]
        public async Task ThenICanCreateARide()
        {
            var rideRequest = getCreateRideRequest();

            var response = await PostAsync("api/rides/create", rideRequest);
            Console.WriteLine(await response.Content.ReadAsStringAsync());
            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));
        }


        #region Helper Methods

        private async Task<HttpResponseMessage> LoginOnCustomer(string customerEmail)
        {
            var loginRequest = new LoginRequest()
            {
                Email = customerEmail,
                Password = _defaultPassword
            };

            return await PostAsync("api/customer/login", loginRequest);
        }

        protected async Task<HttpResponseMessage> PutAsync(string endPointUrl, object data)
        {
            var json = JsonConvert.SerializeObject(data);
            var response = await _client.PutAsync(endPointUrl, new StringContent(json, Encoding.UTF8, "application/json"));
            return response;
        }

        private async Task LoginOnCustomerAndAttachedToken(string customerEmail)
        {
            var response = await LoginOnCustomer(customerEmail);
            var responseObject = GetObject<LoginResponse>(response);
            _client.DefaultRequestHeaders.Add("authorization", "Bearer " + responseObject.Token);
        }

        protected T GetObject<T>(HttpResponseMessage response)
        {
            return JsonConvert.DeserializeObject<T>(response.Content.ReadAsStringAsync().Result);
        }

        protected async Task<HttpResponseMessage> PostAsync(string endPointUrl, object data)
        {
            var json = JsonConvert.SerializeObject(data);
            var response = await _client.PostAsync(endPointUrl, new StringContent(json, Encoding.UTF8, "application/json"));
            return response;
        }

        protected CreateRideRequest getCreateRideRequest(RideType type = RideType.SoloRide)
        {
            return new CreateRideRequest()
            {
                ConfirmationDeadline = DateTime.Now.AddSeconds(1), //added one second because those dates must be in the future
                DepartureTime = DateTime.Now.AddSeconds(1),
                StartDestination = new Address("City", 8000, "Street", 21),
                EndDestination = new Address("City", 8000, "Street", 21),
                RideType = type,
                PassengerCount = 2
            };
        }

        #endregion
    }
}
