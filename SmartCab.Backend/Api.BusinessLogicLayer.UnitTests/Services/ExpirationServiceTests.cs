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
            var ride = new Ride()
            {
                StartDestination = new Address("city", 1000, "street", 1),
                EndDestination = new Address("city", 1000, "street", 1)
            };
            _unitOfWork.RideRepository.FindExpiredUnmatchedRides().ReturnsForAnyArgs(new List<Ride>()
            {
                ride
            });

            _unitOfWork.OrderRepository.FindOrdersWithExpiredRides().ReturnsForAnyArgs(new List<Order>()
            {
                new Order()
                {
                    Rides = new List<Ride>()
                    {
                        ride
                    }
                }
            });

            var push = Substitute.For<IPushNotificationFactory>();
            push.GetPushNotification().ReturnsForAnyArgs(new PushNotification());
            var center = Substitute.For<IPushNotificationService>();

            IExpirationService _uut = new ExpirationService(_unitOfWork, push, center);

            _uut.UpdateExpiredRidesAndOrders();

            _unitOfWork.Received(1).SaveChangesAsync();
        }
    }
}
