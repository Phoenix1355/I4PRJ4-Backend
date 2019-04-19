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
    }
}