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
        /// <remarks>
        /// The generated token will only give access to endpoints which is available to customers.
        /// </remarks>
        /// <param name="request">The required data needed to create the customer</param>
        /// <returns>A RegisterResponse object containing a valid JWT token</returns>
        public async Task<RegisterResponse> AddCustomerAsync(RegisterRequest request)
        {

            //Create the customer
            var customer = new Customer
            {
                Name = request.Name,
                PhoneNumber = request.PhoneNumber,
                UserName = request.Email,
                Email = request.Email
            };

            //Overwrite the customer with the one created and create a CustomerDto
            customer = await _customerRepository.AddCustomerAsync(customer, request.Password);
            var customerDto = _mapper.Map<CustomerDto>(customer);

            //Create the token, wrap it and return the response with the customerDto
            var token = _jwtService.GenerateJwtToken(request.Email, nameof(Customer));
            var response = new RegisterResponse
            {
                Token = token,
                Customer = customerDto
            };

            return response;
        }

        /// <summary>
        /// Attempts to log the user in. Upon a successful login a new JWT token is generated and returned.
        /// <remarks>
        /// The generated token will only give access to endpoints which is available to customers.
        /// </remarks>
        /// </summary>
        /// <param name="request">The email and password used to log in.</param>
        /// <returns>A JWT token and certain information about the logged in user.</returns>
        public async Task<LoginResponse> LoginCustomerAsync(LoginRequest request)
        {
            //Check if its possible to log in
            var result = await _applicationUserRepository.SignInAsync(request.Email, request.Password);

            if (result.Succeeded)
            {
                //Check if the logged in user is indeed a customer. If not this call will throw an ArgumentException
                var customer = await _customerRepository.GetCustomerAsync(request.Email);

                //All good, now generate the token and return it
                var token = _jwtService.GenerateJwtToken(request.Email, nameof(Customer));
                var customerDto = _mapper.Map<CustomerDto>(customer);
                var response = new LoginResponse
                {
                    Token = token,
                    Customer = customerDto
                };
                return response;
            }

            //Log in failed
            throw new ArgumentException("Login failed. Credentials was not found in the database.");
        }
    }
}
