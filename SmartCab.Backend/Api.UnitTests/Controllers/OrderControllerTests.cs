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

        [Test]
        public async Task Open_Success_ReturnsOkResponse()
        {
            _orderService.GetOpenOrdersAsync().Returns(new OpenOrdersResponse());

            var response = await _orderController.Open(null) as ObjectResult;

            Assert.That(response.StatusCode, Is.EqualTo(StatusCodes.Status200OK));
        }
    }
}