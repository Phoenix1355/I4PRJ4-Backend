using System.Threading.Tasks;
using Api.DataAccessLayer.Repositories;
using Api.DataAccessLayer.UnitTests.Factories;
using Api.DataAccessLayer.UnitTests.Fakes;
using CustomExceptions;
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
        public async Task SigninAsync_SigningIn_ThrowsExceptionWhenFailed()
        {

            var email = "Dummy Email";
            var password = "Dummy Password";

            _mockSignManager.PasswordSignInAsyncReturn = SignInResult.Failed;
            Assert.ThrowsAsync<IdentityException>(async ()=>await _uut.SignInAsync(email,password));
        }

        #endregion

        #region AddToRoleAsync

        [Test]
        public async Task AddToRoleAsync_AddingToRole_DelegatedExpectedValues()
        {
            var response = await _uut.AddToRoleAsync(null, null);
            Assert.That(response, Is.EqualTo(IdentityResult.Success));
        }


        [Test]
        public async Task AddToRoleAsync_AddingToRole_ThrowsExceptionWhenFailed()
        {
            _mockUserManager.AddToRoleAsyncReturn = IdentityResult.Failed(new IdentityError());
            Assert.ThrowsAsync<IdentityException>(async () => await _uut.AddToRoleAsync(null, null));
        }

        #endregion

        #region AddIdentityUserAsync

        [Test]
        public async Task AddIdentityUserAsync_AddUser_DelegatedExpectedValues()
        {
            var response = await _uut.AddIdentityUserAsync(null, null);
            Assert.That(response, Is.EqualTo(IdentityResult.Success));
        }


        [Test]
        public async Task AddIdentityUserAsyncc_AddingUser_ThrowsExceptionWhenFailed()
        {
            _mockUserManager.CreateAsyncReturn = IdentityResult.Failed(new IdentityError());
            Assert.ThrowsAsync<IdentityException>(async () => await _uut.AddIdentityUserAsync(null, null));
        }

        [Test]
        public async Task TransactionWrapper_FunctionExecuted_IncrementExpectedVarByOne()
        {
            int i = 0;

            await _uut.TransactionWrapper(async() => { i++; });

            Assert.That(i,Is.EqualTo(1));
        }

        #endregion

        #region ChangeEmailAsync
        
        [Test]
        public async Task ChangeEmail_EmailIsNowChangedWithCorrectValue()
        {

            var response = await _mockUserManager.ChangeEmailAsync(null, null, null);
            Assert.That(response, Is.EqualTo(IdentityResult.Success));
        }

        [Test]
        public async Task ChangeEmailAsync_ChangingEmail_ThrowsExceptionWhenFailed()
        {
            _mockUserManager.ChangeEmailAsyncReturn = IdentityResult.Failed(new IdentityError());
            Assert.ThrowsAsync<IdentityException>(async () => await _mockUserManager.ChangeEmailAsync(null,null, null));
        }


        #endregion

    }
}
