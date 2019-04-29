using System;
using System.Linq;
using System.Threading.Tasks;
using Api.DataAccessLayer.Models;
using Api.DataAccessLayer.Repositories;
using Api.DataAccessLayer.UnitOfWork;
using Api.DataAccessLayer.UnitTests.Factories;
using Api.DataAccessLayer.UnitTests.Fakes;
using Microsoft.AspNetCore.Identity;
using CustomExceptions;
using NUnit.Framework;

namespace Api.DataAccessLayer.UnitTests.Repositories
{
    [TestFixture]
    public class TaxiCompanyRepositoryTests
    {
        private TaxiCompanyRepository _uut;
        private InMemorySqlLiteContextFactory _factory;

        #region SetUp

        [SetUp]
        public void SetUp()
        {
            _factory = new InMemorySqlLiteContextFactory();
            _uut = new TaxiCompanyRepository(_factory.CreateContext());
        }
        #endregion
        /*
        #region GetTaxiCompanyAsync
        // Testing Taxi Company Name
        [Test]
        public void GetTaxiCompanyAsync_TaxiCompanyNameInDatabase_ReturnsTaxiCompanyName()
        {
            TaxiCompany taxicompanyAddedToDatabase = new TaxiCompany
            {
                Email = "valid@email.com",
                Name = "DanTaxi",
                PhoneNumber = "45612378",
            };

            using (var content = _factory.CreateContext())
            {
                content.TaxiCompanies.Add(taxicompanyAddedToDatabase);
                content.SaveChanges();
            }


            var taxicompanyReturned = _uut.GetTaxiCompanyAsync(taxicompanyAddedToDatabase.Email).Result;
            Assert.That(taxicompanyReturned.Name, Is.EqualTo("DanTaxi"));
        }

        // Testing Taxi Company Email
        [Test]
        public void GetTaxiCompanyAsync_TaxiCompanyEmail_ReturnsTaxiCompanyEmail()
        {
            TaxiCompany taxicompanyAddedToDatabase = new TaxiCompany
            {
                Email = "valid@email.com",
                Name = "TaxiCompanyName",
                PhoneNumber = "45612378",
            };

            using (var content = _factory.CreateContext())
            {
                content.TaxiCompanies.Add(taxicompanyAddedToDatabase);
                content.SaveChanges();
            }


            var taxicompanyReturned = _uut.GetTaxiCompanyAsync(taxicompanyAddedToDatabase.Email).Result;
            Assert.That(taxicompanyReturned.Email, Is.EqualTo("valid@email.com"));
        }

        //Testing Taxi Company Number
        [Test]
        public void GetTaxiCompanyAsync_TaxiCompanyPhoneNumber_ReturnsTaxiCompanyPhoneNumber()
        {
            TaxiCompany taxicompanyAddedToDatabase = new TaxiCompany
            {
                Email = "valid@email.com",
                Name = "TaxiCompanyName",
                PhoneNumber = "45612378",
            };

            using (var content = _factory.CreateContext())
            {
                content.TaxiCompanies.Add(taxicompanyAddedToDatabase);
                content.SaveChanges();
            }


            var taxicompanyReturned = _uut.GetTaxiCompanyAsync(taxicompanyAddedToDatabase.Email).Result;
            Assert.That(taxicompanyReturned.PhoneNumber, Is.EqualTo("45612378"));
        }

        [Test]
        public void GetTaxiCompanyAsync_NoTaxiCompany_ThrowsExceptionWithMessage()
        {
            try
            {
                _uut.GetTaxiCompanyAsync("DanTaxi@mail.com"); 

            }
            catch(ArgumentException e)
            {
                Assert.That(e.Message, Is.EqualTo("TaxiCompany does not exist."));
            }
        }

        [Test]
        public void GetTaxiCompanyAsync_NoTaxiCompany_ThrowsNotFound()
        {
            Assert.ThrowsAsync<UserIdInvalidException>(() => _uut.GetTaxiCompanyAsync("DanTaxi@mail.com"));
        }
        #endregion

        #region TearDown
        [TearDown]
        public void TearDown()
        {
            _factory.Dispose();
        }
        #endregion

        #region AddTaxiCompanyAsync
        // With phone
        [Test]
        public async Task AddTaxiCompanyAsync_TaxicompanyValid_TaxiCompanyExistsInDatabase()
        {
            TaxiCompany taxicompany = new TaxiCompany
            {
                Name = "CompanyName",
                PhoneNumber = "66664444",
            };
            //As function now relies on Identity framework, insert it manually. 
            using (var context = _factory.CreateContext())
            {
                context.TaxiCompanies.Add(taxicompany);
                context.SaveChanges();
            }


            await _uut.AddTaxiCompanyAsync(taxicompany, "Qwerrr111!");



            using (var context = _factory.CreateContext())
            {
                var taxicompanyFromDatabase = context.TaxiCompanies.FirstOrDefault(taxicompanyFromDB => taxicompany.Id.Equals(taxicompanyFromDB.Id));

                Assert.That(taxicompanyFromDatabase.Name, Is.EqualTo("CompanyName"));
            }
        }

        // No phone
        [Test]
        public async Task AddTaxiCompanyAsync_TaxicompanyValidNoPhone_TaxiCompanyExistsInDatabase()
        {
            TaxiCompany taxicompany = new TaxiCompany
            {
                Name = "TaxiCompanyName",
            };
            //As function now relies on Identity framework, insert it manually. 
            using (var context = _factory.CreateContext())
            {
                context.TaxiCompanies.Add(taxicompany);
                context.SaveChanges();
            }


            await _uut.AddTaxiCompanyAsync(taxicompany, "Qwerrr111!");



            using (var context = _factory.CreateContext())
            {
                var taxicompanyFromDatabase = context.TaxiCompanies.FirstOrDefault(taxicompanyFromDB => taxicompany.Id.Equals(taxicompanyFromDB.Id));

                Assert.That(taxicompanyFromDatabase.Name, Is.EqualTo("TaxiCompanyName"));
            }
        }

        // With phone number
        [Test]
        public void AddTaxiCompanyAsync_TaxiCompanyInvalid_TaxiCompanyAlreadyExistsInDatabase()
        {
            TaxiCompany taxicompanyToAddToDatabase = new TaxiCompany
            {
                Name = "CompanyName",
                PhoneNumber = "11223344",
            };

            _mockUserManager.AddToRoleAsyncReturn = IdentityResult.Failed();

            using (var context = _factory.CreateContext())
            {
                context.TaxiCompanies.Add(taxicompanyToAddToDatabase);
                context.SaveChanges();
            }


            Assert.ThrowsAsync<IdentityException>(async () => await _uut.AddTaxiCompanyAsync(taxicompanyToAddToDatabase, "Qwerrrr111111!"));
        }

        // No phone number
        [Test]
        public void AddTaxiCompanyAsync_TaxiCompanyInvalidWithNoPhone_TaxiCompanyAlreadyExistsInDatabase()
        {
            TaxiCompany taxicompanyToAddToDatabase = new TaxiCompany
            {
                Name = "CompanyName",
            };

            _mockUserManager.AddToRoleAsyncReturn = IdentityResult.Failed();

            using (var context = _factory.CreateContext())
            {
                context.TaxiCompanies.Add(taxicompanyToAddToDatabase);
                context.SaveChanges();
            }


            Assert.ThrowsAsync<IdentityException>(async () => await _uut.AddTaxiCompanyAsync(taxicompanyToAddToDatabase, "YaaaAssSS4512!"));
        }

        #endregion
    */
    }
}