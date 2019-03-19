using System;
using System.Threading.Tasks;
using Api.BusinessLogicLayer.Interfaces;
using Api.BusinessLogicLayer.Requests;
using Api.DataAccessLayer;
using Api.DataAccessLayer.Interfaces;
using Api.DataAccessLayer.Models;
using Microsoft.AspNetCore.Identity;

namespace Api.BusinessLogicLayer.Services
{
    public class CustomerService : ICustomerService
    {
        private readonly IJwtService _jwtService;
        private readonly IApplicationUserRepository _applicationUserRepository;
        private readonly ICustomerRepository _customerRepository;

        public CustomerService(
            IJwtService jwtService, 
            ICustomerRepository customerRepository, 
            IApplicationUserRepository applicationUserRepository)
        {
            _jwtService = jwtService;
            _customerRepository = customerRepository;
            _applicationUserRepository = applicationUserRepository;
        }

        public async Task<string> AddCustomerAsync(RegisterRequest request)
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
                return token;
            }

            throw new ApplicationException("Unexpected error");
        }
    }
}
