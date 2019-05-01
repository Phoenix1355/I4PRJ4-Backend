using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Api.BusinessLogicLayer.DataTransferObjects;
using Api.BusinessLogicLayer.Enums;
using Api.BusinessLogicLayer.Responses;
using Api.DataAccessLayer.Models;
using Api.DataAccessLayer.Statuses;
using NUnit.Framework;

namespace Api.IntegrationTests.Order
{
    [TestFixture]
    class AcceptTests : IntegrationSetup
    {
        [Test]
        public async Task Accept_OrderDoesNotExist_UnauthorizedResponse()
        {
            //Login
            await LoginOnTaxiCompanyAccount();

            var id = 1;
            var uri = "/api/order/" + id + "/accept";
            var response = await PutAsync(uri, null);
            
            Assert.That(response.StatusCode,Is.EqualTo(HttpStatusCode.Unauthorized));
        }

        [Test]
        public async Task Accept_NotLoggedIn_UnauthorizedResponse()
        {

            var id = 1;
            var uri = "/api/order/" + id + "/accept";
            var response = await PutAsync(uri, null);

            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.Unauthorized));
        }

        [Test]
        public async Task Accept_OrderDoesExist_OKResponse()
        {
            await CreateRideWithLogin();

            ClearHeaders();
            //Login
            await LoginOnTaxiCompanyAccount("anotherEmail@test.com");

            var id = 1;
            var uri = "/api/order/" + id + "/accept";
            var response = await PutAsync(uri, null);
            
            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));
        }

        [Test]
        public async Task Accept_OrderAlreadyAccepted_BadRequestResponse()
        {
            await CreateRideWithLogin();

            ClearHeaders();
            //Login
            await LoginOnTaxiCompanyAccount("anotherEmail@test.com");

            var id = 1;
            var uri = "/api/order/" + id + "/accept";
            await PutAsync(uri, null);
            var response = await PutAsync(uri, null);
            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.BadRequest));
        }

        [Test]
        public async Task Accept_OrderDoesExist_ResponseContainsExpectedValues()
        {
            await CreateRideWithLogin();

            ClearHeaders();
            //Login
            await LoginOnTaxiCompanyAccount("anotherEmail@test.com");

            var id = 1;
            var uri = "/api/order/" + id + "/accept";
            var response = await PutAsync(uri, null);
            var responseObject = GetObject<AcceptOrderResponse>(response);

            AcceptOrderResponse expectedResponse = new AcceptOrderResponse()
            {
                Order = new OrderDto()
                {
                    Id = 1,
                    Status = OrderStatus.Accepted.ToString(),
                    Price = 100,
                    Rides = new List<RideDto>()
                    {
                    }
                }
            };

            Assert.That(responseObject.Order.Status, Is.EqualTo(expectedResponse.Order.Status));
        }

        [Test]
        public async Task Accept_OrderDoesExist_OrderChangedToAccepted()
        {
            await CreateRideWithLogin();

            ClearHeaders();
            //Login
            await LoginOnTaxiCompanyAccount("anotherEmail@test.com");

            var id = 1;
            var uri = "/api/order/" + id + "/accept";
            var response = await PutAsync(uri, null);

            using (var context = _factory.CreateContext())
            {
                Assert.That(context.Orders.First().Status,Is.EqualTo(OrderStatus.Accepted));
            }

        }

        [Test]
        public async Task Accept_OrderDoesExist_RideChangedToAccepted()
        {
            await CreateRideWithLogin();

            ClearHeaders();
            //Login
            await LoginOnTaxiCompanyAccount("anotherEmail@test.com");

            var id = 1;
            var uri = "/api/order/" + id + "/accept";
            var response = await PutAsync(uri, null);

            using (var context = _factory.CreateContext())
            {
                Assert.That(context.Rides.First().Status, Is.EqualTo(RideStatus.Accepted));
            }
        }

        [Test]
        public async Task Accept_SharedRideOrderDoesExist_RidesChangedToAccepted()
        {
            await CreateRideWithLogin("test12@gmail.com", RideType.SharedRide);

            ClearHeaders();

            await CreateRideWithLogin("test13@gmail.com", RideType.SharedRide);

            ClearHeaders();
            //Login
            await LoginOnTaxiCompanyAccount("anotherEmail@test.com");

            var id = 1;
            var uri = "/api/order/" + id + "/accept";
            var response = await PutAsync(uri, null);

            using (var context = _factory.CreateContext())
            {
                var order = context.Orders.First();
                foreach (var orderRide in order.Rides)
                {
                    Assert.That(orderRide.Status,Is.EqualTo(RideStatus.Accepted));
                }
                
            }
        }
    }
}
