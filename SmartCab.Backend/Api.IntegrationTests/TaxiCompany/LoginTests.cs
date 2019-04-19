using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Api.BusinessLogicLayer.Responses;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NSubstitute.Extensions;
using NUnit.Framework;

namespace Api.IntegrationTests.TaxiCompany
{
    [TestFixture]
    public class LoginTests : IntegrationSetup
    {
       
        [Test]
        public async Task Login_TaxiCompanyExists_LogsInAndReturnsTaxiCompany()
        {
            var request = getRegisterRequest();

            await PostAsync("/api/taxicompany/register", request);

            var loginRequest = getLoginRequest();

            var response = await PostAsync("/api/taxicompany/login", loginRequest);
            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));
        }

        [Test]
        public async Task Login_TaxiCompanyExists_LogsInAndReturnsTaxiCompanyWithCorrectEmail()
        {
            // Using client to create taxi company
            var request = getRegisterRequest();
            await PostAsync("/api/taxicompany/register", request);

            var loginRequest = getLoginRequest();

            var response = await PostAsync("/api/taxicompany/login", loginRequest);

            var responseObject = GetObject<LoginResponseTaxiCompany>(response);
            
            Assert.That(responseObject.TaxiCompany.Email, Is.EqualTo(request.Email));
        }

        [Test]
        public async Task Login_TaxiCompanyDoesNotExist_ReturnsBadRequest()
        {
            var loginRequest = getLoginRequest();

            var response = await PostAsync("api/taxicompany/login", loginRequest);

            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.BadRequest));
        }

        [TestCase("")]
        [TestCase("something")]
        [TestCase("alllowercase")]
        [TestCase("CAPSLETTERS")]
        [TestCase("CAPSWITHNUMBER1")]
        [TestCase("lowercasewithnumber1")]
        [TestCase("specialcharacter#")]
        public async Task Login_TaxiCompanyExistsButWrongPasswordFormat_ReturnsBadRequest(string password)
        {
            // Using client to create taxi company
            var request = getRegisterRequest();

            await PostAsync("/api/taxicompany/register", request);

            var loginRequest = getLoginRequest("test2@gmail.com", password);

            var response = await PostAsync("/api/taxicompany/login", loginRequest);

            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.BadRequest));
        }

        [Test]
        public async Task Login_TaxiCompanyExistsButWrongPasswordButRightFormat_ReturnsBadRequest()
        {
            // Using client to create taxi company
            var request = getRegisterRequest();

            await PostAsync("/api/taxicompany/register", request);

            var loginRequest = getLoginRequest("test9@mail.com", "Qwererrr1111!");

            var response = await PostAsync("/api/taxicompany/login", loginRequest);

            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.BadRequest));
        }
    }
}