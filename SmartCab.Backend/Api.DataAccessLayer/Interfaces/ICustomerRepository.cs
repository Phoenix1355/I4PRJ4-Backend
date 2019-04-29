using System.Collections.Generic;
using System.Threading.Tasks;
using Api.DataAccessLayer.Models;
using Api.DataAccessLayer.Repositories;
using CustomExceptions;
using Microsoft.AspNetCore.Identity;

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
        Task<List<Ride>> GetRidesAsync(string customerId);
        Task<Customer> EditCustomerAsync(Customer newCustomer, string customerId, string password, string oldPassword);
    }
}