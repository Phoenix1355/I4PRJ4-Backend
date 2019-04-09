using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Api.BusinessLogicLayer.Responses;
using NUnit.Framework;

namespace Api.IntegrationTests.TaxiCompany
{
    [TestFixture]
    public class LoginTests : IntegrationSetup
    {
        [Test]
        public async Task Login_TaxiCompanyExists_LogsInAndReturnsTaxiCompany()
        {

            // Using client to create taxi company
            var request = getRegisterRequest();
            await PostAsync("/api/customer/register", request);


            using (var context = _factory.CreateContext())
            {
                var customer = context.Customers.First();
                var customerId = customer.Id;

                var role = context.Roles.Where(x => x.Name == "TaxiCompany").First();
                var roleId = role.Id;

                var customerRole = context.UserRoles.Where(x => x.UserId == customerId).First();
                context.UserRoles.Remove(customerRole);
                context.SaveChanges();

                customerRole.RoleId = roleId;
                context.UserRoles.Add(customerRole);
                context.SaveChanges();
            }

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

            var responseObject = GetObject<LoginResponse>(response);
            
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

            var loginRequest = getLoginRequest("test12@gmail.com", password);

            var response = await PostAsync("/api/taxicompany/login", loginRequest);

            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.BadRequest));
        }

        [Test]
        public async Task Login_TaxiCompanyExistsButWrongPasswordButRightFormat_ReturnsBadRequest()
        {
            // Using client to create taxi company
            var request = getRegisterRequest();
            await PostAsync("/api/taxicompany/register", request);

            var loginRequest = getLoginRequest("test12@gmail.com", "Qwer1!");

            var response = await PostAsync("/api/taxicompany/login", loginRequest);

            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.BadRequest));
        }
    }
}