using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Api.BusinessLogicLayer.DataTransferObjects;
using Api.BusinessLogicLayer.Interfaces;
using Api.BusinessLogicLayer.Responses;
using Api.DataAccessLayer.Interfaces;
using Api.DataAccessLayer.Models;
using Api.DataAccessLayer.Statuses;
using Api.DataAccessLayer.UnitOfWork;
using AutoMapper;

namespace Api.BusinessLogicLayer.Services
{
    public class OrderService : IOrderService
    {
        private readonly IOrderRepository _orderRepository;
        private readonly IMapper _mapper;
        private readonly IUoW _unitOfWork;

        public OrderService(IOrderRepository orderRepository, IMapper mapper, IUoW unitOfWork)
        {
            _orderRepository = orderRepository;
            _mapper = mapper;
            _unitOfWork = unitOfWork;
        }

        public async Task<OpenOrdersResponse> GetOpenOrdersAsync()
        {
            var openOrders = _unitOfWork.OrderRepository.Find(order => order.Status == OrderStatus.WaitingForAccept);
            var openOrderDtos = _mapper.Map<List<OrderDto>>(openOrders);
            var response = new OpenOrdersResponse {Orders = openOrderDtos};
            return response;
        }
    }
}