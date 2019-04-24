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
    public class CreateRideUOW
    {
        private GenericRepository<Customer> _customerRepository;
        private GenericRepository<SoloRide> _rideRepository;
        private ApplicationContext _context;

        public CreateRideUOW(ApplicationContext context)
        {
            _context = context;
            _customerRepository = new GenericRepository<Customer>(_context);
            _rideRepository = new GenericRepository<SoloRide>(_context);
        }

        public async Task<SoloRide> AddSoloRideAsync(SoloRide ride)
        {
            using (var transaction = _context.Database.BeginTransaction())
            {
                //Add ride and reserves amount.
                ride = await AddRideAndReserveFundsForRide(ride);

                //Adds order
                await AddOrderFromRide(ride);

                //Reserve from Customer
                transaction.Commit();
                return ride;
            }
        }

        private async Task<SoloRide> AddRideAndReserveFundsForRide(SoloRide ride)
        {
            //Reserve funds. 
            ReservePriceFromCustomer(ride.CustomerId, ride.Price);

            //adds SoloRide
            return _rideRepository.Update(ride);
        }

        private void ReservePriceFromCustomer(string customerId, decimal price)
        {
            var customer = _customerRepository.FindOnlyOne(customerFilter=>customerFilter.Id == customerId);
            _customerRepository.Update(customer, (customerInline) =>
            {
                if ((customerInline.Balance - customerInline.ReservedAmount) >= price)
                {
                    customerInline.ReservedAmount += price;
                }
                else
                {
                    throw new InsufficientFundsException("Not enough credit");
                }
            });
        }

        private async Task<SoloRide> AddRide(SoloRide ride)
        {
            _context.Rides.Update(ride);
            await _context.SaveChangesAsync();
            return ride;
        }

        private async Task<Order> AddOrderFromRide(Ride ride)
        {
            if (_context.Orders.Count(o => o.Rides.Contains(ride)) != 0)
            {
                throw new MultipleOrderException("Already an order for given ride. ");
            }

            Order order = new Order()
            {
                Price = ride.Price,
                Rides = new List<Ride>(),
                Status = OrderStatus.WaitingForAccept,
            };
            order.Rides.Add(ride);

            await _context.Orders.AddAsync(order);
            await _context.SaveChangesAsync();
            return order;
        }

    }
}
