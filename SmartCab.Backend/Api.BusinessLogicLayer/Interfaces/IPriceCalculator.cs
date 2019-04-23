namespace Api.BusinessLogicLayer.Interfaces
{
    public interface IPriceCalculator
    {
        decimal CalculatePrice(decimal distanceInKm);
    }
}