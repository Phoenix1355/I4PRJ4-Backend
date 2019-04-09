using System.Collections.Generic;
using System.Threading.Tasks;
using Api.BusinessLogicLayer.DataTransferObjects;
using Api.BusinessLogicLayer.Responses;
using Api.BusinessLogicLayer.Services;
using Api.DataAccessLayer.Interfaces;
using AutoMapper;
using NSubstitute;
using NUnit.Framework;

namespace Api.BusinessLogicLayer.UnitTests.Services
{
    [TestFixture]
    public class OrderServiceTests
    {
        private IMapper _mapper;
        private IOrderRepository _orderRepository;
        private OrderService _orderService;

        [SetUp]
        public void Setup()
        {
            _mapper = Substitute.For<IMapper>();
            _orderRepository = Substitute.For<IOrderRepository>();
            _orderService = new OrderService(_orderRepository, _mapper);
        }

        [Test]
        public async Task GetOpenOrdersAsync_WhenCalled_ReturnsExpectedOpenOrdersResponse()
        {
            var orderDtos = new List<OrderDto>
            {
                new OrderDto {Id = 1, Price = 200, Rides = null, Status = 0},
                new OrderDto {Id = 2, Price = 400, Rides = null, Status = 0}
            };
            _mapper.Map<List<OrderDto>>(null).ReturnsForAnyArgs(orderDtos);
            var expectedResponse = new OpenOrdersResponse{Orders = orderDtos};

            var response = await _orderService.GetOpenOrdersAsync();

            Assert.That(response.Orders.Count, Is.EqualTo(expectedResponse.Orders.Count));
        }
    }
}