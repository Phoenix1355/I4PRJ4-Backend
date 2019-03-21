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
            //Create the identity user and try to add the user to the database
            var user = new ApplicationUser
            {
                UserName = request.Email,
                Email = request.Email
            };

            var result = await _applicationUserRepository.AddApplicationUserAsync(user, request.Password);

            //If the identity user was successfully added, then create the customer object and assign a role to it
            if (result.Succeeded)
            {
                Customer customer = new Customer
                {
                    ApplicationUserId = user.Id,
                    Name = request.Name,
                    PhoneNumber = request.PhoneNumber,
                    Email = request.Email
                };

                var role = "Customer";
                customer = await _customerRepository.AddCustomerAsync(customer);

                await _applicationUserRepository.AddToRoleAsync(user, role);

                //Create the token, wrap it and return the response
                var customerDto = _mapper.Map<CustomerDto>(customer);
                var token = _jwtService.GenerateJwtToken(request.Email, user, role);
                var response = new RegisterResponse
                {
                    Token = token,
                    Customer = customerDto
                };
                return response;
            }

            //If the identity user was not successfully added, then throw an error containing the error message from the identity framework
            throw new ArgumentException(result.Errors.First().Description);
        }

        public async Task<LoginResponse> LoginCustomerAsync(LoginRequest request)
        {
            var result = await _applicationUserRepository.SignInAsync(request.Email, request.Password);

            if (result.Succeeded)
            {
                var customer = await _customerRepository.GetCustomerAsync(request.Email);

                var token = _jwtService.GenerateJwtToken(request.Email, customer.ApplicationUser, "Customer");
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
