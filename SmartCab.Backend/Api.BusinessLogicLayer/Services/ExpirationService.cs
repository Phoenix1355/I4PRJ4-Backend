using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Api.BusinessLogicLayer.Interfaces;
using Api.DataAccessLayer.Models;
using Api.DataAccessLayer.Statuses;
using Api.DataAccessLayer.UnitOfWork;
using Castle.Core.Internal;

namespace Api.BusinessLogicLayer.Services
{
    /// <summary>
    /// A class that is used by a background task to ensure that rides and orders that is expired,
    /// gets updated and notifies the customer. 
    /// </summary>
    public class ExpirationService : IExpirationService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IPushNotificationFactory _pushNotificationFactory;
        private readonly IPushNotificationService _pushNotificationService;

        /// <summary>
        /// Constructor for Expiration service
        /// </summary>
        /// <param name="unitOfWork">Unit of work</param>
        /// <param name="pushNotificationFactory">Factory for push notifications</param>
        /// <param name="pushNotificationService">Service to send notifications</param>
        public ExpirationService(IUnitOfWork unitOfWork, 
            IPushNotificationFactory pushNotificationFactory, 
            IPushNotificationService pushNotificationService)
        {
            _unitOfWork = unitOfWork;
            _pushNotificationFactory = pushNotificationFactory;
            _pushNotificationService = pushNotificationService;
        }

        /// <summary>
        /// Updates all expired ride and orders and notifies customer. 
        /// </summary>
        /// <returns></returns>
        public async Task UpdateExpiredRidesAndOrders()
        {
             await UpdateNonMatchedRides();
             await UpdateOrdersWithExpiredRides();
             await _unitOfWork.SaveChangesAsync();
        }

        /// <summary>
        /// Updates all expired shared rides that have expired.
        /// These are rides that have not been matched yet. 
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
                await NotifyCustomerAboutMatchNotFound(ride);
            }
        }

        /// <summary>
        /// All order that contains rides that have expired
        /// This is both shared and solo rides. 
        /// </summary>
        /// <returns></returns>
        private async Task UpdateOrdersWithExpiredRides()
        {
            var ordersWithExpiredRides = await _unitOfWork.OrderRepository.FindOrdersWithExpiredRides();

            //Iterate through all orders
            foreach (var order in ordersWithExpiredRides)
            {
                //Set status
                order.Status = OrderStatus.Expired;

                //Iterate through all rides in the given order
                foreach (var orderRide in order.Rides)
                {
                    orderRide.Status = RideStatus.Expired;
                    //Unreserve amount from customer. 
                    await _unitOfWork.CustomerRepository.ReservePriceFromCustomerAsync(orderRide.CustomerId,-orderRide.Price);
                    await NotifyCustomerAboutOrderExpired(orderRide);
                }
                _unitOfWork.OrderRepository.Update(order);
            }
        }

        /// <summary>
        /// Contains the information that should be sent to the customer about missing match
        /// </summary>
        /// <param name="expiredRide">Ride which did not find a match</param>
        /// <returns></returns>
        private async Task NotifyCustomerAboutMatchNotFound(Ride expiredRide)
        {
            var notification = _pushNotificationFactory.GetPushNotification();
            notification.Name = "Ingen match";
            notification.Title = "Ingen match";
            notification.Body =
                $"Din dele-tur fra {expiredRide.StartDestination.StreetName} {expiredRide.StartDestination.StreetNumber} i {expiredRide.StartDestination.CityName} til {expiredRide.EndDestination.StreetName} {expiredRide.EndDestination.StreetNumber} i {expiredRide.EndDestination.CityName} fandt ingen match. Prøv en solotur. ";
            notification.Devices.Add(expiredRide.DeviceId);
            notification.CustomData.Add("Type", "NoMatch");

            await _pushNotificationService.SendAsync(notification);
        }

        /// <summary>
        /// Contains the information about the order that expired. 
        /// </summary>
        /// <param name="expiredRide">Ride which did not get accepted</param>
        /// <returns></returns>
        private async Task NotifyCustomerAboutOrderExpired(Ride expiredRide)
        {
            var notification = _pushNotificationFactory.GetPushNotification();
            notification.Name = "Udløbet";
            notification.Title = "Tur udløbet";
            notification.Body =
                $"Din tur fra {expiredRide.StartDestination.StreetName} {expiredRide.StartDestination.StreetNumber} i {expiredRide.StartDestination.CityName} til {expiredRide.EndDestination.StreetName} {expiredRide.EndDestination.StreetNumber} i {expiredRide.EndDestination.CityName} er ikke blevet accepteret af en vognmand.";
            notification.Devices.Add(expiredRide.DeviceId);
            notification.CustomData.Add("Type", "Expired");

            await _pushNotificationService.SendAsync(notification);
        }
    }
}
