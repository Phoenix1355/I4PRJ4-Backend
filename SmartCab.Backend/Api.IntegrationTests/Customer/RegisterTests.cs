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
    public class RegisterTest : IntegrationSetup
    {

        [Test]
        public async Task Register_ValidRequest_StatusOk()
        {
            var request = getRegisterRequest();

            var response = await PostAsync("/api/customer/register", request);
            
            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));
        }

        [Test]
        public async Task Register_ValidRequest_ElementExistInDatabase()
        {
            var request = getRegisterRequest();

            var response = await PostAsync("/api/customer/register", request);


            using (var context = _factory.CreateContext())
            {
                var entry = context.Customers.Where(x => x.Email == request.Email);
                Assert.That(entry.Count, Is.EqualTo(1));
            }

           
        }

        [Test]
        public async Task Register_RequestTwice_FirstOk()
        {
            var request = getRegisterRequest();

            var responseFirstRequest = await PostAsync("/api/customer/register", request);

            var responseSecondRequest = await PostAsync("/api/customer/register", request);

            Assert.That(responseFirstRequest.StatusCode, Is.EqualTo(HttpStatusCode.OK));
        }

        [Test]
        public async Task Register_RequestTwice_SecondBadRequest()
        {
            var request = getRegisterRequest();

            var responseFirstRequest = await PostAsync("/api/customer/register", request);

            var responseSecondRequest = await PostAsync("/api/customer/register", request);

            Assert.That(responseSecondRequest.StatusCode, Is.EqualTo(HttpStatusCode.BadRequest));

           
        }

        //See LoginRequestTest for full range. 
        [TestCase("a.gmail.com")]
        [TestCase("plaintext")]
        [TestCase("#@%^%#$@#$@#.com")]
        [TestCase("Michael Moeller <email@domain.com>")]
        public async Task Register_InvalidRequestEmail_GetsBadRequest(string email)
        {
            var request = getRegisterRequest(email);

            var responseFirstRequest = await PostAsync("/api/customer/register", request);

            Assert.That(responseFirstRequest.StatusCode, Is.EqualTo(HttpStatusCode.BadRequest));
        }


        
        [TestCase("","")]
        [TestCase("111111", "111111")]
        [TestCase("111111#", "111111#")]
        [TestCase("12345678", "12345678")]
        [TestCase("232425aA", "2324252aA")]
        [TestCase("999999aA#", "999999aA¤")]
        [TestCase("99995599aA#", "99992399aA¤")]
        public async Task Register_InvalidRequestPassword_GetsBadRequest(string password, string passwordRepeated)
        {
            Console.WriteLine(password);
            var request = getRegisterRequest("test12@gmail.com","12345678",password,passwordRepeated);

            var responseFirstRequest = await PostAsync("/api/customer/register", request);

            Assert.That(responseFirstRequest.StatusCode, Is.EqualTo(HttpStatusCode.BadRequest));
        }
    }
}