using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Api.BusinessLogicLayer.DataTransferObjects;
using NUnit.Framework;
using NUnit.Framework.Internal;

namespace Api.IntegrationTests.Order
{
    [TestFixture()]
    class DetailsTests : IntegrationSetup
    {
        private async Task<HttpResponseMessage> LoginAndGetDetailedResponse(int id = 1)
        {
            await CreateRideWithLogin();

            ClearHeaders();
            //Login
            await LoginOnTaxiCompanyAccount("anotherEmail@test.com");

            var uri = "/api/order/" + id + "/details";
            var response = await _client.GetAsync(uri);
            return response;
        }

        [Test]
        public async Task Details_OrderExist_ReturnsStatusOk()
        {
            var response = await LoginAndGetDetailedResponse();

            Assert.That(response.StatusCode,Is.EqualTo(HttpStatusCode.OK));
        }

        [Test]
        public async Task Details_OrderDoesNotExist_ReturnsBadRequest()
        {
            //Only a order with id 1. 

            await CreateRideWithLogin();

            ClearHeaders();
            //Login
            await LoginOnTaxiCompanyAccount("anotherEmail@test.com");

            var uri = "/api/order/" + 2 + "/details";
            var response = await _client.GetAsync(uri);
            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.Unauthorized));
        }

        [Test]
        public async Task Details_OrderExist_ReturnsExpectedOrder()
        {
            //Expect order with id 1. 
            var response = await LoginAndGetDetailedResponse();
            var responseObject = GetObject<OrderDetailedDto>(response);

            Assert.That(responseObject.Id, Is.EqualTo(1));
        }
    }
}
