using System;
using Api.BusinessLogicLayer.Enums;
using Api.BusinessLogicLayer.Factories;
using Api.BusinessLogicLayer.Helpers;
using NUnit.Framework;

namespace Api.BusinessLogicLayer.UnitTests.Factories
{
    [TestFixture]
    public class PriceStrategyFactoryTests
    {
        [Test]
        public void GetPriceStrategy_RideTypeEqualsSoloRide_ReturnsSoloRidePriceStrategy()
        {
            var factory = new PriceStrategyFactory();

            var strategy = factory.GetPriceStrategy(RideType.SoloRide);

            Assert.That(strategy.GetType(), Is.EqualTo(typeof(SoloRideStrategy)));
        }

        [Test]
        public void GetPriceStrategy_RideTypeEqualsSharedRide_ReturnsSharedRidePriceStrategy()
        {
            var factory = new PriceStrategyFactory();

            var strategy = factory.GetPriceStrategy(RideType.SharedRide);

            Assert.That(strategy.GetType(), Is.EqualTo(typeof(SharedRideStrategy)));
        }

        [Test]
        public void GetPriceStrategy_RideTypeIsInvalid_ThrowsException()
        {
            var factory = new PriceStrategyFactory();

            Assert.Throws<Exception>(()=> factory.GetPriceStrategy((RideType) 999));
        }
    }
}