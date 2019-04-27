using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Api.BusinessLogicLayer.DataTransferObjects;
using Api.BusinessLogicLayer.Interfaces;
using Api.BusinessLogicLayer.Responses;
using Api.DataAccessLayer.Interfaces;
using Api.DataAccessLayer.Models;
using AutoMapper;

namespace Api.BusinessLogicLayer.Services
{
    /// <summary>
    /// This class contains business logic related to orders.
    /// </summary>
    public class OrderService : IOrderService
    {
        private readonly IOrderRepository _orderRepository;
        private readonly IMapper _mapper;

        /// <summary>
        /// Constructor for this class
        /// </summary>
        /// <param name="orderRepository">The repository used to get and edit orders.</param>
        /// <param name="mapper">A mapper used to map object to/from data transfer objects (DTO's)</param>
        public OrderService(IOrderRepository orderRepository, IMapper mapper)
        {
            _orderRepository = orderRepository;
            _mapper = mapper;
        }

        /// <summary>
        /// Returns an object containing all open orders stored in the system.
        /// </summary>
        /// <returns>An object containing all open orders stored in the system</returns>
        public async Task<OpenOrdersResponse> GetOpenOrdersAsync()
        {
            var openOrders = await _orderRepository.GetOpenOrdersAsync();
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
        public async Task<AcceptOrderResponse> AcceptOrder(string taxiCompanyId, int orderId)
        {
            var order = await _orderRepository.AcceptOrder(taxiCompanyId, orderId);
            //TODO: push out notifications to associated customers (Debit customers should happen in the dataaccess layer as par of a transaction)
            var orderDto = _mapper.Map<OrderDto>(order);
            var response = new AcceptOrderResponse {Order = orderDto};
            return response;
        }
    }
}