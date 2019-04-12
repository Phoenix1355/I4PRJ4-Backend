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
        private async Task<DataAccessLayer.Models.TaxiCompany> CreateTaxiCompanyAccount()
        {
            //Setup database
            var request = getRegisterRequest();
            var taxi = new DataAccessLayer.Models.TaxiCompany();
            await PostAsync("/api/customer/register", request);
            using (var context = _factory.CreateContext())
            {
                //Get data
                var customer = context.Customers.First();
                var roleId = context.UserRoles.Where(x => x.UserId == customer.Id).First().RoleId;

                //Set data
                taxi.PasswordHash = customer.PasswordHash;
                taxi.Email = customer.Email;

                //Add taxi
                context.TaxiCompanies.Add(taxi);
                context.SaveChanges();

                //Add role
                var userRole = new IdentityUserRole<string>()
                {
                    RoleId = roleId,
                    UserId = taxi.Id
                };
                //Save role
                context.UserRoles.Add(userRole);
                context.SaveChanges();
            }

            return taxi;
        }

        [Test]
        public async Task Login_TaxiCompanyExists_LogsInAndReturnsTaxiCompany()
        {
            await CreateTaxiCompanyAccount();

            var loginRequest = getLoginRequest();

            var response = await PostAsync("/api/taxicompany/login", loginRequest);
            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));
        }

        [Test]
        public async Task Login_TaxiCompanyExists_LogsInAndReturnsTaxiCompanyWithCorrectEmail()
        {
            // Using client to create taxi company
            var request = getRegisterRequest();

            await CreateTaxiCompanyAccount();

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

            await CreateTaxiCompanyAccount();

            var loginRequest = getLoginRequest("test12@gmail.com", password);

            var response = await PostAsync("/api/taxicompany/login", loginRequest);

            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.BadRequest));
        }

        [Test]
        public async Task Login_TaxiCompanyExistsButWrongPasswordButRightFormat_ReturnsBadRequest()
        {
            // Using client to create taxi company
            var request = getRegisterRequest();

            await CreateTaxiCompanyAccount();

            var loginRequest = getLoginRequest("test12@gmail.com", "Qwer1!");

            var response = await PostAsync("/api/taxicompany/login", loginRequest);

            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.BadRequest));
        }
    }
}