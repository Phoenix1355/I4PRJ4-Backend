using System.Collections.Generic;
using System.Threading.Tasks;
using Api.DataAccessLayer.Models;
using Api.DataAccessLayer.Repositories;
using CustomExceptions;

namespace Api.DataAccessLayer.Interfaces
{
    /// <summary>
    /// Interface for CustomerRepository, containing relevant methods. 
    /// </summary>
    public interface ICustomerRepository : IGenericRepository<Customer>
    {
        Task DepositAsync(string customerId, decimal deposit);
        Task ReservePriceFromCustomerAsync(string customerId, decimal price);
        Task<Customer> FindByEmailAsync(string email);
        Task<List<Ride>> FindCustomerRidesAsync(string customerId);

        /// <summary>
        /// Debits the given amount to a customers account.
        /// </summary>
        /// <param name="customerId">The customers id.</param>
        /// /// <param name="debit">The amount to debit.</param>
        /// <returns></returns>
        /// <exception cref="NegativeDepositException">Cannot debit negative amount.</exception>
        Task DebitAsync(string customerId, decimal debit);
    }
}