using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Api.BusinessLogicLayer.DataTransferObjects;
using Api.BusinessLogicLayer.Interfaces;
using Api.BusinessLogicLayer.Requests;
using Api.BusinessLogicLayer.Responses;
using Api.BusinessLogicLayer.Services;
using Api.DataAccessLayer.Interfaces;
using Api.DataAccessLayer.Models;
using Api.DataAccessLayer.UnitOfWork;
using Api.Requests;
using AutoMapper;
using CustomExceptions;
using Microsoft.AspNetCore.Identity;
using NSubstitute;
using NSubstitute.ExceptionExtensions;
using NUnit.Framework;

namespace Api.BusinessLogicLayer.UnitTests.Services
{
    [TestFixture]
    public class CustomerServiceTests
    {
        #region Setup and fields

        private IJwtService _jwtService;
        private IIdentityUserRepository _identityUserRepository;
        private ICustomerRepository _customerRepository;
        private IMapper _mapper;
        private CustomerService _customerService;
        private IUnitOfWork _unitOfWork;

        [SetUp]
        public void Setup()
        {
            _jwtService = Substitute.For<IJwtService>();
            _identityUserRepository = Substitute.For<IIdentityUserRepository>();
            _customerRepository = Substitute.For<ICustomerRepository>();
            _mapper = Substitute.For<IMapper>();
            _unitOfWork = Substitute.For<IUnitOfWork>();
            _customerService = new CustomerService(_jwtService, _mapper, _unitOfWork);
        }

        #endregion
        
        #region AddCustomerAsync

        [Test]
        public async Task AddCustomerAsync_AddingCustomerSucceeds_ReturnsAResisterResponseThatContainsTheExpectedToken()
        {
            var request = new RegisterRequest
            {
                Email = "m@gmail.com",
                Name = "Michael",
                Password = "Qwer111!",
                PasswordRepeated = "Qwer111!",
                PhoneNumber = "12345678"
            };

            var customer = new Customer
            {
                Email = request.Email,
                Name = request.Name,
                PhoneNumber = request.PhoneNumber
            };

            _unitOfWork.CustomerRepository.Add( null).ReturnsForAnyArgs(customer);
            _jwtService.GenerateJwtToken(null, null, null).ReturnsForAnyArgs("TheGeneratedToken");

            var response = await _customerService.AddCustomerAsync(request);

            Assert.That(response.Token, Is.EqualTo("TheGeneratedToken"));
        }

        [Test]
        public async Task AddCustomerAsync_AddingCustomerSucceeds_ReturnsAResisterResponseThatContainsTheExpectedCustomerDto()
        {
            var request = new RegisterRequest
            {
                Email = "m@gmail.com",
                Name = "Michael",
                Password = "Qwer111!",
                PasswordRepeated = "Qwer111!",
                PhoneNumber = "12345678"
            };

            var customer = new Customer
            {
                Email = request.Email,
                Name = request.Name,
                PhoneNumber = request.PhoneNumber
            };

            _unitOfWork.CustomerRepository.FindOnlyOneAsync(Arg.Any<Expression< Func<Customer, bool> >> ()).ReturnsForAnyArgs<Customer>(customer);

            var customerDto = new CustomerDto
            {
                Email = request.Email,
                Name = request.Name,
                PhoneNumber = request.PhoneNumber
            };

            _mapper.Map<CustomerDto>(null).ReturnsForAnyArgs(customerDto);

            var response = await _customerService.AddCustomerAsync(request);

            Assert.That(response.Customer, Is.EqualTo(customerDto));
        }

        #endregion

        #region LoginCustomerAsync

        [Test]
        public async Task LoginCustomerAsync_LoginSucceeds_ReturnsLoginResponseThatContainsTheExpectedToken()
        {
            //Arrange
            var request = new LoginRequest
            {
                Email = "test@domain.com",
                Password = "Password1!"
            };

            var token = "Token";
            _jwtService.GenerateJwtToken(null, null, null).ReturnsForAnyArgs(token);

            var customer = new Customer
            {
                Id = "SomeId",
                Email = request.Email
            };

            _unitOfWork.CustomerRepository.FindByEmailAsync(null).ReturnsForAnyArgs<Customer>(customer);

            var customerDto = new CustomerDto
            {
                Email = request.Email,
                Name = "Some Name",
                PhoneNumber = "12345678"
            };
            _mapper.Map<CustomerDto>(null).ReturnsForAnyArgs(customerDto);

            //Act
            var response = await _customerService.LoginCustomerAsync(request);

            //Assert
            Assert.That(response.Token, Is.EqualTo(token));
        }

        [Test]
        public async Task LoginCustomerAsync_LoginSucceeds_ReturnsLoginResponseThatContainsTheExpectedCustomerDto()
        {
            //Arrange
            var request = new LoginRequest
            {
                Email = "test@domain.com",
                Password = "Password1!"
            };

            var token = "Token";
            _jwtService.GenerateJwtToken(null, null, null).ReturnsForAnyArgs(token);

            var customer = new Customer
            {
                Id = "SomeId",
                Email = request.Email
            };

            _unitOfWork.CustomerRepository.FindByEmailAsync(null).ReturnsForAnyArgs<Customer>(customer);

            var customerDto = new CustomerDto
            {
                Email = request.Email,
                Name = "Some Name",
                PhoneNumber = "12345678"
            };
            _mapper.Map<CustomerDto>(null).ReturnsForAnyArgs(customerDto);

            //Act
            var response = await _customerService.LoginCustomerAsync(request);

            //Assert
            Assert.That(response.Customer, Is.EqualTo(customerDto));
        }


        #endregion

        #region GetRidesAsync

        [Test]
        public async Task GetRidesAsync_NoRidesFromDatabase_DoesNotThrow()
        {
            Assert.DoesNotThrowAsync(async () =>  await _customerService.GetRidesAsync(null)
        );
        }

        [Test]
        public async Task GetRidesAsync__NoRidesFromDatabase_MapsTheExpectedType()
        {
            await _customerService.GetRidesAsync(null);
            _mapper.Received().Map<List<RideDto>>(Arg.Any<List<Ride>>());
        }

        [Test]
        public async Task GetRidesAsync__NoRidesFromDatabase_ReceivesExpectedInput()
        {
            List<Ride> rideList = new List<Ride>();
            _unitOfWork.CustomerRepository.FindCustomerRidesAsync(null).ReturnsForAnyArgs<List<Ride>>(rideList);
            await _customerService.GetRidesAsync(null);
            _mapper.Received().Map<List<RideDto>>(rideList);
        }

        [Test]
        public async Task GetRidesAsync__NoRidesFromDatabase_ResponseContainsTheList()
        {
            List<Ride> rideList = new List<Ride>();

            _unitOfWork.CustomerRepository.FindCustomerRidesAsync(null).ReturnsForAnyArgs<List<Ride>>(rideList);

            List<RideDto> rideListDto = new List<RideDto>();
            _mapper.Map<List<RideDto>>(Arg.Any<List<Ride>>()).ReturnsForAnyArgs(rideListDto);

            var response = await _customerService.GetRidesAsync(null);
            Assert.That(response.Rides,Is.EqualTo(rideListDto));
            _mapper.Received().Map<List<RideDto>>(rideList);
        }


        #endregion

        #region EditCustomerAsync
        
        [Test]
        public async Task EditCustomerAsync_EditingCustomerSucceeds__ReturnsAEditCustomerResponse()
        {
            //Arrange
            var request = new EditCustomerRequest
            {
                Email = "test@domain.com",
                Name = "Axel",
                Password = "Qwer111!",
                RepeatedPassword = "Qwer111!",
                PhoneNumber = "66666666",
                OldPassword = "Qwerrr111!",
                ChangePassword = true
            };

            var customer = new Customer
            {
                Id = "SomeId",
                Email = "test@gmail.com",
                PhoneNumber = "11111111",
                Name = "Hans"
            };

            _unitOfWork.IdentityUserRepository.AddIdentityUserAsync(customer, request.OldPassword)
                .ReturnsForAnyArgs(IdentityResult.Success);

            _unitOfWork.IdentityUserRepository.SignInAsync(customer.Email, request.OldPassword)
                .ReturnsForAnyArgs(SignInResult.Success);
            
            var customerDto = new CustomerDto
            {
                Email = request.Email,
                Name = request.Name,
                PhoneNumber = request.PhoneNumber
            };

            var editCustomerResponse = new EditCustomerResponse
            {
                Customer = customerDto
            };

            _unitOfWork.CustomerRepository.FindByIDAsync(null).ReturnsForAnyArgs(customer);
            _mapper.Map<CustomerDto>(null).ReturnsForAnyArgs(customerDto);

            //Act
            var response = await _customerService.EditCustomerAsync(request, customer.Id);

            //Assert
            Assert.That(response.Customer, Is.EqualTo(editCustomerResponse.Customer));
        }

        #endregion

        #region DepositAsync

        [Test]
        public void DepositAsync_ReturnedNull_DoesNotThrow()
        {
            Console.WriteLine("hej");
            Assert.DoesNotThrowAsync(async () => await _customerService.GetRidesAsync(null));
        }

        #endregion
    }
}
