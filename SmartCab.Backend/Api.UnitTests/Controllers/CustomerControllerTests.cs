using System;
using System.Security.Claims;
using System.Threading.Tasks;
using Api.BusinessLogicLayer;
using Api.BusinessLogicLayer.Interfaces;
using Api.BusinessLogicLayer.Requests;
using Api.BusinessLogicLayer.Responses;
using Api.Controllers;
using CustomExceptions;
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

        #region Deposit

        [Test]
        public async Task Deposits_Success_ReturnsOkResponse()
        {
            //Set claims
            _customerController.ControllerContext = new ControllerContext
            {
                HttpContext = new DefaultHttpContext
                {
                    User = new ClaimsPrincipal(new ClaimsIdentity(new Claim[]
                    {
                        new Claim(Constants.UserIdClaim, "SomeCustomerId") 
                    }))
                }
            };
            //Act and Assert
            var response = await _customerController.Deposit(null, null) as ObjectResult;

            Assert.That(response.StatusCode, Is.EqualTo(StatusCodes.Status200OK));
        }

        [Test]
        public async Task Deposits_CustomerIdEmpty_ThrowsExpectedException()
        {
            //Set claims
            _customerController.ControllerContext = new ControllerContext
            {
                HttpContext = new DefaultHttpContext
                {
                    User = new ClaimsPrincipal(new ClaimsIdentity(new Claim[]
                    {
                        new Claim(Constants.UserIdClaim, "")
                    }))
                }
            };
            //Act and Assertsult;

            Assert.ThrowsAsync<UserIdInvalidException>(async () => await _customerController.Deposit(null,null));
        }
        #endregion
    }
}