using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Api.BusinessLogicLayer.Interfaces;
using Api.DataAccessLayer.Statuses;
using Api.DataAccessLayer.UnitOfWork;
using Castle.Core.Internal;

namespace Api.BusinessLogicLayer.Services
{
    public class ExpirationService : IExpirationService
    {
        private readonly IUnitOfWork _unitOfWork;

        public ExpirationService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        /// <summary>
        /// Simple that only updates rides to expired
        /// Requires Sync Behaviour
        /// </summary>
        /// <returns></returns>
        public void UpdateExpiredRidesAndOrders()
        {
             UpdateNonMatchedRides().Wait();
             UpdateOrdersWithExpiredRides().Wait();
             _unitOfWork.SaveChangesAsync().Wait();
        }

        /// <summary>
        /// All expired shared rides that have expired. Update these. 
        /// </summary>
        /// <returns></returns>
        private async Task UpdateNonMatchedRides()
        {
            var nonmatchedRides = await _unitOfWork.RideRepository.FindExpiredUnmatchedRides();

            foreach (var ride in nonmatchedRides)
            {
                ride.Status = RideStatus.Expired;
                _unitOfWork.RideRepository.Update(ride);
                //Unreserve the amount
                await _unitOfWork.CustomerRepository.ReservePriceFromCustomerAsync(ride.CustomerId,-ride.Price);

                //Notify the customer
            }
        }

        /// <summary>
        /// All order that contains rides that have expired
        /// </summary>
        /// <returns></returns>
        private async Task UpdateOrdersWithExpiredRides()
        {
            var ordersWithExpiredRides = await _unitOfWork.OrderRepository.FindOrdersWithExpiredRides();

            foreach (var order in ordersWithExpiredRides)
            {
                order.Status = OrderStatus.Expired;
                foreach (var orderRide in order.Rides)
                {
                    orderRide.Status = RideStatus.Expired;
                    await _unitOfWork.CustomerRepository.ReservePriceFromCustomerAsync(orderRide.CustomerId,-orderRide.Price);
                }
                _unitOfWork.OrderRepository.Update(order);
            }
        }

        
    }
}
