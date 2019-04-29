using System;
using System.Linq;
using System.Threading.Tasks;
using Api.BusinessLogicLayer.DataTransferObjects;
using Api.BusinessLogicLayer.Interfaces;
using Api.BusinessLogicLayer.Requests;
using Api.BusinessLogicLayer.Responses;
using Api.DataAccessLayer.Interfaces;
using Api.DataAccessLayer.Models;
using Api.DataAccessLayer.UnitOfWork;
using AutoMapper;
using CustomExceptions;

namespace Api.BusinessLogicLayer.Services
{
    /// <summary>
    /// Exposes methods containing business logic related to taxi companies.
    /// </summary>
    public class TaxiCompanyService : ITaxiCompanyService
    {
        private readonly IUoW _unitOfWork;
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
            IMapper mapper,
            IJwtService jwtService,
            IUoW unitOfWork)
        {
            _mapper = mapper;
            _jwtService = jwtService;
            _unitOfWork = unitOfWork;
        }
        /// <summary>
        /// Adds a new taxi company to the database asynchronously and returns a JWT token wrapped in a response object.
        /// </summary>
        /// <remarks>
        /// The generated token will only give access to endpoints which are available to taxi companies.
        /// </remarks>
        /// <param name="request">The required data needed to create the taxi company</param>
        /// <returns>A RegisterResponseTaxiCompany object containing a valid JWT token</returns>
        public async Task<RegisterResponseTaxiCompany> AddTaxiCompanyAsync(RegisterRequest request)
        {
            // Create the taxi company
            var taxiCompany = new TaxiCompany
            {
                Name = request.Name,
                PhoneNumber = request.PhoneNumber,
                UserName = request.Email,
                Email = request.Email
            };

            // Overwrite the taxi company with the one created and create a TaxiCompanyDto
            await _unitOfWork.IdentityUserRepository.AddIdentityUserAsync(taxiCompany, request.Password);
            await _unitOfWork.IdentityUserRepository.AddToRoleAsync(taxiCompany, nameof(Customer));
            var taxiCompanyDto = _mapper.Map<TaxiCompanyDto>(taxiCompany);

            // Create the token, wrap it and return the response with the taxiCompanyDto
            var token = _jwtService.GenerateJwtToken(taxiCompany.Id, taxiCompany.Email, nameof(TaxiCompany));
            var response = new RegisterResponseTaxiCompany
            {
                Token = token,
                TaxiCompany = taxiCompanyDto
            };

            return response;
        }

        /// <summary>
        /// Attempts to log the taxi company in. Upon a successful login, a new JWT token is generated and returned.
        /// <remarks>
        /// The generated token will only give access to endpoints which is avaliable to taxi companies.
        /// </remarks>
        /// </summary>
        /// <param name="request">The email and password used to login.</param>
        /// <returns>A JWT token and certain information about the logged in taxi company.</returns>
        public async Task<LoginResponseTaxiCompany> LoginTaxiCompanyAsync(LoginRequest request)
        {
            // Check if it's possible to log in
            var result = await _unitOfWork.IdentityUserRepository.SignInAsync(request.Email, request.Password);

            // Check if the logged in taxi company is indeed a taxi company. If not, this call will throw an ArgumentException
            var taxiCompany = _unitOfWork.TaxiCompanyRepository.FindByEmail(request.Email);

            // Generate the token and return it
            var token = _jwtService.GenerateJwtToken(taxiCompany.Id, request.Email, nameof(TaxiCompany));
            var taxiCompanyDto = _mapper.Map<TaxiCompanyDto>(taxiCompany);
            var response = new LoginResponseTaxiCompany()
            {
                Token = token,
                TaxiCompany = taxiCompanyDto
            };
            return response;
            
        }
    }
}