using System.Collections.Generic;
using System.Threading.Tasks;
using Api.BusinessLogicLayer.DataTransferObjects;
using Api.BusinessLogicLayer.Responses;
using Api.BusinessLogicLayer.Services;
using Api.DataAccessLayer.Interfaces;
using Api.DataAccessLayer.UnitOfWork;
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
        private IUoW _unitOfWork;

        [SetUp]
        public void Setup()
        {
            _mapper = Substitute.For<IMapper>();
            _orderRepository = Substitute.For<IOrderRepository>();
            _unitOfWork = Substitute.For<IUoW>();
            _orderService = new OrderService(_mapper, _unitOfWork);
        }

        [Test]
        public async Task GetOpenOrdersAsync_WhenCalled_ReturnsExpectedOpenOrdersResponse()
        {
            var orderDtos = new List<OrderDto>
            {
                new OrderDto {Id = 1, Price = 200, Rides = null, Status = RideStatus.WaitingForAccept.ToString()},
                new OrderDto {Id = 2, Price = 400, Rides = null, Status = RideStatus.WaitingForAccept.ToString()}
            };
            _mapper.Map<List<OrderDto>>(null).ReturnsForAnyArgs(orderDtos);
            var expectedResponse = new OpenOrdersResponse{Orders = orderDtos};

            var response = await _orderService.GetOpenOrdersAsync();

            Assert.That(response.Orders.Count, Is.EqualTo(expectedResponse.Orders.Count));
        }

        [Test]
        public async Task AcceptOrder_WhenCalled_ReturnsExpectedAcceptOrderResponse()
        {
            var taxiCompanyId = "someId";
            var orderId = 1;

            var orderDto = new OrderDto
            {
                Id = orderId,
                Price = 200,
                Rides = null,
                Status = RideStatus.WaitingForAccept.ToString()
            };
            _mapper.Map<OrderDto>(null).ReturnsForAnyArgs(orderDto);
            var expectedResponse = new AcceptOrderResponse {Order = orderDto};

            var response = await _orderService.AcceptOrderAsync(taxiCompanyId, orderId);

            Assert.That(response.Order, Is.EqualTo(expectedResponse.Order));
        }
    }
}