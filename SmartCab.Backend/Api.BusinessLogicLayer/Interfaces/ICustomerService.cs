﻿using System.Threading.Tasks;
using Api.BusinessLogicLayer.Requests;
using Api.BusinessLogicLayer.Responses;
using Api.Responses;

namespace Api.BusinessLogicLayer.Interfaces
{
    /// <summary>
    /// Defines a number of methods related to a customer.
    /// </summary>
    public interface ICustomerService
    {
        Task<RegisterResponse> AddCustomerAsync(RegisterRequest request);
        Task<LoginResponse> LoginCustomerAsync(LoginRequest request);
        Task DepositAsync(DepositRequest request, string customerId);
        Task<CustomerRidesResponse> GetRidesAsync(string customerId);
        Task<EditCustomerResponse> EditCustomerAsync(EditCustomerRequest request, string customerId);
    }
}