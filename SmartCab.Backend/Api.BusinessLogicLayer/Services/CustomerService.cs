using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Api.BusinessLogicLayer.DataTransferObjects;
using Api.BusinessLogicLayer.Interfaces;
using Api.BusinessLogicLayer.Requests;
using Api.BusinessLogicLayer.Responses;
using Api.DataAccessLayer.Interfaces;
using Api.DataAccessLayer.Models;
using Api.DataAccessLayer.UnitOfWork;
using Api.Responses;
using AutoMapper;
using CustomExceptions;
using Microsoft.AspNetCore.Identity;

namespace Api.BusinessLogicLayer.Services
{
    /// <summary>
    /// Exposes methods containing business logic related to customers.
    /// </summary>
    public class CustomerService : ICustomerService
    {
        private readonly IJwtService _jwtService;
        private readonly IMapper _mapper;
        private readonly IUoW _unitOfWork;

        /// <summary>
        /// Constructor for this class.
        /// </summary>
        /// <param name="jwtService">Used to generate Json Web Tokens</param>
        /// <param name="customerRepository">Used to access the database when updating/creating customers</param>
        /// <param name="identityUserRepository">Used to access the database when updating/creating customers</param>
        /// <param name="mapper">Mapper used to map between domain models and data transfer objects</param>
        public CustomerService(
            IJwtService jwtService,
            IMapper mapper, 
            IUoW unitOfWork)
        {
            _jwtService = jwtService;
            _mapper = mapper;
            _unitOfWork = unitOfWork;
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
            await _unitOfWork.IdentityUserRepository.AddIdentityUserAsync(customer, request.Password);
            await _unitOfWork.IdentityUserRepository.AddToRoleAsync(customer, nameof(Customer));
            

            //customer = await _customerRepository.AddCustomerAsync(customer, request.Password);
            var customerDto = _mapper.Map<CustomerDto>(customer);

            //Create the token, wrap it and return the response with the customerDto
            var token = _jwtService.GenerateJwtToken(customer.Id, customer.Email, nameof(Customer));
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
            var result = await _unitOfWork.IdentityUserRepository.SignInAsync(request.Email, request.Password);

            //Check if the logged in user is indeed a customer. If not this call will throw an ArgumentException
            var customer = _unitOfWork.CustomerRepository.FindOnlyOne(customerFilter => customerFilter.Email == request.Email);

            //All good, now generate the token and return it
            var token = _jwtService.GenerateJwtToken(customer.Id, request.Email, nameof(Customer));
            var customerDto = _mapper.Map<CustomerDto>(customer);
            var response = new LoginResponse
            {
                Token = token,
                Customer = customerDto
            };
            return response;

        }


        /// <summary>
        /// Deposits amount.
        /// </summary>
        /// <param name="request">The request containing the amount to deposit.</param>
        /// /// <param name="customerId">Id of the customer to deposit to.</param>
        /// <returns>A customer wrapped in a responseobject.</returns>
        public async Task DepositAsync(DepositRequest request, string customerId)
        {
            //Amount to deposit
            var depositAmount = request.Deposit;

            //Deposits
            await _unitOfWork.CustomerRepository.DepositAsync(customerId,depositAmount);
            _unitOfWork.SaveChanges();
            
        }


        /// <summary>
        /// Gets the rides associated to the customerId and wraps it into a CustomerRidesResponse object as RideDtos. 
        /// </summary>
        /// <param name="customerId">Id of the requesting customer</param>
        /// <returns></returns>
        public async Task<CustomerRidesResponse> GetRidesAsync(string customerId)
        {
            //var customerRides = _factory.UnitOfWork.GenericRideRepository.Find(ride => ride.CustomerId == customerId);
            var customerRides = _unitOfWork.CustomerRepository.FindByID(customerId).Rides;
            var customerRidesDto = _mapper.Map<List<RideDto>>(customerRides);
            var response = new CustomerRidesResponse
            {
                Rides = customerRidesDto
            };
            return response;
        }
    }
}
