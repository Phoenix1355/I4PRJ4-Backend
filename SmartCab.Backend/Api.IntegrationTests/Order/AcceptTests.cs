using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Api.BusinessLogicLayer.DataTransferObjects;
using Api.BusinessLogicLayer.Enums;
using Api.BusinessLogicLayer.Responses;
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
                    Status = OrderStatus.Debited.ToString(),
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
                Assert.That(context.Orders.First().Status,Is.EqualTo(OrderStatus.Debited));
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
                Assert.That(context.Rides.First().Status, Is.EqualTo(RideStatus.Debited));
            }
        }

        [TestCase(100, 0)]
        [TestCase(200, 100)]
        [TestCase(400, 300)]
        public async Task Accept_OrderDoesExist_CustomerDebitedExpectedFixed100Price(int deposit, int expected)
        {
            await CreateRideWithLogin(deposit);

            ClearHeaders();
            //Login
            await LoginOnTaxiCompanyAccount("anotherEmail@test.com");

            var id = 1;
            var uri = "/api/order/" + id + "/accept";
            var response = await PutAsync(uri, null);

            using (var context = _factory.CreateContext())
            {
                Assert.That(context.Customers.First().Balance, Is.EqualTo(expected));
            }
        }

        [TestCase(100, 0)]
        [TestCase(200, 0)]
        [TestCase(400, 0)]
        public async Task Accept_OrderDoesExist_CustomerReservedExpectedFixed100Price(int deposit, int expected)
        {
            await CreateRideWithLogin(deposit);

            ClearHeaders();
            //Login
            await LoginOnTaxiCompanyAccount("anotherEmail@test.com");

            var id = 1;
            var uri = "/api/order/" + id + "/accept";
            var response = await PutAsync(uri, null);

            using (var context = _factory.CreateContext())
            {
                Assert.That(context.Customers.First().ReservedAmount, Is.EqualTo(expected));
            }
        }

        [Test]
        public async Task Accept_SharedRideOrderDoesExist_RidesChangedToDebited()
        {
            await CreateRideWithLogin(1000,"test12@gmail.com", RideType.SharedRide);

            ClearHeaders();

            await CreateRideWithLogin(1000, "test13@gmail.com", RideType.SharedRide);

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
                    Assert.That(orderRide.Status,Is.EqualTo(RideStatus.Debited));
                }
                
            }
        }

        [Test]
        public async Task Accept_SharedRideOrderDoesExist_CustomerDebitedExpectedAmount()
        {
            await CreateRideWithLogin(1000, "test12@gmail.com", RideType.SharedRide);

            ClearHeaders();

            await CreateRideWithLogin(1000, "test13@gmail.com", RideType.SharedRide);

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
                    Assert.That(orderRide.Customer.Balance, Is.EqualTo(925));
                }

            }
        }

    }
}
