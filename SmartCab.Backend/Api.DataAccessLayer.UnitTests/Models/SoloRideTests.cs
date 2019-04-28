using System;
using System.Collections.Generic;
using System.Text;
using Api.DataAccessLayer.Models;
using Api.DataAccessLayer.Statuses;
using NUnit.Framework;
using NUnit.Framework.Internal;

namespace Api.DataAccessLayer.UnitTests.Models
{
    [TestFixture()]
    public class SoloRideTests
    {
        [Test]
        public void Ctor_SoloRideConstructed_HasStatusWaitingForAccept()
        {
            var soloRide = new SoloRide();
            Assert.That(soloRide.Status,Is.EqualTo(RideStatus.WaitingForAccept));
        }
    }
}
