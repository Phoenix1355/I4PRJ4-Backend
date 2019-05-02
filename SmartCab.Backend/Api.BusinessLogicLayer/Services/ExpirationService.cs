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
    public class ExpirationService : IExpirationService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IPushNotificationFactory _pushNotificationFactory;
        private readonly IPushNotificationService _pushNotificationService;

        public ExpirationService(IUnitOfWork unitOfWork, 
            IPushNotificationFactory pushNotificationFactory, 
            IPushNotificationService pushNotificationService)
        {
            _unitOfWork = unitOfWork;
            _pushNotificationFactory = pushNotificationFactory;
            _pushNotificationService = pushNotificationService;
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
                await NotifyCustomerAboutMatchNotFound(ride);
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
                    await NotifyCustomerAboutOrderExpired(orderRide);
                }
                _unitOfWork.OrderRepository.Update(order);
            }
        }

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
