﻿using System;
using System.Linq;
using System.Threading.Tasks;
using Api.BusinessLogicLayer.DataTransferObjects;
using Api.BusinessLogicLayer.Interfaces;
using Api.BusinessLogicLayer.Requests;
using Api.BusinessLogicLayer.Responses;
using Api.DataAccessLayer.Interfaces;
using Api.DataAccessLayer.Models;
using AutoMapper;
using CustomExceptions;

namespace Api.BusinessLogicLayer.Services
{
    /// <summary>
    /// Exposes methods containing business logic related to taxi companies.
    /// </summary>
    public class TaxiCompanyService : ITaxiCompanyService
    {
        private readonly IIdentityUserRepository _identityUserRepository;
        private readonly ITaxiCompanyRepository _taxiCompanyRepository;
        private readonly IMapper _mapper;
        private readonly IJwtService _jwtService;

        /// <summary>
        /// Constructor for this class.
        /// </summary>
        /// <param name="identityUserRepository">Used to access the database regarding a taxi company</param>
        /// <param name="taxiCompanyRepository">Used to access the database regarding a taxi company</param>
        /// <param name="mapper">Mapper used to map between domain models and data transfer objects</param>
        /// <param name="jwtService">Used to generate Json Web Tokens</param>
        public TaxiCompanyService(
            IIdentityUserRepository identityUserRepository,
            ITaxiCompanyRepository taxiCompanyRepository,
            IMapper mapper,
            IJwtService jwtService)
        {
            _identityUserRepository = identityUserRepository;
            _taxiCompanyRepository = taxiCompanyRepository;
            _mapper = mapper;
            _jwtService = jwtService;
        }

        /// <summary>
        /// Attempts to log the taxi company in. Upon a successful login, a new JWT token is generated and returned.
        /// <remarks>
        /// The generated token will only give access to endpoints which is avaliable to taxi companies.
        /// </remarks>
        /// </summary>
        /// <param name="request">The email and password used to login.</param>
        /// <returns>A JWT token and certain information about the logged in taxi company.</returns>
        public async Task<LoginResponse> LoginTaxiCompanyAsync(LoginRequest request)
        {
            // Check if it's possible to log in
            var result = await _identityUserRepository.SignInAsync(request.Email, request.Password);

            if (result.Succeeded)
            {
                // Check if the logged in taxi company is indeed a taxi company. If not, this call will throw an ArgumentException
                var taxiCompany = await _taxiCompanyRepository.GetTaxiCompanyAsync(request.Email);

                // Generate the token and return it
                var token = _jwtService.GenerateJwtToken(taxiCompany.Id, request.Email, nameof(TaxiCompany));
                var taxiCompanyDto = _mapper.Map<TaxiCompanyDto>(taxiCompany);
                var response = new LoginResponse
                {
                    Token = token,
                    TaxiCompany = taxiCompanyDto
                };
                return response;
            }
            // Log in failed
            throw new IdentityException("Login failed. Credentials were not found in the database.");
        }
    }
}