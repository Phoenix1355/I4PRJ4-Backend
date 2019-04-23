using Api.BusinessLogicLayer.Helpers;
using NUnit.Framework;

namespace Api.BusinessLogicLayer.UnitTests.Helpers
{
    [TestFixture]
    public class SoloRideStrategyTests
    {
        [TestCase(0.1, 1)]
        [TestCase(1, 10)]
        [TestCase(5, 50)]
        [TestCase(10.9, 109)]
        [TestCase(50, 500)]
        [TestCase(1000.1, 10001)]
        public void CalculatePrice_WhenCalled_CalculatesPriceCorrectly(decimal distance, decimal expectedPrice)
        {
            var uut = new SoloRideStrategy();

            var price = uut.CalculatePrice(distance);

            Assert.That(price, Is.EqualTo(expectedPrice));
        }
    }
}