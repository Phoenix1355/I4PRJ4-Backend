using Api.DataAccessLayer.Models;
using Api.DataAccessLayer.Statuses;
using NUnit.Framework;

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
