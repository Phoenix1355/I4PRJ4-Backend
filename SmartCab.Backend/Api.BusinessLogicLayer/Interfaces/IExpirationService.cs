using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Api.BusinessLogicLayer.Interfaces
{
    /// <summary>
    /// Interface to Expiration service, for testability. 
    /// </summary>
    public interface IExpirationService
    {
        Task UpdateExpiredRidesAndOrders();
    } 
}
