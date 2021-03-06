﻿using System;
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
    class CreateTests : IntegrationSetup
    {
        //-----------------------------------------INFO--------------------------------
        //   AllAsync rides cost 100 kr, no matter what adresses they contain. 
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
        public async Task Create_WhenAuthorizedUserCallCreateWithValidSharedRequest_RideIsCreated()
        {

            await LoginOnCustomerAccount();

            await DepositToCustomer(1000);

            //Create Ride Request
            var request = getCreateRideRequest(RideType.SharedRide);

            //Make request
            var response = await PostAsync("api/rides/create", request);
            Console.WriteLine(await response.Content.ReadAsStringAsync());

            using (var context = _factory.CreateContext())
            {
                Assert.That(context.Rides.ToList().Count, Is.EqualTo(1));
            }
        }

        [Test]
        public async Task Create_WhenAuthorizedUserCallCreateWithValidSharedRequest_OrderIsNotCreated()
        {

            await LoginOnCustomerAccount();

            await DepositToCustomer(1000);

            //Create Ride Request
            var request = getCreateRideRequest(RideType.SharedRide);

            //Make request
            var response = await PostAsync("api/rides/create", request);
            Console.WriteLine(await response.Content.ReadAsStringAsync());

            using (var context = _factory.CreateContext())
            {
                Assert.That(context.Orders.Count(), Is.EqualTo(0));
            }
        }

        [Test]
        public async Task Create_WhenAuthorizedUserCreateSharedRidesWithRideThatsMatchable_OrderIsCreated()
        {
            //Cusstomer 1
            await LoginOnCustomerAccount();

            await DepositToCustomer(1000);

            //Create Ride Request
            var request1 = getCreateRideRequest(RideType.SharedRide);

            //Make request
            var response1 = await PostAsync("api/rides/create", request1);
            
            //Customer 2
            ClearHeaders();
            await LoginOnCustomerAccount("test13@gmail.com");

            await DepositToCustomer(1000);

            //Create Ride Request
            var request2 = getCreateRideRequest(RideType.SharedRide);

            //Make request
            var response2 = await PostAsync("api/rides/create", request2);

            using (var context = _factory.CreateContext())
            {
                Assert.That(context.Orders.Count(), Is.EqualTo(1));
            }
        }

        [Test]
        public async Task Create_SoloAndSharedRideDoesNotGetMatched_OrderIsNotCreated()
        {
            //Cusstomer 1
            await LoginOnCustomerAccount();

            await DepositToCustomer(1000);

            //Create Ride Request
            var request1 = getCreateRideRequest(RideType.SoloRide);

            //Make request
            var response1 = await PostAsync("api/rides/create", request1);

            //Customer 2
            ClearHeaders();
            await LoginOnCustomerAccount("test13@gmail.com");

            await DepositToCustomer(1000);

            //Create Ride Request
            var request2 = getCreateRideRequest(RideType.SharedRide);
            

            //Make request
            var response2 = await PostAsync("api/rides/create", request2);
            var response2Object = GetObject<CreateRideResponse>(response2);
            Assert.That(response2Object.Status, Is.EqualTo(RideStatus.LookingForMatch.ToString()));
            
        }

        [Test]
        public async Task Create_WhenAuthorizedUserCreateSharedRidesWithRideThatsMatchable_TheRideThatResultInMatchHasExpectedReturnStatus()
        {
            //Cusstomer 1
            await LoginOnCustomerAccount();

            await DepositToCustomer(1000);

            //Create Ride Request
            var request1 = getCreateRideRequest(RideType.SharedRide);

            //Make request
            var response1 = await PostAsync("api/rides/create", request1);

            //Customer 2
            ClearHeaders();
            await LoginOnCustomerAccount("test13@gmail.com");

            await DepositToCustomer(1000);

            //Create Ride Request
            var request2 = getCreateRideRequest(RideType.SharedRide);

            //Make request
            var response2 = await PostAsync("api/rides/create", request2);
            var response2object = GetObject<CreateRideResponse>(response2);
            Assert.That(response2object.Status, Is.EqualTo(RideStatus.WaitingForAccept.ToString()));
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
            for (int x = 0; x < 2; x++)
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
            for (int x = 0; x < 2; x++)
            {
                var response = await PostAsync("api/rides/create", request);
            }

            using (var context = _factory.CreateContext())
            {
                Assert.That(context.Rides.Count(), Is.EqualTo(2));
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
            for (int x = 0; x < 2; x++)
            {
                var response = await PostAsync("api/rides/create", request);
            }

            using (var context = _factory.CreateContext())
            {
                Assert.That(context.Orders.Count(), Is.EqualTo(2));
            }
        }

        [Test]
        public async Task Create_WhenAuthorizedUserCreatesUntilRunsOutOfFunds_LastOneFails()
        {
            await LoginOnCustomerAccount();

            await DepositToCustomer(100);

            //Create Ride Request
            var request = getCreateRideRequest();

            //Make request
            for (int x = 0; x < 1; x++)
            {
                await PostAsync("api/rides/create", request);
            }
            //No more funds, everything reserved. 

            var response  = await PostAsync("api/rides/create", request);
            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.BadRequest));
        }

        private CreateRideRequest getCreateRideRequest(RideType type = RideType.SoloRide)
        {
            return new CreateRideRequest()
            {
                ConfirmationDeadline = DateTime.Now.AddSeconds(1), //added one second because those dates must be in the future
                DepartureTime = DateTime.Now.AddSeconds(1),
                StartDestination = new Address("City", 8000, "Street", 21),
                EndDestination = new Address("City", 8000, "Street", 21),
                RideType = type,
                PassengerCount = 2
            };
        }


        private async Task LoginOnCustomerAccount(string email)
        {
            //Create customer
            var registerRequest = getRegisterRequest(email);
            Console.WriteLine("Email " +registerRequest.Email);
            var response = await PostAsync("/api/customer/register", registerRequest);

            Console.WriteLine("response: "+ await response.Content.ReadAsStringAsync());
            //Login on customer
            var loginRequest = getLoginRequest(email);
            var loginResponse = await PostAsync("/api/customer/login", loginRequest);
            Console.WriteLine("loginResponse: " + await loginResponse.Content.ReadAsStringAsync());
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
            
            var response = await PutAsync("/api/customer/deposit", request);
        }
    }
}
