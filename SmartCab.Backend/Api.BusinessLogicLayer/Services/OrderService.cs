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
        private readonly IMapper _mapper;
        private readonly IUoW _unitOfWork;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="mapper">Used to map between domain classes and request/response/dto classes.</param>
        /// <param name="unitOfWork">Used to access the database repositories</param>
        public OrderService(IMapper mapper,  IUoW unitOfWork)
        {
            _mapper = mapper;
            _unitOfWork = unitOfWork;
        }

        /// <summary>
        /// Get all open orders with status waiting for accept. 
        /// </summary>
        /// <returns>A object that wraps a list of orders. </returns>
        public async Task<OpenOrdersResponse> GetOpenOrdersAsync()
        {
            var openOrders = await _unitOfWork.OrderRepository.FindOpenOrdersAsync();
            var openOrderDtos = _mapper.Map<List<OrderDto>>(openOrders);
            var response = new OpenOrdersResponse {Orders = openOrderDtos};
            return response;
        }
    }
}