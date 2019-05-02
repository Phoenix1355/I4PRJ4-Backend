using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Api.BusinessLogicLayer.Enums;
using Api.BusinessLogicLayer.Factories;
using Api.BusinessLogicLayer.Services;
using Api.DataAccessLayer;
using Api.DataAccessLayer.Repositories;
using Api.DataAccessLayer.Statuses;
using Api.DataAccessLayer.UnitOfWork;
using Api.DataAccessLayer.UnitTests.Fakes;
using Api.IntegrationTests.Fakes;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using NSubstitute;
using NUnit.Framework;
using NUnit.Framework.Internal;

namespace Api.IntegrationTests.Expiration
{
    [TestFixture()]
    public class ExpirationTests : IntegrationSetup
    {
        /// <summary>
        /// Long test, multiple asserts. 
        /// </summary>
        /// <returns></returns>
        /*[Test]
        public async Task UpdateExpiredRidesAndOrders_UpdatesOrderToExpired_OrdersHasBeenUpdated()
        {
            await CreateRideWithLogin();
            await CreateRide(RideType.SoloRide, 1);
            ClearHeaders();
            await LoginOnCustomerAccount("test13@gmail.com");
            await DepositToCustomer(1000);
            await CreateRide(RideType.SharedRide, 1);

            //Sleep to allow task to run(hopefully)
            //Prone to error, no way to substantial test this. 
            Thread.Sleep(40000);
            
            using (var context = _factory.CreateContext())
            {
                Assert.That(context.Orders.First().Status,Is.EqualTo(OrderStatus.Expired));
                Assert.That(context.Rides.Where(ride=>ride.Status !=RideStatus.Expired).Any, Is.EqualTo(false));
                Assert.That(context.Customers.Where(customer=>customer.ReservedAmount!=0).Any,Is.EqualTo(false));
            }
        }*/
    }
}