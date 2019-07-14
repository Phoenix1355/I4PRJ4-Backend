using System;
using Api.BusinessLogicLayer.Enums;
using Api.BusinessLogicLayer.Helpers;
using Api.BusinessLogicLayer.Interfaces;

namespace Api.BusinessLogicLayer.Factories
{
    /// <summary>
    /// Factory to return different price strategies. 
    /// </summary>
    public class PriceStrategyFactory : IPriceStrategyFactory
    {
        /// <summary>
        /// This factory returns a strategy for calculation of a price for a given ride type.
        /// </summary>
        /// <param name="type">The type of the ride</param>
        /// <returns>The strategy used for the given ride</returns>
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