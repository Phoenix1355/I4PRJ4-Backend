using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Api.DataAccessLayer.Models;
using Api.DataAccessLayer.Repositories;
using Api.DataAccessLayer.Statuses;
using Api.DataAccessLayer.UnitTests.Factories;
using Api.DataAccessLayer.UnitTests.Fakes;
using CustomExceptions;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Data.Sqlite;
using NUnit.Framework;
using Microsoft.EntityFrameworkCore;
using NSubstitute;

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

        #endregion

    }
}
