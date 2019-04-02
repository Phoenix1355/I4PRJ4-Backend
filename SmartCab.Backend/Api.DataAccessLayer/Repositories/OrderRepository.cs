using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Api.DataAccessLayer.Interfaces;
using Api.DataAccessLayer.Models;
using Api.DataAccessLayer.Statuses;
using Microsoft.EntityFrameworkCore;

namespace Api.DataAccessLayer.Repositories
{
    public class OrderRepository : IOrderRepository
    {
        private readonly ApplicationContext _context;

        public OrderRepository(ApplicationContext context)
        {
            _context = context;
        }


        public Task<List<Order>> GetOpenOrdersAsync()
        {
            var rides = _context.Orders
                .Where(x => x.Status == OrderStatus.WaitingForAccept) //TODO: Change this method
                .ToListAsync();
            return rides;
        }
    }
}