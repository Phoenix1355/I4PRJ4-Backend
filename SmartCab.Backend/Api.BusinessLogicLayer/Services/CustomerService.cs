using System.Collections.Generic;
using System.Threading.Tasks;
using Api.BusinessLogicLayer.DataTransferObjects;
using Api.BusinessLogicLayer.Interfaces;
using Api.BusinessLogicLayer.Requests;
using Api.BusinessLogicLayer.Responses;
using Api.DataAccessLayer.Models;
using Api.DataAccessLayer.UnitOfWork;
using Api.Requests;
using Api.Responses;
using AutoMapper;

namespace Api.BusinessLogicLayer.Services
{
    /// <summary>
    /// Exposes methods containing business logic related to customers.
    /// </summary>
    public class CustomerService : ICustomerService
    {
        private readonly IJwtService _jwtService;
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _unitOfWork;

        /// <summary>
        /// Constructor for this class.
        /// </summary>
        /// <param name="jwtService">Used to generate Json Web Tokens</param>
        /// <param name="unitOfWork">Used to access the database repositories</param>
        /// <param name="mapper">Mapper used to map between domain models and data transfer objects</param>
        public CustomerService(
            IJwtService jwtService,
            IMapper mapper, 
            IUnitOfWork unitOfWork)
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
            await _unitOfWork.IdentityUserRepository.TransactionWrapper(async () =>
            {
                await _unitOfWork.IdentityUserRepository.AddIdentityUserAsync(customer, request.Password);
                await _unitOfWork.IdentityUserRepository.AddToRoleAsync(customer, nameof(Customer));
            });
            
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
            //Check if its possible to log in. If not an identity exception will be thrown
            await _unitOfWork.IdentityUserRepository.SignInAsync(request.Email, request.Password);

            //Check if the logged in user is indeed a customer. If not this call will throw an UserIdInvalidException
            var customer = await _unitOfWork.CustomerRepository.FindByEmailAsync(request.Email);

            //All good, now generate the token and return it with a customerDto
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
        /// Updates the name, email, password and phone number of the customer with the specified id.
        /// </summary>
        /// <param name="request">The request containing the information that should be updated for the customer.</param>
        /// <param name="customerId">The id of the customer the changes should be applied to.</param>
        /// <returns></returns>
        public async Task<EditCustomerResponse> EditCustomerAsync(EditCustomerRequest request, string customerId)
        {
            var customer = await _unitOfWork.CustomerRepository.FindByIDAsync(customerId);
            
            await _unitOfWork.IdentityUserRepository.TransactionWrapper(async () =>
            {
                customer.Name = request.Name;
                customer.PhoneNumber = request.PhoneNumber;

                if (request.ChangePassword == true)
                {
                    var password = request.Password;
                    var oldPassword = request.OldPassword;
                    await _unitOfWork.IdentityUserRepository.ChangePasswordAsync(customer, password, oldPassword);
                }

                if (customer.Email != request.Email)
                    await _unitOfWork.IdentityUserRepository.ChangeEmailAsync(customer, request.Email);

                _unitOfWork.CustomerRepository.Update(customer);
                await _unitOfWork.SaveChangesAsync();
            });
            
            var customerDto = _mapper.Map<CustomerDto>(customer);
            var response = new EditCustomerResponse
            {
                Customer = customerDto
            };
            return response;
        }

        /// <summary>
        /// Deposits an amount to the a customers account.
        /// </summary>
        /// <param name="request">The request containing the amount to deposit.</param>
        /// <param name="customerId">Id of the customer to deposit to.</param>
        /// <returns>A customer wrapped in a response object.</returns>
        public async Task DepositAsync(DepositRequest request, string customerId)
        {
            //Amount to deposit
            var depositAmount = request.Deposit;

            //Deposits
            await _unitOfWork.CustomerRepository.DepositAsync(customerId, depositAmount);
            await _unitOfWork.SaveChangesAsync();
        }

        /// <summary>
        /// Gets the rides associated to the customerId and wraps it into a CustomerRidesResponse object as RideDtos. 
        /// </summary>
        /// <param name="customerId">Id of the requesting customer</param>
        /// <returns></returns>
        public async Task<CustomerRidesResponse> GetRidesAsync(string customerId)
        {
            var customerRides = await _unitOfWork.CustomerRepository.FindCustomerRidesAsync(customerId);

            var customerRidesDto = _mapper.Map<List<RideDto>>(customerRides);
            var response = new CustomerRidesResponse
            {
                Rides = customerRidesDto
            };
            return response;
        }
    }
}
