using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Api.BusinessLogicLayer.Enums;
using Api.BusinessLogicLayer.Services;
using Api.DataAccessLayer;
using Api.DataAccessLayer.Statuses;
using Api.DataAccessLayer.UnitOfWork;
using Microsoft.EntityFrameworkCore;
using NUnit.Framework;
using NUnit.Framework.Internal;

namespace Api.IntegrationTests.Expiration
{
    [TestFixture()]
    public class ExpirationTests : IntegrationSetup
    {
        [Test]
        public async Task UpdateExpiredRidesAndOrders_UpdatesOrderToExpired_OrdersHasBeenUpdated()
        {
            await CreateRideWithLogin();
            await CreateRide(RideType.SoloRide, 1);
            ClearHeaders();
            await LoginOnCustomerAccount("test13@gmail.com");
            await DepositToCustomer(1000);
            await CreateRide(RideType.SharedRide, 1);

            //Sleep to allow task to run(hopefully)
            Thread.Sleep(30000);
            
            using (var context = _factory.CreateContext())
            {
                Assert.That(context.Orders.First().Status,Is.EqualTo(OrderStatus.Expired));
                Assert.That(context.Orders.First().Rides.First(), Is.EqualTo(RideStatus.Expired));
            }
        }
    }
}