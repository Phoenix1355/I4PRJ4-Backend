using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Api.DataAccessLayer.Interfaces;
using Api.DataAccessLayer.Models;
using Api.DataAccessLayer.Repositories;
using Api.DataAccessLayer.Statuses;
using Api.DataAccessLayer.UnitOfWork;
using Api.DataAccessLayer.UnitTests.Factories;
using CustomExceptions;
using NSubstitute;
using NUnit.Framework;

namespace Api.DataAccessLayer.UnitTests.Repositories
{
    class RideRepositoryTests
    {

        #region Setup

        private IUoW _uut;
        private InMemorySqlLiteContextFactory _factory;

        [SetUp]
        public void SetUp()
        {
            _factory = new InMemorySqlLiteContextFactory();
            var identityRepository = Substitute.For<IIdentityUserRepository>();
            _uut = new UoW(_factory.CreateContext(), identityRepository);
        }

        [TearDown]
        public void TearDown()
        {
            _factory.Dispose();
        }

        #endregion
    }
}
