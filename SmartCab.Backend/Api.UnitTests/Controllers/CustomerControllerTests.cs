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
        private ICustomerService _customerService;
        private CustomerController _customerController;

        [SetUp]
        public void Setup()
        {
            _customerService = Substitute.For<ICustomerService>();
            _customerController = new CustomerController(_customerService);
        }

        [Test]
        public async Task Register_Success_ReturnOkResponse()
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
    }
}