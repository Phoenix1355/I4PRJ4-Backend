namespace Api.BusinessLogicLayer.Interfaces
{
    /// <summary>
    /// Interface for price strategy, that the price strategy factory can create. 
    /// </summary>
    public interface IPriceStrategy
    {
        decimal CalculatePrice(decimal distanceInKm);
    }
}