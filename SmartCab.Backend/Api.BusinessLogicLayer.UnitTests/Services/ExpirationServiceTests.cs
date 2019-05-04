using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using Api.BusinessLogicLayer.Factories;
using Api.BusinessLogicLayer.Interfaces;
using Api.BusinessLogicLayer.Models;
using Api.BusinessLogicLayer.Services;
using Api.DataAccessLayer.Models;
using Api.DataAccessLayer.UnitOfWork;
using Microsoft.Extensions.Configuration;
using NSubstitute;
using NUnit.Framework;
using NUnit.Framework.Internal;

namespace Api.BusinessLogicLayer.UnitTests.Services
{
    [TestFixture()]
    public class ExpirationServiceTests
    {

        
        [Test]
        public void UpdateExpiredRidesAndOrders_WhenCalled_SavesChanges()
        {
            var _unitOfWork = NSubstitute.Substitute.For<IUnitOfWork>();
            var orderList = new List<Order>();
            var order = new Order();
            var ride = new Ride();
            ride.StartDestination = new Address("city", 1000, "street", 1);
            ride.EndDestination = new Address("city", 1000, "street", 1);
            order.Rides.Add(ride);
            var rideList = new List<Ride>();
            rideList.Add(ride);
            _unitOfWork.OrderRepository.FindOrdersWithExpiredRides().ReturnsForAnyArgs(orderList);
            _unitOfWork.RideRepository.FindExpiredUnmatchedRides().ReturnsForAnyArgs(rideList);
            var push = Substitute.For<IPushNotificationFactory>();
            var pushNot = new PushNotification();
            push.GetPushNotification().ReturnsForAnyArgs(pushNot);
            var center = Substitute.For<IPushNotificationService>();

            IExpirationService _uut = new ExpirationService(_unitOfWork, push, center);
            _uut.UpdateExpiredRidesAndOrders();

            _unitOfWork.Received(1).SaveChangesAsync();
        }

        [Test]
        public void UpdateExpiredRidesAndOrders_WhenCalled_NotifiesTwice()
        {
            var _unitOfWork = NSubstitute.Substitute.For<IUnitOfWork>();
            var orderList = new List<Order>();
            var order = new Order();
            var ride = new Ride();
            ride.StartDestination = new Address("city", 1000, "street", 1);
            ride.EndDestination = new Address("city", 1000, "street", 1);
            order.Rides.Add(ride);
            var rideList = new List<Ride>();
            rideList.Add(ride);
            _unitOfWork.OrderRepository.FindOrdersWithExpiredRides().ReturnsForAnyArgs(orderList);
            _unitOfWork.RideRepository.FindExpiredUnmatchedRides().ReturnsForAnyArgs(rideList);
            var push = Substitute.For<IPushNotificationFactory>();
            var pushNot = new PushNotification();
            push.GetPushNotification().ReturnsForAnyArgs(pushNot);
            var center = Substitute.For<IPushNotificationService>();

            IExpirationService _uut = new ExpirationService(_unitOfWork, push, center);
            _uut.UpdateExpiredRidesAndOrders();

            center.ReceivedWithAnyArgs(1).SendAsync(null);
        }
    }
}
