using System.Security.Claims;
using System.Threading.Tasks;
using Api.BusinessLogicLayer;
using Api.BusinessLogicLayer.Interfaces;
using Api.BusinessLogicLayer.Responses;
using Api.Controllers;
using CustomExceptions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NSubstitute;
using NUnit.Framework;

namespace Api.UnitTests.Controllers
{
    [TestFixture]
    public class OrderControllerTests
    {
        #region Setup and fields

        private IOrderService _orderService;
        private OrderController _orderController;

        [SetUp]
        public void Setup()
        {
            _orderService = Substitute.For<IOrderService>();
            _orderController = new OrderController(_orderService);
        }

        #endregion

        #region Open

        [Test]
        public async Task Open_Success_ReturnsOkResponse()
        {
            _orderService.GetOpenOrdersAsync().Returns(new OpenOrdersResponse());

            var response = await _orderController.Open(null) as ObjectResult;

            Assert.That(response.StatusCode, Is.EqualTo(StatusCodes.Status200OK));
        }

        #endregion

        #region Accept

        [Test]
        public async Task Accept_Success_ReturnsOkResponse()
        {
            //Set claims
            _orderController.ControllerContext = new ControllerContext
            {
                HttpContext = new DefaultHttpContext
                {
                    User = new ClaimsPrincipal(new ClaimsIdentity(new Claim[]
                    {
                        new Claim(Constants.UserIdClaim, "SomeTaxiCompanyId")
                    }))
                }
            };

            //Act and assert
            var response = await _orderController.Accept(null, 1) as ObjectResult;

            Assert.That(response.StatusCode, Is.EqualTo(StatusCodes.Status200OK));
        }

        [Test]
        public void Accept_TaxiCompanyIdEmpty_ThrowsUserIdInvalidException()
        {
            //Set claims
            _orderController.ControllerContext = new ControllerContext
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
            Assert.ThrowsAsync<UserIdInvalidException>(async () => await _orderController.Accept(null, 1));
        }

        #endregion
    }
}