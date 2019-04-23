using Api.BusinessLogicLayer.Enums;

namespace Api.BusinessLogicLayer.Interfaces
{
    public interface IPriceStrategyFactory
    {
        IPriceStrategy GetPriceStrategy(RideType type);
    }
}