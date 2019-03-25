using System.Diagnostics;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Api.BusinessLogicLayer.Requests;
using Api.DataAccessLayer.UnitTests.Factories;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.TestHost;
using Newtonsoft.Json;
using NUnit.Framework;
using SmartCabPoc.Integration.Test;

namespace Api.IntegrationTests.Customers
{
    [TestFixture]
    public class CustomerIntegrationTest
    {
        private HttpClient _client;
        private SqliteConnectionFactory _connectionFactory;

        [SetUp]
        public void Setup()
        {
            _connectionFactory = new SqliteConnectionFactory();
            var webFactory = new EmptyDB_WebApplicationFactory<Startup>(_connectionFactory.Connection);

            _client = webFactory.CreateClient();
        }

        [TearDown]
        public void TearDown()
        {
            _connectionFactory.Dispose();
        }

        [Test]
        public async Task Register_WhenCalled_Todo()
        {
            var request = new RegisterRequest
            {
                Email = "test4@gmail.com",
                Password = "Qwer111!",
                PasswordRepeated = "Qwer111!",
                Name = "test4",
                PhoneNumber = "12345678"
            };

            var response = await PostAsync("/api/customer/register", request);

            var responseBodyAsText = await response.Content.ReadAsStringAsync();
            Debug.WriteLine(responseBodyAsText);

            Assert.That(HttpStatusCode.OK, Is.EqualTo(response.StatusCode));
        }


        private async Task<HttpResponseMessage> PostAsync(string endPointUrl, object data)
        {
            var json = JsonConvert.SerializeObject(data);
            var response = await _client.PostAsync(endPointUrl, new StringContent(json, Encoding.UTF8, "application/json"));
            return response;
        }
    }
}