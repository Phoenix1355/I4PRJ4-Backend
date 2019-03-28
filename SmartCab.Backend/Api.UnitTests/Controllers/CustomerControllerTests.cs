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

        [Test]
        public async Task Register_ValidationFails_ReturnsBadRequestResponse()
        {
            _customerService
                .When(x => x.AddCustomerAsync(null))
                .Do(x => throw new ArgumentException()); //Reaches the bad request clause which is errors we throw manually

            var response = await _customerController.Register(null) as ObjectResult;

            Assert.That(response.StatusCode, Is.EqualTo(StatusCodes.Status400BadRequest));
        }

        [Test]
        public async Task Register_UnknownErrorOccurs_ReturnsInternalErrorResponse()
        {
            _customerService
                .When(x => x.AddCustomerAsync(null))
                .Do(x => throw new Exception()); //Reaches the "catch all" clause which is the unknown error clause

            var response = await _customerController.Register(null) as ObjectResult;

            Assert.That(response.StatusCode, Is.EqualTo(StatusCodes.Status500InternalServerError));
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

        [Test]
        public async Task Login_ValidationFails_ReturnsBadRequestResponse()
        {
            _customerService
                .When(x => x.LoginCustomerAsync(null))
                .Do(x => throw new ArgumentException()); //Reaches the bad request clause which is errors we throw manually

            var response = await _customerController.Login(null) as ObjectResult;

            Assert.That(response.StatusCode, Is.EqualTo(StatusCodes.Status400BadRequest));
        }

        [Test]
        public async Task Login_UnknownErrorOccurs_ReturnsInternalErrorResponse()
        {
            _customerService
                .When(x => x.LoginCustomerAsync(null))
                .Do(x => throw new Exception()); //Reaches the "catch all" clause which is the unknown error clause

            var response = await _customerController.Login(null) as ObjectResult;

            Assert.That(response.StatusCode, Is.EqualTo(StatusCodes.Status500InternalServerError));
        }

        #endregion
    }
}