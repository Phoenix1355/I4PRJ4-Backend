using System;
using System.Collections.Generic;
using System.Text;
using Api.BusinessLogicLayer.Interfaces;
using Api.BusinessLogicLayer.Services;
using Api.DataAccessLayer.Models;
using Api.DataAccessLayer.UnitOfWork;
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
            _unitOfWork.RideRepository.FindExpiredUnmatchedRides().ReturnsForAnyArgs(new List<Ride>()
            {
                new Ride(),
            });

            _unitOfWork.OrderRepository.FindOrdersWithExpiredRides().ReturnsForAnyArgs(new List<Order>()
            {
                new Order()
            });

            IExpirationService _uut = new ExpirationService(_unitOfWork);

            _uut.UpdateExpiredRidesAndOrders();

            _unitOfWork.Received(1).SaveChangesAsync();
        }
    }
}
