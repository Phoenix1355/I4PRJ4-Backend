using System.Collections.Generic;
using System.Threading.Tasks;
using Api.DataAccessLayer.Models;
using CustomExceptions;

namespace Api.DataAccessLayer.Interfaces
{
    /// <summary>
    /// Interface for CustomerRepository, containing relevant methods. 
    /// </summary>
    public interface ICustomerRepository
    {
        Task DepositAsync(string customerId, decimal deposit);
        void ReservePriceFromCustomer(string customerId, decimal price);
    }
}