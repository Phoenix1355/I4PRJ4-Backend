using System.Threading.Tasks;
using Api.BusinessLogicLayer.Interfaces;
using Api.BusinessLogicLayer.Responses;
using Api.Controllers;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NSubstitute;
using NUnit.Framework;

namespace Api.UnitTests.Controllers
{
    [TestFixture]
    public class TaxiCompanyControllerTests
    {
        #region Setup

        private ITaxiCompanyService _taxiCompanyService;
        private TaxiCompanyController _taxiCompanyController;

        [SetUp]
        public void Setup()
        {
            _taxiCompanyService = Substitute.For<ITaxiCompanyService>();
            _taxiCompanyController = new TaxiCompanyController(_taxiCompanyService);
        }

        #endregion

        #region Login

        [Test]
        public async Task Login_Success_ReturnOkResponse()
        {
            _taxiCompanyService.LoginTaxiCompanyAsync(null).ReturnsForAnyArgs(new LoginResponse());

            var response = await _taxiCompanyController.Login(null) as ObjectResult;

            Assert.That(response.StatusCode, Is.EqualTo(StatusCodes.Status200OK));
        }

        #endregion
    }
}