using System.Net;
using System.Threading.Tasks;
using Api.BusinessLogicLayer.Responses;
using NUnit.Framework;

namespace Api.IntegrationTests.Customer
{
    [TestFixture]
    public class EditTests : IntegrationSetup
    {
        [Test]
        public async Task Edit_UserExists_EditsUserAndReturnsCustomer()
        {
            await LoginOnCustomerAccount();
            
            var editRequest = getEditRequest();
            var response = await PutAsync("/api/customer/edit", editRequest);

            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));
        }

        [Test]
        public async Task Edit_CustomerNotLoggedIn_ReturnsUnAuthorized()
        {
            var editRequest = getEditRequest();

            var response = await PutAsync("api/customer/edit", editRequest);
            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.Unauthorized));
        }

        [Test]
        public async Task Edit_PasswordIsChanged_CanLogInWithNewCredentials()
        {
            await LoginOnCustomerAccount();

            var editRequest = getEditRequest(changePassword: true, name: "TestUser", email: "test12@gmail.com",
                phoneNumber: "12345678");

            await PutAsync("api/customer/edit", editRequest);

            var loginRequest = getLoginRequest(editRequest.Email, editRequest.Password);

            var response = await PostAsync("api/customer/login", loginRequest);

            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));
        }
    }
}