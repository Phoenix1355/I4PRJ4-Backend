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
            var request = getRegisterRequest();
            await PostAsync("/api/customer/register", request);

            var loginRequest = getLoginRequest();
            await PostAsync("/api/customer/login", loginRequest);

            var editRequest = getEditRequest();
            var response = await PostAsync("/api/customer/edit", editRequest);

            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));
        }
        
    }
}