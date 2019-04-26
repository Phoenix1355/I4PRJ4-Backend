using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Api.DataAccessLayer.Interfaces;
using Api.DataAccessLayer.Models;
using Api.DataAccessLayer.Repositories;
using Api.DataAccessLayer.Statuses;
using CustomExceptions;

namespace Api.DataAccessLayer.UnitOfWork
{
    public interface IUoW
    {
        IGenericRepository<Customer> CustomerRepository { get; }
        IGenericRepository<Ride> RideRepository { get; }
        IGenericRepository<Order> OrderRepository { get; }
        IIdentityUserRepository IdentityUserRepository { get; }

        void ReservePriceFromCustomer(string customerId, decimal price);
        Order AddRideToOrder(Ride ride, Order order);
        void SaveChanges();
        Task DepositAsync(string customerId, decimal deposit);
    }
}
