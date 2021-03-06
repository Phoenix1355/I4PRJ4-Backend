﻿using System.Threading.Tasks;
using Api.BusinessLogicLayer.Requests;
using Api.BusinessLogicLayer.Responses;

namespace Api.BusinessLogicLayer.Interfaces
{
    /// <summary>
    /// Defines a number of methods related to a taxi company
    /// </summary>
    public interface ITaxiCompanyService
    {
        Task<RegisterResponseTaxiCompany> AddTaxiCompanyAsync(RegisterRequest request);
        Task<LoginResponseTaxiCompany> LoginTaxiCompanyAsync(LoginRequest request);
    }
}