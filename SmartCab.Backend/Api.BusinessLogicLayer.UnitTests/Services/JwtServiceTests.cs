using System;
using System.Globalization;
using Api.BusinessLogicLayer.Services;
using Microsoft.Extensions.Configuration;
using NSubstitute;
using NUnit.Framework;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;

namespace Api.BusinessLogicLayer.UnitTests.Services
{
    [TestFixture]
    public class JwtServiceTests
    {
        private const string JwtSecret = "JwtKey";
        private const string JwtSecretValue = "SOME_SECRET_KEY_USED_FOR_TESTING";
        private const string JwtExpire = "JwtExpireSeconds";
        private const string JwtExpireValue = "1000";

        [TestCase("test1@gmail.com")]
        [TestCase("test2@yahoo.com")]
        [TestCase("HelloWorld")] //any string should be accepted but still
        public void GenerateJwtToken_WhenCalled_ReturnsATokenWithASpecifiedEmail(string email)
        {
            //Create uut
            var configuration = Substitute.For<IConfiguration>();
            configuration[JwtSecret].Returns(JwtSecretValue);
            configuration[JwtExpire].Returns(JwtExpireValue);
            var jwtService = new JwtService(configuration);

            //We are not testing the role and id, so lets hard code those
            var role = "Customer";
            var id = "someId";

            var serializedToken = jwtService.GenerateJwtToken(id, email, role);

            var token = new JwtSecurityToken(serializedToken);
            var emailInToken = token.Payload.Claims.FirstOrDefault(x => x.Type == ClaimTypes.Email)?.Value;
            Assert.That(email, Is.EqualTo(emailInToken));
        }

        [TestCase("1")]
        [TestCase("SomeId")]
        [TestCase("guid123-guid123-guid123-guid123-guid123")]
        public void GenerateJwtToken_WhenCalled_ReturnsATokenWithASpecifiedUserId(string id)
        {
            //Create uut
            var configuration = Substitute.For<IConfiguration>();
            configuration[JwtSecret].Returns(JwtSecretValue);
            configuration[JwtExpire].Returns(JwtExpireValue);
            var jwtService = new JwtService(configuration);

            //We are not testing the role and email, so lets hard code those
            var role = "Customer";
            var email = "test@gmail.com";

            var serializedToken = jwtService.GenerateJwtToken(id, email, role);

            var token = new JwtSecurityToken(serializedToken);
            var emailInToken = token.Payload.Claims.FirstOrDefault(x => x.Type == ClaimTypes.Email)?.Value;
            Assert.That(email, Is.EqualTo(emailInToken));
        }

        [TestCase("Customer")]
        [TestCase("TaxiCompany")]
        [TestCase("President")]
        public void GenerateJwtToken_WhenCalled_ReturnsATokenWithASpecifiedRole(string role)
        {
            //Create uut
            var configuration = Substitute.For<IConfiguration>();
            configuration[JwtSecret].Returns(JwtSecretValue);
            configuration[JwtExpire].Returns(JwtExpireValue);
            var jwtService = new JwtService(configuration);

            //We are not testing the email and id, so lets hard code those
            var email = "test@gmail.com";
            var id = "someId";

            var serializedToken = jwtService.GenerateJwtToken(id, email, role);

            var token = new JwtSecurityToken(serializedToken);
            var roleInToken = token.Payload.Claims.FirstOrDefault(x => x.Type == ClaimTypes.Role)?.Value;
            Assert.That(role, Is.EqualTo(roleInToken));
        }

        [TestCase(1)]
        [TestCase(10)]
        [TestCase(100)]
        [TestCase(1000)]
        [TestCase(10000000)]
        public void GenerateJwtToken_WhenCalled_ReturnsATokenWithTheSpecifiedExpirationStoredAsAClaim(int lengthInSeconds)
        {
            //Create uut
            var configuration = Substitute.For<IConfiguration>();
            configuration[JwtSecret].Returns(JwtSecretValue);
            configuration[JwtExpire].Returns(lengthInSeconds.ToString());
            var jwtService = new JwtService(configuration);

            //We are not testing the email, role or id, so lets hard code those
            var role = "Customer";
            var email = "test@gmail.com";
            var id = "someId";

            //Create a datetime object and remove milliseconds. If milliseconds are not removed, all tests will fails
            var expectedExpirationDateTime = DateTime.Now.AddMinutes(lengthInSeconds);
            expectedExpirationDateTime = DateTime.ParseExact(expectedExpirationDateTime.ToString("yyyy-MM-dd HH:mm:ss"), "yyyy-MM-dd HH:mm:ss", null);

            var serializedToken = jwtService.GenerateJwtToken(id, email, role);

            var token = new JwtSecurityToken(serializedToken);
            var expirationDateTimeInToken = token.Payload.Claims.FirstOrDefault(x => x.Type == ClaimTypes.Expiration)?.Value;
            Assert.That(expectedExpirationDateTime, Is.EqualTo(DateTime.Parse(expirationDateTimeInToken, CultureInfo.InvariantCulture)));
        }

        [TestCase(1)]
        [TestCase(10)]
        [TestCase(100)]
        [TestCase(1000)]
        [TestCase(1000000)]
        public void GenerateJwtToken_WhenCalled_ReturnsATokenWithTheSpecifiedExpirationStoredOutsideClaims(int lengthInMinutes)
        {
            //Create uut
            var configuration = Substitute.For<IConfiguration>();
            configuration[JwtSecret].Returns(JwtSecretValue);
            configuration[JwtExpire].Returns(lengthInMinutes.ToString());
            var jwtService = new JwtService(configuration);

            //We are not testing the email, role or id, so lets hard code those
            var role = "Customer";
            var email = "test@gmail.com";
            var id = "someId";

            var expectedExpirationDateTime = DateTime.Now.AddMinutes(lengthInMinutes);
            expectedExpirationDateTime = DateTime.ParseExact(expectedExpirationDateTime.ToString("yyyy-MM-dd HH:mm:ss"), "yyyy-MM-dd HH:mm:ss", null);

            var serializedToken = jwtService.GenerateJwtToken(id, email, role);

            var token = new JwtSecurityToken(serializedToken);
            var expirationDateTimeInToken = DateTime.UnixEpoch.AddSeconds(token.Payload.Exp.Value).ToLocalTime(); //To local time to avoid errors because of winter/summer time

            Assert.That(expectedExpirationDateTime, Is.EqualTo(expirationDateTimeInToken));
        }
    }
}
