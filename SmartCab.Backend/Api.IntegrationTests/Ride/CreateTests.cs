using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Api.BusinessLogicLayer.Requests;
using Api.BusinessLogicLayer.Responses;
using Api.DataAccessLayer.Models;
using Microsoft.AspNetCore.Diagnostics;
using NUnit.Framework;

namespace Api.IntegrationTests.Ride
{
    [TestFixture]
    class CreateTests : IntegrationSetup
    {
        [Test]
        public async Task Create_WhenAuthorizedUserCallCreateWithValidRequest_ReturnedStatusOk()
        {

            await LoginOnCustomerAccount();

            await DepositToCustomer(1000);

            //Create Ride Request
            var request = getCreateRideRequest();

            //Make request
            var response = await PostAsync("api/rides/create", request);

           Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));
        }

        [Test]
        public async Task Create_WhenAuthorizedUserCallCreateWithValidRequest_RideIsCreated()
        {

            await LoginOnCustomerAccount();

            await DepositToCustomer(1000);

            //Create Ride Request
            var request = getCreateRideRequest();

            //Make request
            var response = await PostAsync("api/rides/create", request);
            Console.WriteLine(await response.Content.ReadAsStringAsync());

            using (var context = _factory.CreateContext())
            {
                Assert.That(context.Rides.ToList().Count, Is.EqualTo(1));
            }
        }



        [Test]
        public async Task Create_WhenUnauthorizedUserCallCreateWithValidRequest_RideIsNotCreated()
        {
            var request = getCreateRideRequest();
            var response = await PostAsync("api/rides/create", request);

            //No rides exist.
            using (var context = _factory.CreateContext())
            {
                Assert.That(context.Rides.ToList().Count, Is.EqualTo(0));
            }
        }

        [Test]
        public async Task Create_WhenUnauthorizedUserCallCreateWithValidRequest_ErrorMessageReturned()
        {
            var request = getCreateRideRequest();
            var response = await PostAsync("api/rides/create", request);
            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.Unauthorized));
        }

        [Test]
        public async Task Create_WhenAuthorizedUserCallCreateWithValidRequestButInsufficientFunds_RideIsNotCreated()
        {
            await LoginOnCustomerAccount();

            //Create Ride Request
            var request = getCreateRideRequest();

            //Make request
            var response = await PostAsync("api/rides/create", request);

            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));
        }

        [Test]
        public async Task Create_WhenAuthorizedUserCallCreateWithValidRequestButInsufficientFunds_ErrorMessageReturned()
        {

        }

        [Test]
        public async Task Create_WhenAuthorizedUserCallCreateWithInvalidRequest_RideIsNotCreated()
        {

        }

        [Test]
        public async Task Create_WhenAuthorizedUserCallCreateWithInvalidRequest_ErrorMessageReturned()
        {

        }

        [Test]
        public async Task Create_WhenAuthorizedUserCallsSameRequestSeveralTimes_SeveralRidesIsCreated()
        {

        }

        private CreateRideRequest getCreateRideRequest()
        {
            return new CreateRideRequest()
            {
                ConfirmationDeadline = DateTime.Now,
                DepartureTime = DateTime.Now,
                StartDestination = new Address("City", 8000, "Street", 21),
                EndDestination = new Address("City", 8000, "Street", 21),
                IsShared = false,
                PassengerCount = 2
            };
        }


        private async Task LoginOnCustomerAccount()
        {
            //Create customer
            var registerRequest = getRegisterRequest();
            await PostAsync("/api/customer/register", registerRequest);

            //Login on customer
            var loginRequest = getLoginRequest();
            var loginResponse = await PostAsync("/api/customer/login", loginRequest);

            //Map login returned to object
            var loginResponseObject = GetObject<LoginResponse>(loginResponse);

            //Get Token
            var token = loginResponseObject.Token;

            //Default header authentication setup.
            _client.DefaultRequestHeaders.Add("authorization", "Bearer " + token);
        }

        private async Task DepositToCustomer(int amount)
        {
            DepositRequest request = new DepositRequest()
            {
                Deposit = amount
            };
            
            await PutAsync("/api/customer/deposit", request);
        }
    }
}
