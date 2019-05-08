using System.Net;
using System.Threading.Tasks;
using Api.Responses;
using NUnit.Framework;

namespace Api.IntegrationTests.Customer
{
    [TestFixture]
    public class RidesTests : IntegrationSetup
    {

        [Test]
        public async Task Rides_CustomerNotLoggedIn_ReturnsUnAuthorized()
        {
            var response = await _client.GetAsync("api/customer/rides");
            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.Unauthorized));
        }

        [Test]
        public async Task Rides_CustomerExist_ReturnsOk()
        {
            await LoginOnCustomerAccount();
            await DepositToCustomer(1000);

            var response = await _client.GetAsync("api/customer/rides");
            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));
        }

        [Test]
        public async Task Rides_CustomerExist_ReturnsEmptyList()
        {
            await LoginOnCustomerAccount();
            await DepositToCustomer(1000);

            var response = await _client.GetAsync("api/customer/rides");
            var responseObject = GetObject<CustomerRidesResponse>(response);

            Assert.That(responseObject.Rides.Count, Is.EqualTo(0));
        }

        [Test]
        public async Task Rides_CustomerExistWithRide_ReturnsListContaining1Ridet()
        {
            await CreateRideWithLogin();
            var response = await _client.GetAsync("api/customer/rides");
            var responseObject = GetObject<CustomerRidesResponse>(response);

            Assert.That(responseObject.Rides.Count, Is.EqualTo(1));
        }

        [Test]
        public async Task Rides_RideExistOnOtherCustomer_ReturnsEmptyList()
        {
            await CreateRideWithLogin();

            //Clear previous token
            _client.DefaultRequestHeaders.Clear();

            //Login on other account
            await LoginOnCustomerAccount("test13@gmail.com");

            var response = await _client.GetAsync("api/customer/rides");
            var responseObject = GetObject<CustomerRidesResponse>(response);

            Assert.That(responseObject.Rides.Count, Is.EqualTo(0));
        }

    }
}