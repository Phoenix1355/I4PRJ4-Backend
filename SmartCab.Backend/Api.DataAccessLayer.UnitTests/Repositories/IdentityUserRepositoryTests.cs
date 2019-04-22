using System.Threading.Tasks;
using Api.DataAccessLayer.Repositories;
using Api.DataAccessLayer.UnitTests.Factories;
using Api.DataAccessLayer.UnitTests.Fakes;
using Microsoft.AspNetCore.Identity;
using NSubstitute;
using NUnit.Framework;

namespace Api.DataAccessLayer.UnitTests.Repositories
{
    [TestFixture]
    public class IdentityUserRepositoryTests
    {
        #region Setup
        private IdentityUserRepository _uut;
        private InMemorySqlLiteContextFactory _factory;
        private FakeSignInManager _mockSignManager;
        private FakeUserManager _mockUserManager;

        [SetUp]
        public void SetUp()
        {
            _factory = new InMemorySqlLiteContextFactory();
            _mockSignManager = new FakeSignInManager();
            _mockUserManager = new FakeUserManager();
             _uut = new IdentityUserRepository(_mockUserManager,_mockSignManager);
        }

        [TearDown]
        public void TearDown()
        {
            _factory.Dispose();
        }

        #endregion

        #region SignIn

        [Test]
        public async Task SigninAsync_SigningIn_DelegatedExpectedValues()
        {
            var email = "Dummy Email";
            var password = "Dummy Password";
            var response = await _uut.SignInAsync(email, password);

            Assert.That(response, Is.EqualTo(SignInResult.Success));
        }


        [Test]
        public async Task SigninAsync_SigningIn_DelegatedExpectedFail()
        {

            var email = "Dummy Email";
            var password = "Dummy Password";

            _mockSignManager.PasswordSignInAsyncReturn = SignInResult.Failed;
            var response = await _uut.SignInAsync(email, password);

            Assert.That(response, Is.EqualTo(SignInResult.Failed));

            
        }
        #endregion

    }
}
