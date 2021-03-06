﻿using System.Collections.Generic;
using System.Threading.Tasks;
using Api.BusinessLogicLayer.DataTransferObjects;
using Api.BusinessLogicLayer.Interfaces;
using Api.BusinessLogicLayer.Responses;
using Api.BusinessLogicLayer.Services;
using Api.DataAccessLayer.Interfaces;
using Api.DataAccessLayer.Models;
using Api.DataAccessLayer.Statuses;
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
        private IUnitOfWork _unitOfWork;
        private IPushNotificationFactory _pushNotificationFactory;
        private IPushNotificationService _pushNotificationService;

        [SetUp]
        public void Setup()
        {
            _mapper = Substitute.For<IMapper>();
            _orderRepository = Substitute.For<IOrderRepository>();
            _unitOfWork = Substitute.For<IUnitOfWork>();
            _pushNotificationFactory = Substitute.For<IPushNotificationFactory>();
            _pushNotificationService = Substitute.For<IPushNotificationService>();
            _orderService = new OrderService(_mapper, _unitOfWork, _pushNotificationFactory, _pushNotificationService);
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
            var order = new Order();
            var taxicompany = new TaxiCompany();
            _unitOfWork.OrderRepository.FindByIDAsync(null).ReturnsForAnyArgs(order);
            _unitOfWork.TaxiCompanyRepository.FindByIDAsync(null).ReturnsForAnyArgs(taxicompany);

            var expectedResponse = new AcceptOrderResponse {Order = orderDto};

            var response = await _orderService.AcceptOrderAsync(taxiCompanyId, orderId);

            Assert.That(response.Order, Is.EqualTo(expectedResponse.Order));
        }

        [Test]
        public async Task AcceptOrder_WhenCalled_SendsPushNotification()
        {
            // Arrange
            var taxiCompanyId = "someId";
            var orderId = 1;

            var taxiCompany = new TaxiCompany();
            var order = new Order();

            order.TaxiCompany = taxiCompany;
            
            var ride = new Ride();
            ride.StartDestination = new Address("", 1, "", 1);
            ride.EndDestination = new Address("", 1, "", 1);

            order.Rides = new List<Ride> {ride};

            _unitOfWork.OrderRepository.FindByIDAsync(null).ReturnsForAnyArgs(order);
            _unitOfWork.TaxiCompanyRepository.FindByIDAsync(null).ReturnsForAnyArgs(taxiCompany);

            // Act
            await _orderService.AcceptOrderAsync(taxiCompanyId, orderId);

            // Assert
            await _pushNotificationService.Received().SendAsync(Arg.Any<IPushNotification>());
        }


        [Test]
        public async Task GetOrderAsync_OrderExist_ReturnsOrderDetailedDto()
        {
            var order = new Order();
            _unitOfWork.OrderRepository.FindByIDAsync(null).ReturnsForAnyArgs(order);
            var orderDto = new OrderDetailedDto
            {
                Id = 1,
                Price = 200,
                Rides = null,
                Status = RideStatus.WaitingForAccept.ToString()
            };

            _mapper.Map<OrderDetailedDto>(null).ReturnsForAnyArgs(orderDto);

            var response = await _orderService.GetOrderAsync(1);
            Assert.That(orderDto,Is.EqualTo(response));
        }
    }
}