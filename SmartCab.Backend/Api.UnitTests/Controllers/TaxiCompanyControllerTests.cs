using System;
using System.Threading.Tasks;
using Api.BusinessLogicLayer.Interfaces;
using Api.BusinessLogicLayer.Requests;
using Api.BusinessLogicLayer.Responses;
using Api.Controllers;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NSubstitute;
using NSubstitute.Extensions;
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
    }
}