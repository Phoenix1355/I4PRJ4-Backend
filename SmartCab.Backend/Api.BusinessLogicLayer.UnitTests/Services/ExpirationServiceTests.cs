using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
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
        public async Task UpdateExpiredRidesAndOrders_WhenCalled_SavesChanges()
        {
            var _unitOfWork = NSubstitute.Substitute.For<IUnitOfWork>();

            //Create
            var orderList = new List<Order>();
            var order = new Order();
            var ride = new Ride();

            //Set
            ride.StartDestination = new Address("city", 1000, "street", 1);
            ride.EndDestination = new Address("city", 1000, "street", 1);
            order.Rides.Add(ride);
            orderList.Add(order);
            //Create and set
            var rideList = new List<Ride>();
            rideList.Add(ride);

            //Stub out
            _unitOfWork.OrderRepository.FindOrdersWithExpiredRides().ReturnsForAnyArgs(orderList);
            _unitOfWork.RideRepository.FindExpiredUnmatchedRides().ReturnsForAnyArgs(rideList);


            var push = Substitute.For<IPushNotificationFactory>();
            var pushNot = new PushNotification();
            //VERY Important that push notification gets a new otherwise it will throw misleading exception
            //For all. So 1 order with 1 ride, and 1 ride. aka two. 
            push.GetPushNotification().ReturnsForAnyArgs(new PushNotification(), new PushNotification());
            var center = Substitute.For<IPushNotificationService>();

            IExpirationService _uut = new ExpirationService(_unitOfWork, push, center);
            await _uut.UpdateExpiredRidesAndOrders();

            _unitOfWork.Received(1).SaveChangesAsync();
        }

        [Test]
        public async Task UpdateExpiredRidesAndOrders_WhenCalled_NotifiesTwice()
        {
            var _unitOfWork = NSubstitute.Substitute.For<IUnitOfWork>();
            var orderList = new List<Order>();
            var order = new Order();
            var ride = new Ride();
            ride.StartDestination = new Address("city", 1000, "street", 1);
            ride.EndDestination = new Address("city", 1000, "street", 1);
            order.Rides.Add(ride);
            orderList.Add(order);
            var rideList = new List<Ride>();
            rideList.Add(ride);
            _unitOfWork.OrderRepository.FindOrdersWithExpiredRides().ReturnsForAnyArgs(orderList);
            _unitOfWork.RideRepository.FindExpiredUnmatchedRides().ReturnsForAnyArgs(rideList);
            var push = Substitute.For<IPushNotificationFactory>();
            push.GetPushNotification().ReturnsForAnyArgs(new PushNotification(), new PushNotification());
            var center = Substitute.For<IPushNotificationService>();

            IExpirationService _uut = new ExpirationService(_unitOfWork, push, center);
            await _uut.UpdateExpiredRidesAndOrders();

            center.ReceivedWithAnyArgs(2).SendAsync(null);
        }
    }
}
