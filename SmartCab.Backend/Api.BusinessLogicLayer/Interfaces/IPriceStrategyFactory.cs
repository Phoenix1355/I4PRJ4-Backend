using Api.BusinessLogicLayer.Enums;

namespace Api.BusinessLogicLayer.Interfaces
{
    /// <summary>
    /// Factory of price strategies. 
    /// </summary>
    public interface IPriceStrategyFactory
    {
        IPriceStrategy GetPriceStrategy(RideType type);
    }
}