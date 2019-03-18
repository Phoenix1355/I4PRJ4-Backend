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
        private readonly IIdentityService _identityService;
        private readonly ICustomerRepository _customerRepository;

        public CustomerService(
            IJwtService jwtService, 
            IIdentityService identityService, 
            ICustomerRepository customerRepository)
        {
            _jwtService = jwtService;
            _identityService = identityService;
            _customerRepository = customerRepository;
        }

        public async Task<string> AddCustomerAsync(RegisterRequest request)
        {
            var user = new ApplicationUser
            {
                UserName = request.Email,
                Email = request.Email
            };

            var result = await _identityService.AddApplicationUserAsync(user, request.Password);

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
                await _identityService.AddToRoleAsync(user, "Customer");

                var token = await _jwtService.GetJwtToken(request.Email, user);
                return token;
            }

            throw new ApplicationException("Unexpected error");
        }
    }
}
