﻿using System.Collections.Generic;
using System.Threading.Tasks;
using Api.DataAccessLayer.Models;

namespace Api.DataAccessLayer.Interfaces
{
    public interface IOrderRepository
    {
        Task<List<Order>> GetOpenOrdersAsync();
        Task<Order> AcceptOrder(string taxicompanyId, int orderId);
    }
}