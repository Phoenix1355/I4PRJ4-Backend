using Api.BusinessLogicLayer.Interfaces;

namespace Api.BusinessLogicLayer.Helpers
{
    /// <summary>
    /// This class implements a concrete stategy used to calculate prices for solo rides.
    /// </summary>
    public class SoloRideStrategy : IPriceCalculator
    {
        /// <summary>
        /// Calculates and returns the price of a solo ride.
        /// </summary>
        /// <param name="distanceInKm">The distance in kilometers.</param>
        /// <returns>The price.</returns>
        public decimal CalculatePrice(decimal distanceInKm)
        {
            const decimal multiplier = 10;
            return distanceInKm * multiplier;
        }
    }
}