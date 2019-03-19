﻿using System.Threading.Tasks;
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
    }
}