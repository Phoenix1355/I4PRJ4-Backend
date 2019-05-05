using System;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Api.BusinessLogicLayer.Enums;
using Api.BusinessLogicLayer.Requests;
using Api.BusinessLogicLayer.Responses;
using Api.DataAccessLayer.Models;
using Api.DataAccessLayer.Statuses;
using NUnit.Framework;

namespace Api.IntegrationTests.Ride
{

    [TestFixture]
    class PriceTests : IntegrationSetup
    {
        [Test]
        public async Task Price_ValidRequest_ReturnsStatusCodeOK()
        {
            await LoginOnCustomerAccount();

            var request = getPriceRequest();

            var response = await PostAsync("api/rides/price", request);

            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));
        }

        [Test]
        public async Task Price_WhenUnauthorizedUserCallPriceWithValidRequest_ErrorMessageReturned()
        {
            var request = getPriceRequest();
            var response = await PostAsync("api/rides/price", request);
            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.Unauthorized));
        }

        [Test]
        public async Task Price_WhenAuthorizedUserCallPriceWithInvalidRequest_ErrorMessageReturned()
        {
            await LoginOnCustomerAccount();

            var request = getPriceRequest();

            // Invalidate request
            request.EndAddress = new Address("Århus", 800, "StreetName", 1);

            var response = await PostAsync("api/rides/price", request);

            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.BadRequest));
        }
    }
}
