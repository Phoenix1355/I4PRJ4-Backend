using Api.DataAccessLayer.Models;

namespace Api.BusinessLogicLayer.Interfaces
{
    /// <summary>
    /// Defines a number of methods related to a customer.
    /// </summary>
    public interface IMatchService
    {
        bool Match(Ride ride1, Ride ride2, int maxDistance);
    }
}