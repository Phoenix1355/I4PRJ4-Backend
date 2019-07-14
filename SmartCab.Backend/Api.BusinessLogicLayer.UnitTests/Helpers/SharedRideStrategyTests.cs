using Api.BusinessLogicLayer.Helpers;
using NUnit.Framework;

namespace Api.BusinessLogicLayer.UnitTests.Helpers
{
    [TestFixture]
    public class SharedRideStrategyTests
    {
        [TestCase(0.1, 0.75)]
        [TestCase(1, 7.5)]
        [TestCase(5, 37.5)]
        [TestCase(10.9, 81.75)]
        [TestCase(50, 375)]
        [TestCase(1000.1, 7500.75)]
        public void CalculatePrice_WhenCalled_CalculatedPriceCorrectly(decimal distance, decimal expectedPrice)
        {
            var uut = new SharedRideStrategy();

            var price = uut.CalculatePrice(distance);

            Assert.That(price, Is.EqualTo(expectedPrice));
        }
    }
}