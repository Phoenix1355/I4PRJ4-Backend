using Api.BusinessLogicLayer.Interfaces;

namespace Api.BusinessLogicLayer.Helpers
{
    /// <summary>
    /// This class implements a concrete stategy used to calculate prices for shared rides.
    /// </summary>
    public class SharedRideStrategy : IPriceCalculator
    {
        /// <summary>
        /// Calculates and returns the price of a shared ride.
        /// </summary>
        /// <param name="distanceInKm">The distance in kilometers.</param>
        /// <returns>The price.</returns>
        public decimal CalculatePrice(decimal distanceInKm)
        {
            const decimal multiplier = 10;
            const decimal discount = (decimal)0.75;

            return distanceInKm * multiplier * discount;
        }
    }
}