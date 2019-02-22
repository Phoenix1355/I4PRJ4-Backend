using System;
using System.Net.Http;
using NUnit.Framework;
using System.Text;


namespace SmartCabPoc.Integration.Test
{
    [TestFixture]
    public class Integration_Test
    {
        
        //Maybe instead of client be system under test == Sut, or Unit under test?
        private HttpClient _client;

        //Setup a new client through custom web application factory -> Enforces empty test database in memory for every test. 
        [SetUp]
        public void Setup()
        {
            var _factory = new EmptyDBInMemory_WebApplicationFactory<SmartCabPoC.WebAPILayer.Startup>();
            _client = _factory.CreateClient();
        }

        [Test]
        public void FirstTest()
        {
            //Create
            string myJson = "{\"departureTime\": \"" + DateTime.Now.ToString("yyyy-MM-dd HH:mm") + "\"}";
            StringContent queryString = new StringContent(myJson, Encoding.UTF8, "application/json");

            //Act
            _client.PostAsync("/api/rides", queryString).Wait();
            _client.PostAsync("/api/rides", queryString).Wait();
            _client.PostAsync("/api/rides", queryString).Wait();

            //Receive - Should contain one message of the desired format. 
            var response = _client.GetAsync("/api/rides");

            //Validate
            string responseBody = response.Result.Content.ReadAsStringAsync().Result;           
            Console.WriteLine(responseBody);

            //Very rough validation...
            Assert.True(responseBody.Contains("departureTime"));
        }
    }
}