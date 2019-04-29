using System;
using System.Linq;
using System.Threading.Tasks;
using Api.DataAccessLayer.Interfaces;
using Api.DataAccessLayer.Models;
using Api.DataAccessLayer.Repositories;
using Api.DataAccessLayer.UnitOfWork;
using Api.DataAccessLayer.UnitTests.Factories;
using Api.DataAccessLayer.UnitTests.Fakes;
using Microsoft.AspNetCore.Identity;
using CustomExceptions;
using NSubstitute;
using NUnit.Framework;

namespace Api.DataAccessLayer.UnitTests.Repositories
{
    [TestFixture]
    public class TaxiCompanyRepositoryTests
    {

        #region SetUp

        private IUoW _uut;
        private InMemorySqlLiteContextFactory _factory;

        [SetUp]
        public void SetUp()
        {
            _factory = new InMemorySqlLiteContextFactory();
            var identityRepository = Substitute.For<IIdentityUserRepository>();
            _uut = new UoW(_factory.CreateContext(), identityRepository);
        }
        #endregion

        #region Helper Methods

        private TaxiCompany addTaxiCompanyToTestDatabase(int balance = 0)
        {
            TaxiCompany taxiCompany = new TaxiCompany
            {
                Email = "valid@email.com",
                Name = "Name",
                PhoneNumber = "12345678"
            };

            using (var context = _factory.CreateContext())
            {
                context.TaxiCompanies.Add(taxiCompany);
                context.SaveChanges();
            }

            return taxiCompany;
        }

        #endregion

        #region FindByEmail

        [Test]
        public void FindByEmail_CustomerExist_ReturnsCustomer()
        {
            var customer = addTaxiCompanyToTestDatabase();

            Assert.That(customer.Name, Is.EqualTo(_uut.TaxiCompanyRepository.FindByEmail(customer.Email).Name));
        }

        [Test]
        public void FindByEmail_CustomerDoesNotExist_ThrowsException()
        {
            Assert.Throws<UserIdInvalidException>(() => _uut.TaxiCompanyRepository.FindByEmail("ValidEmail@ButNoCustomerInDatabase.com"));
        }

        #endregion
    }
}