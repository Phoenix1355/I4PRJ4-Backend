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
using Api.BusinessLogicLayer.Responses;
using Api.DataAccessLayer.UnitTests.Factories;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.TestHost;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using Newtonsoft.Json;
using NUnit.Framework;
using SmartCabPoc.Integration.Test;

namespace Api.IntegrationTests.Customer
{
    [TestFixture]
    public class LoginTests : IntegrationSetup
    {
        [Test]
        public async Task Login_UserExists_LogsInAndReturnsCustomer()
        {
            //Using client to create customer. 
            var request = getRegisterRequest();
            await PostAsync("/api/customer/register", request);

            var loginRequest = getLoginRequest();

            var response = await PostAsync("/api/customer/login", loginRequest);
            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));
        }

        [Test]
        public async Task Login_UserExists_LogsInAndReturnsCustomerWithCorrectEmail()
        {
            //Using client to create customer. 
            var request = getRegisterRequest();
            await PostAsync("/api/customer/register", request);

            var loginRequest = getLoginRequest();

            var response = await PostAsync("/api/customer/login", loginRequest);

            var responseObject = GetObject<LoginResponse>(response);

            Assert.That(responseObject.Customer.Email, Is.EqualTo(request.Email));
        }

        [Test]
        public async Task Login_UserDoesNotExist_ReturnsBadRequest()
        {

            var loginRequest = getLoginRequest();

            var response = await PostAsync("/api/customer/login", loginRequest);

            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.BadRequest));
        }

        [TestCase("")]
        [TestCase("length")]
        [TestCase("alllowercase")]
        [TestCase("CapsLetters")]
        [TestCase("CapWithNumbers1")]
        [TestCase("lowercasewithnumbers1")]
        [TestCase("length#")]
        public async Task Login_UserExistButWrongPasswordFormat_ReturnsBadRequest(string password)
        {
            //Using client to create customer. 
            var request = getRegisterRequest();
            await PostAsync("/api/customer/register", request);

            var loginRequest = getLoginRequest("test12@gmail.com", password);

            var response = await PostAsync("/api/customer/login", loginRequest);

            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.BadRequest));
        }

        [Test]
        public async Task Login_UserExistButWrongPasswordButRightFormat_ReturnsBadRequest()
        {
            //Using client to create customer. 
            var request = getRegisterRequest();
            await PostAsync("/api/customer/register", request);

            var loginRequest = getLoginRequest("test12@gmail.com", "Qwer111#");

            var response = await PostAsync("/api/customer/login", loginRequest);

            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.BadRequest));
        }
    }
}