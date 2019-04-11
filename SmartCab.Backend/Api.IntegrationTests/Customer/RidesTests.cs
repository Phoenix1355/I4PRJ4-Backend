using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Api.BusinessLogicLayer.Requests;
using Api.DataAccessLayer.UnitTests.Factories;
using Api.Responses;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.TestHost;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using Newtonsoft.Json;
using NUnit.Framework;
using SmartCabPoc.Integration.Test;
using SmartCabPoc.Integration;
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