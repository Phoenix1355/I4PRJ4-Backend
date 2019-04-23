using System;
using Api.BusinessLogicLayer.Enums;
using Api.BusinessLogicLayer.Helpers;
using Api.BusinessLogicLayer.Interfaces;

namespace Api.BusinessLogicLayer.Factories
{
    public class PriceStrategyFactory : IPriceStrategyFactory
    {
        public IPriceStrategy GetPriceStrategy(RideType type)
        {
            if (type == RideType.SoloRide)
            {
                return new SoloRideStrategy();
            }

            if (type == RideType.SharedRide)
            {
                return new SharedRideStrategy();
            }

            throw new Exception("Invalid ride type");
        }
    }
}