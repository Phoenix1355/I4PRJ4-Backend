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
    public class CustomerControllerTests
    {
        #region Setup and fields

        private ICustomerService _customerService;
        private CustomerController _customerController;

        [SetUp]
        public void Setup()
        {
            _customerService = Substitute.For<ICustomerService>();
            _customerController = new CustomerController(_customerService);
        }

        #endregion

        #region Register

        [Test]
        public async Task Register_Success_ReturnsOkResponse()
        {
            _customerService.AddCustomerAsync(null).ReturnsForAnyArgs(new RegisterResponse());

            var response = await _customerController.Register(null) as ObjectResult;

            Assert.That(response.StatusCode, Is.EqualTo(StatusCodes.Status200OK));
        }

        #endregion

        #region Login

        [Test]
        public async Task Login_Success_ReturnsOkResponse()
        {
            _customerService.LoginCustomerAsync(null).ReturnsForAnyArgs(new LoginResponse());

            var response = await _customerController.Login(null) as ObjectResult;

            Assert.That(response.StatusCode, Is.EqualTo(StatusCodes.Status200OK));
        }

        #endregion
    }
}