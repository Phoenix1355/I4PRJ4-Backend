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
    public class CreateRideUOW : ICreateRideUOW
    {
        public GenericRepository<Customer> CustomerRepository { get; }
        public GenericRepository<Ride> RideRepository { get; }
        public GenericRepository<Order> OrderRepository { get; }
        private ApplicationContext _context;

        public CreateRideUOW(ApplicationContext context)
        {
            _context = context;
            CustomerRepository = new GenericRepository<Customer>(_context);
            RideRepository = new GenericRepository<Ride>(_context);
            OrderRepository = new GenericRepository<Order>(_context);
        }


        public void ReservePriceFromCustomer(string customerId, decimal price)
        {
            var customer = CustomerRepository.FindOnlyOne(customerFilter=>customerFilter.Id == customerId);

            if ((customer.Balance - customer.ReservedAmount) >= price)
            {
                customer.ReservedAmount += price;
            }
            else
            {
                throw new InsufficientFundsException("Not enough credit");
            }

            CustomerRepository.Update(customer);
        }

        public Order AddRideToOrder(Ride ride, Order order)
        {
            if (_context.Orders.Count(o => o.Rides.Contains(ride)) != 0)
            {
                throw new MultipleOrderException("Already an order for given ride. ");
            }

            order.Price += ride.Price;
            order.Rides.Add(ride);
            return OrderRepository.Update(order);
        }

        public void SaveChanges()
        {
            _context.SaveChanges();
        }
    }
}
