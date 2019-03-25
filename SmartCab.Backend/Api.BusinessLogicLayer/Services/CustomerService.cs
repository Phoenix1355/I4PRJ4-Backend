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

namespace Api.BusinessLogicLayer.Services
{
    /// <summary>
    /// Exposes methods containing business logic related to customers.
    /// </summary>
    public class CustomerService : ICustomerService
    {
        private readonly IJwtService _jwtService;
        private readonly IApplicationUserRepository _applicationUserRepository;
        private readonly ICustomerRepository _customerRepository;
        private readonly IMapper _mapper;

        /// <summary>
        /// Constructor for this class.
        /// </summary>
        /// <param name="jwtService">Used to generate Json Web Tokens</param>
        /// <param name="customerRepository">Used to access the database when updating/creating customers</param>
        /// <param name="applicationUserRepository">Used to access the database when updating/creating customers</param>
        /// <param name="mapper">Mapper used to map between domain models and data transfer objects</param>
        public CustomerService(
            IJwtService jwtService, 
            ICustomerRepository customerRepository, 
            IApplicationUserRepository applicationUserRepository, 
            IMapper mapper)
        {
            _jwtService = jwtService;
            _customerRepository = customerRepository;
            _applicationUserRepository = applicationUserRepository;
            _mapper = mapper;
        }

        /// <summary>
        /// Adds a new customer to the database asynchronously and returns a JWT token wrapped in a response object.
        /// </summary>
        /// <param name="request">The required data needed to create the customer</param>
        /// <returns>A RegisterResponse object containing a valid JWT token</returns>
        public async Task<RegisterResponse> AddCustomerAsync(RegisterRequest request)
        {
            //Create the identity user
            var user = new ApplicationUser
            {
                UserName = request.Email,
                Email = request.Email
            };

            //Create the customer
            var customer = new Customer
            {
                ApplicationUserId = user.Id,
                Name = request.Name,
                PhoneNumber = request.PhoneNumber,
            };

            //Overwrite the customer with the one created and create a CustomerDto
            customer = await _customerRepository.AddCustomerAsync(user, customer, request.Password);
            var customerDto = _mapper.Map<CustomerDto>(customer);

            //Create the token, wrap it and return the response with the customerDto
            var token = _jwtService.GenerateJwtToken(request.Email, "Customer");
            var response = new RegisterResponse
            {
                Token = token,
                Customer = customerDto
            };

            return response;
        }

        public async Task<LoginResponse> LoginCustomerAsync(LoginRequest request)
        {
            var result = await _applicationUserRepository.SignInAsync(request.Email, request.Password);

            if (result.Succeeded)
            {
                var customer = await _customerRepository.GetCustomerAsync(request.Email);

                var token = _jwtService.GenerateJwtToken(request.Email, "Customer");
                var customerDto = _mapper.Map<CustomerDto>(customer);
                var response = new LoginResponse
                {
                    Token = token,
                    Customer = customerDto
                };
                return response;
            }

            throw new ArgumentException("Login failed. Credentials was not found in the database.");
        }
    }
}
