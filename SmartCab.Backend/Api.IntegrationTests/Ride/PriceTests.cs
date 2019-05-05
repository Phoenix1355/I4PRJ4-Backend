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

            await DepositToCustomer(1000);

            //Create Ride Request
            var request = getPriceRequest();
            

            //Make request
            var response = await PostAsync("api/rides/price", request);

            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));
        }
    }
}
