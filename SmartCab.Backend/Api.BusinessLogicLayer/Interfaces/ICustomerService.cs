using System.Threading.Tasks;
using Api.BusinessLogicLayer.Requests;
using Api.BusinessLogicLayer.Responses;
using Api.DataAccessLayer.Models;
using Microsoft.AspNetCore.Identity;

namespace Api.BusinessLogicLayer.Interfaces
{
    /// <summary>
    /// Defines a number of methods related to a customer.
    /// </summary>
    public interface ICustomerService
    {
        Task<RegisterResponse> AddCustomerAsync(RegisterRequest request);
        Task<LoginResponse> LoginCustomerAsync(LoginRequest request);

        /// <summary>
        /// Deposits amount.
        /// </summary>
        /// <param name="request">The request containing the amount to deposit.</param>
        /// /// <param name="customerId">Id of the customer to deposit to.</param>
        /// <returns>A customer wrapped in a responseobject.</returns>
        Task DepositAsync(DepositRequest request, string customerId);
    }
}