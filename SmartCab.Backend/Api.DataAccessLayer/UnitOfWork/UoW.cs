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
    public class UoW : IUoW
    {
        public IGenericRepository<Customer> GenericCustomerRepository { get; }
        public IGenericRepository<Ride> GenericRideRepository { get; }
        public IGenericRepository<Order> GenericOrderRepository { get; }
        private ApplicationContext _context;
        public IIdentityUserRepository IdentityUserRepository { get; }

        public UoW(ApplicationContext context, IIdentityUserRepository identityUserRepository)
        {
            _context = context;
            GenericCustomerRepository = new GenericRepository<Customer>(_context);
            GenericRideRepository = new GenericRepository<Ride>(_context);
            GenericOrderRepository = new GenericRepository<Order>(_context);
            IdentityUserRepository = identityUserRepository;
        }


        public void ReservePriceFromCustomer(string customerId, decimal price)
        {
            var customer = GenericCustomerRepository.FindOnlyOne(customerFilter=>customerFilter.Id == customerId);

            if ((customer.Balance - customer.ReservedAmount) >= price)
            {
                customer.ReservedAmount += price;
            }
            else
            {
                throw new InsufficientFundsException("Not enough credit");
            }

            GenericCustomerRepository.Update(customer);
        }

        public Order AddRideToOrder(Ride ride, Order order)
        {
            if (_context.Orders.Count(o => o.Rides.Contains(ride)) != 0)
            {
                throw new MultipleOrderException("Already an order for given ride. ");
            }

            order.Price += ride.Price;
            order.Rides.Add(ride);
            return GenericOrderRepository.Update(order);
        }

        public void SaveChanges()
        {
            _context.SaveChanges();
        }
    }
}
