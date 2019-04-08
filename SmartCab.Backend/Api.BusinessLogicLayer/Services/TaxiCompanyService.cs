using System;
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

        public async Task<LoginResponse> LoginTaxiCompanyAsync(LoginRequest request)
        {
            // Check if it's possible to log in
            var result = await _identityUserRepository.SignInAsync(request.Email, request.Password);

            if (result.Succeeded)
            {
                // Check if the logged in taxi company is indeed a taxi company. If not, this call will throw an ArgumentException
                var taxiCompany = await _taxiCompanyRepository.GetTaxiCompanyAsync(request.Email);

                // Generate the token and return it
                var token = 
                var taxiCompanyDto = _mapper.Map<TaxiCompanyDto>(taxiCompany);
            }
        }
    }
}