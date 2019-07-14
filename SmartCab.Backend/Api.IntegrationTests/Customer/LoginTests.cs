using System.Net;
using System.Threading.Tasks;
using Api.BusinessLogicLayer.Responses;
using NUnit.Framework;

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