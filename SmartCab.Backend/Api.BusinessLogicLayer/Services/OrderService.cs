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
    /// <summary>
    /// This class contains business logic related to orders.
    /// </summary>
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
        /// Returns an object containing all open orders stored in the system.
        /// </summary>
        /// <returns>An object containing all open orders stored in the system</returns>
        public async Task<OpenOrdersResponse> GetOpenOrdersAsync()
        {
            var openOrders = await _unitOfWork.OrderRepository.FindOpenOrdersAsync();
            var openOrderDtos = _mapper.Map<List<OrderDto>>(openOrders);
            var response = new OpenOrdersResponse {Orders = openOrderDtos};
            return response;
        }

        /// <summary>
        /// Changes an order and all its associated rides to the status 'Accepted'.
        /// </summary>
        /// <remarks>
        /// All customers are debited for the cost of the ride.<br/>
        /// All customers will also receive a notification to let them know,<br/>
        /// that the ride they ordered has been accepted and debited.
        /// </remarks>
        /// <param name="taxiCompanyId">The id of the taxi company who accepted the order.</param>
        /// <param name="orderId">The id of the order that should be accepted.</param>
        /// <returns>A response containing the updated order.</returns>
        public async Task<AcceptOrderResponse> AcceptOrderAsync(string taxiCompanyId, int orderId)
        {
            var order = await _orderRepository.AcceptOrderAsync(taxiCompanyId, orderId);
            //TODO: push out notifications to associated customers (Debit customers should happen in the dataaccess layer as par of a transaction)
            var orderDto = _mapper.Map<OrderDto>(order);
            var response = new AcceptOrderResponse {Order = orderDto};
            return response;
        }
    }
}