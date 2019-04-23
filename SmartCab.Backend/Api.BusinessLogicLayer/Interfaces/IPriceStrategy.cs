namespace Api.BusinessLogicLayer.Interfaces
{
    public interface IPriceStrategy
    {
        decimal CalculatePrice(decimal distanceInKm);
    }
}