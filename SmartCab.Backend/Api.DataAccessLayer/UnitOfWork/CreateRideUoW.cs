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
        private GenericRepository<Customer> _customerRepository;
        private GenericRepository<SoloRide> _rideRepository;
        private GenericRepository<Order> _orderRepository;
        private ApplicationContext _context;

        public CreateRideUOW(ApplicationContext context)
        {
            _context = context;
            _customerRepository = new GenericRepository<Customer>(_context);
            _rideRepository = new GenericRepository<SoloRide>(_context);
            _orderRepository = new GenericRepository<Order>(_context);
        }

        public void ReservePriceFromCustomer(string customerId, decimal price)
        {
            var customer = _customerRepository.FindOnlyOne(customerFilter=>customerFilter.Id == customerId);

            if ((customer.Balance - customer.ReservedAmount) >= price)
            {
                customer.ReservedAmount += price;
            }
            else
            {
                throw new InsufficientFundsException("Not enough credit");
            }

            _customerRepository.Update(customer);
        }

        public SoloRide AddRide(SoloRide ride)
        {
            return _rideRepository.Add(ride);
        }

        public Order CreateOrder()
        {
            Order order = new Order()
            {
                Price = 0,
                Rides = new List<Ride>(),
                Status = OrderStatus.WaitingForAccept,
            };
            return _orderRepository.Add(order);
        }

        public Order AddRideToOrder(Ride ride, Order order)
        {
            if (_context.Orders.Count(o => o.Rides.Contains(ride)) != 0)
            {
                throw new MultipleOrderException("Already an order for given ride. ");
            }

            order.Price += ride.Price;
            order.Rides.Add(ride);
            return _orderRepository.Update(order);
        }

        public void SaveChanges()
        {
            _context.SaveChanges();
        }
    }
}
