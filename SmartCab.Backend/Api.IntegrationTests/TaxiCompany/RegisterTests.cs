using System.Linq;
using System.Net;
using System.Threading.Tasks;
using NUnit.Framework;

namespace Api.IntegrationTests.TaxiCompany
{
    [TestFixture]
    public class RegisterTests : IntegrationSetup
    {
        [Test]
        public async Task Register_ValidRequest_StatusOk()
        {
            var request = getRegisterRequest();

            var response = await PostAsync("/api/taxicompany/register", request);

            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));
        }

        [Test]
        public async Task Register_ValidRequest_ElementExistInDatabase()
        {
            var request = getRegisterRequest();

            var response = await PostAsync("/api/taxicompany/register", request);

            using (var context = _factory.CreateContext())
            {
                var entry = context.TaxiCompanies.Where(x => x.Email == request.Email);
                Assert.That(entry.Count, Is.EqualTo(1));
            }
        }

        [Test]
        public async Task Register_RequestTwice_FirstOk()
        {
            var request = getRegisterRequest();

            var responseFirstRequest = await PostAsync("/api/taxicompany/register", request);

            var responseSecondRequest = await PostAsync("/api/taxicompany/register", request);

            Assert.That(responseFirstRequest.StatusCode, Is.EqualTo(HttpStatusCode.OK));
        }

        [Test]
        public async Task Register_RequestTwice_SecondBadRequest()
        {
            var request = getRegisterRequest();

            var responseFirstRequest = await PostAsync("/api/taxicompany/register", request);

            var responseSecondRequest = await PostAsync("/api/taxicompany/register", request);

            Assert.That(responseSecondRequest.StatusCode, Is.EqualTo(HttpStatusCode.BadRequest));
        }

        [TestCase("a.mail.com")]
        [TestCase("plaintext")]
        [TestCase("#@¤%&##$#£.com")]
        [TestCase("Nikolaj Molzen <test@domain.com>")]
        public async Task Register_InvalidRequestEmail_GetsBadRequest(string email)
        {
            var request = getRegisterRequest(email);

            var response = await PostAsync("/api/taxicompany/register", request);

            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.BadRequest));
        }

        [TestCase("", "")]
        [TestCase("111111", "111111")]
        [TestCase("111111!", "111111!")]
        [TestCase("12345678", "12345678")]
        [TestCase("123456aA", "123456aA")]
        [TestCase("999999aA!", "999999aA#")]
        [TestCase("999995599aA!", "999995599aA#")]
        public async Task Register_InvalidRequestPassword_GetsBadRequest(string password, string passwordRepeated)
        {
            var request = getRegisterRequest("test@domain.com", "12345678", password, passwordRepeated);

            var response = await PostAsync("/api/taxicompany/register", request);

            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.BadRequest));
        }
    }
}