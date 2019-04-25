using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Api.DataAccessLayer.Models;
using Api.DataAccessLayer.Repositories;
using Api.DataAccessLayer.Statuses;
using CustomExceptions;

namespace Api.DataAccessLayer.UnitOfWork
{
    public interface IUoW
    {
        GenericRepository<Customer> CustomerRepository { get; }
        GenericRepository<Ride> RideRepository { get; }
        GenericRepository<Order> OrderRepository { get; }

        void ReservePriceFromCustomer(string customerId, decimal price);
        Order AddRideToOrder(Ride ride, Order order);
        void SaveChanges();
    }
}
