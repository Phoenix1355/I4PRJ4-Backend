using System;
using System.Linq;
using System.Threading.Tasks;
using Api.BusinessLogicLayer.Interfaces;
using Api.BusinessLogicLayer.Requests;
using Api.BusinessLogicLayer.Responses;
using Api.DataAccessLayer.Interfaces;
using Api.DataAccessLayer.Models;

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

        /// <summary>
        /// Constructor for this class.
        /// </summary>
        /// <param name="jwtService">Used to generate Json Web Tokens</param>
        /// <param name="customerRepository">Used to access the database when updating/creating customers</param>
        /// <param name="applicationUserRepository">Used to access the database when updating/creating customers</param>
        public CustomerService(
            IJwtService jwtService, 
            ICustomerRepository customerRepository, 
            IApplicationUserRepository applicationUserRepository)
        {
            _jwtService = jwtService;
            _customerRepository = customerRepository;
            _applicationUserRepository = applicationUserRepository;
        }

        /// <summary>
        /// Adds a new customer to the database asynchronously and returns a JWT token wrapped in a response object.
        /// </summary>
        /// <param name="request">The required data needed to create the customer</param>
        /// <returns>A RegisterResponse object containing a valid JWT token</returns>
        public async Task<RegisterResponse> AddCustomerAsync(RegisterRequest request)
        {
            var user = new ApplicationUser
            {
                UserName = request.Email,
                Email = request.Email
            };

            var result = await _applicationUserRepository.AddApplicationUserAsync(user, request.Password);

            if (result.Succeeded)
            {
                var customer = new Customer
                {
                    ApplicationUserId = user.Id,
                    Name = request.Name,
                    PhoneNumber = request.PhoneNumber,
                    Email = request.Email
                };

                await _customerRepository.AddCustomerAsync(customer);
                await _applicationUserRepository.AddToRoleAsync(user, "Customer");

                var token = await _jwtService.GenerateJwtToken(request.Email, user);
                return new RegisterResponse {Token = token};
            }

            throw new ArgumentException(result.Errors.First().Description);
        }
    }
}
