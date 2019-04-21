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
        //-----------------------------------------INFO--------------------------------
        //   All rides cost 100 kr, no matter what adresses they contain. 
        //-----------------------------------------Info end --------------------------



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
        public async Task Create_WhenAuthorizedUserCallCreateWithValidRequest_OrderIsCreated()
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
                Assert.That(context.Orders.ToList().Count, Is.EqualTo(1));
            }
        }

        [Test]
        public async Task Create_WhenUnauthorizedUserCallCreateWithValidRequest_RideIsNotCreated()
        {
            var request = getCreateRideRequest();
            await PostAsync("api/rides/create", request);

            //No rides exist.
            using (var context = _factory.CreateContext())
            {
                Assert.That(context.Rides.ToList().Count, Is.EqualTo(0));
            }
        }

        [Test]
        public async Task Create_WhenUnauthorizedUserCallCreateWithValidRequest_OrderIsNotCreated()
        {
            var request = getCreateRideRequest();
            await PostAsync("api/rides/create", request);

            //No rides exist.
            using (var context = _factory.CreateContext())
            {
                Assert.That(context.Orders.ToList().Count, Is.EqualTo(0));
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
            await PostAsync("api/rides/create", request);

            using (var context = _factory.CreateContext())
            {
                Assert.That(context.Rides.ToList().Count, Is.EqualTo(0));
            }
        }

        [Test]
        public async Task Create_WhenAuthorizedUserCallCreateWithValidRequestButInsufficientFunds_OrderIsNotCreated()
        {
            await LoginOnCustomerAccount();

            //Create Ride Request
            var request = getCreateRideRequest();

            //Make request
            await PostAsync("api/rides/create", request);

            using (var context = _factory.CreateContext())
            {
                Assert.That(context.Orders.ToList().Count, Is.EqualTo(0));
            }
        }

        [Test]
        public async Task Create_WhenAuthorizedUserCallCreateWithValidRequestButInsufficientFunds_ErrorMessageReturned()
        {
            await LoginOnCustomerAccount();

            //Create Ride Request
            var request = getCreateRideRequest();

            //Make request
            var response = await PostAsync("api/rides/create", request);

            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.BadRequest));
        }

        [Test]
        public async Task Create_WhenAuthorizedUserCallCreateWithInvalidRequest_RideIsNotCreated()
        {
            await LoginOnCustomerAccount();

            //Create Ride Request
            var request = getCreateRideRequest();

            //Invadidate request
            request.PassengerCount = 99;

            //Make request
            await PostAsync("api/rides/create", request);

            using (var context = _factory.CreateContext())
            {
                Assert.That(context.Rides.ToList().Count, Is.EqualTo(0));
            }
        }

        [Test]
        public async Task Create_WhenAuthorizedUserCallCreateWithInvalidRequest_OrderIsNotCreated()
        {
            await LoginOnCustomerAccount();

            //Create Ride Request
            var request = getCreateRideRequest();

            //Invadidate request
            request.PassengerCount = 99;

            //Make request
            await PostAsync("api/rides/create", request);

            using (var context = _factory.CreateContext())
            {
                Assert.That(context.Orders.ToList().Count, Is.EqualTo(0));
            }
        }

        [Test]
        public async Task Create_WhenAuthorizedUserCallCreateWithInvalidRequest_ErrorMessageReturned()
        {
            await LoginOnCustomerAccount();

            //Create Ride Request
            var request = getCreateRideRequest();

            //Invadidate request
            request.PassengerCount = 99;

            //Make request
            var response = await PostAsync("api/rides/create", request);

            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.BadRequest));
        }

        [Test]
        public async Task Create_WhenAuthorizedUserCallsSameRequestSeveralTimes_StatusOk()
        {
            await LoginOnCustomerAccount();

            await DepositToCustomer(1000);

            //Create Ride Request
            var request = getCreateRideRequest();

            //Make request
            for (int x = 0; x < 5; x++)
            {
                var response = await PostAsync("api/rides/create", request);
                Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));
            }
        }

        [Test]
        public async Task Create_WhenAuthorizedUserCallsSameRequestSeveralTimes_SeveralRidesIsCreated()
        {
            await LoginOnCustomerAccount();

            await DepositToCustomer(1000);

            //Create Ride Request
            var request = getCreateRideRequest();

            //Make request
            for (int x = 0; x < 5; x++)
            {
                var response = await PostAsync("api/rides/create", request);
            }

            using (var context = _factory.CreateContext())
            {
                Assert.That(context.Rides.Count(), Is.EqualTo(5));
            }
        }

        [Test]
        public async Task Create_WhenAuthorizedUserCallsSameRequestSeveralTimes_SeveralOrdersIsCreated()
        {
            await LoginOnCustomerAccount();

            await DepositToCustomer(1000);

            //Create Ride Request
            var request = getCreateRideRequest();

            //Make request
            for (int x = 0; x < 5; x++)
            {
                var response = await PostAsync("api/rides/create", request);
            }

            using (var context = _factory.CreateContext())
            {
                Assert.That(context.Orders.Count(), Is.EqualTo(5));
            }
        }

        [Test]
        public async Task Create_WhenAuthorizedUserCreatesUntilRunsOutOfFunds_LastOneFails()
        {
            await LoginOnCustomerAccount();

            await DepositToCustomer(1000);

            //Create Ride Request
            var request = getCreateRideRequest();

            //Make request
            for (int x = 0; x < 10; x++)
            {
                await PostAsync("api/rides/create", request);
            }
            //No more funds, everything reserved. 

            var response  = await PostAsync("api/rides/create", request);
            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.BadRequest));
        }


    }
}
