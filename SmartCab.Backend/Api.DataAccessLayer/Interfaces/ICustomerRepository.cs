using System.Collections.Generic;
using System.Threading.Tasks;
using Api.DataAccessLayer.Models;
using CustomExceptions;
using Microsoft.AspNetCore.Identity;

namespace Api.DataAccessLayer.Interfaces
{
    /// <summary>
    /// Interface for CustomerRepository, containing relevant methods. 
    /// </summary>
    public interface ICustomerRepository
    {
        Task<Customer> AddCustomerAsync(Customer customer, string password);
        Task<Customer> GetCustomerAsync(string email);
        Task DepositAsync(string customerId, decimal deposit);
        Task<List<Ride>> GetRidesAsync(string customerId);
        Task<Customer> EditCustomerAsync(Customer newCustomer, string customerId, string password, string oldPassword);
    }
}