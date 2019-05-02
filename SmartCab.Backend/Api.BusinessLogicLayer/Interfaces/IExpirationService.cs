using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Api.BusinessLogicLayer.Interfaces
{
    public interface IExpirationService
    {
        void UpdateExpiredRidesAndOrders();
    } 
}
