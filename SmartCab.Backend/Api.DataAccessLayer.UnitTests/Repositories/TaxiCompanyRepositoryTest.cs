using System.Threading.Tasks;
using Api.DataAccessLayer.Interfaces;
using Api.DataAccessLayer.Models;
using Api.DataAccessLayer.UnitOfWork;
using Api.DataAccessLayer.UnitTests.Factories;
using CustomExceptions;
using NSubstitute;
using NUnit.Framework;

namespace Api.DataAccessLayer.UnitTests.Repositories
{
    [TestFixture]
    public class TaxiCompanyRepositoryTests
    {

        #region SetUp

        private IUnitOfWork _uut;
        private InMemorySqlLiteContextFactory _factory;

        [SetUp]
        public void SetUp()
        {
            _factory = new InMemorySqlLiteContextFactory();
            var identityRepository = Substitute.For<IIdentityUserRepository>();
            _uut = new UnitOfWork.UnitOfWork(_factory.CreateContext(), identityRepository);
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

        #region FindByEmailAsync

        [Test]
        public async Task FindByEmail_CustomerExist_ReturnsCustomer()
        {
            var taxiCompany = addTaxiCompanyToTestDatabase();

            var taxiCompanyDB = await _uut.TaxiCompanyRepository.FindByEmail(taxiCompany.Email);

            Assert.That(taxiCompany.Name, Is.EqualTo(taxiCompanyDB.Name));
        }

        [Test]
        public async Task FindByEmail_CustomerDoesNotExist_ThrowsException()
        {
            Assert.ThrowsAsync<UserIdInvalidException>(async () => await _uut.TaxiCompanyRepository.FindByEmail("ValidEmail@ButNoCustomerInDatabase.com"));
        }

        #endregion
    }
}