using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Api.BusinessLogicLayer.DataTransferObjects;
using Api.BusinessLogicLayer.Interfaces;
using Api.BusinessLogicLayer.Requests;
using Api.BusinessLogicLayer.Responses;
using Api.BusinessLogicLayer.Services;
using Api.DataAccessLayer.Interfaces;
using Api.DataAccessLayer.Models;
using Api.DataAccessLayer.UnitOfWork;
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
        private IUoW _UoW;

        [SetUp]
        public void Setup()
        {
            _jwtService = Substitute.For<IJwtService>();
            _identityUserRepository = Substitute.For<IIdentityUserRepository>();
            _customerRepository = Substitute.For<ICustomerRepository>();
            _mapper = Substitute.For<IMapper>();
            _UoW = Substitute.For<IUoW>();
            _customerService = new CustomerService(_jwtService, _customerRepository, _identityUserRepository, _mapper, _UoW);
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

            _customerRepository.AddCustomerAsync(null, null).ReturnsForAnyArgs(customer);
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

            _customerRepository.AddCustomerAsync(null, null).ReturnsForAnyArgs<Customer>(customer);

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
            _identityUserRepository.SignInAsync(null, null).ReturnsForAnyArgs(SignInResult.Success);
            _jwtService.GenerateJwtToken(null, null, null).ReturnsForAnyArgs(token);

            var customer = new Customer
            {
                Id = "SomeId",
                Email = request.Email
            };

            _customerRepository.GetCustomerAsync(null).ReturnsForAnyArgs(customer);

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
            _identityUserRepository.SignInAsync(null, null).ReturnsForAnyArgs(SignInResult.Success);
            _jwtService.GenerateJwtToken(null, null, null).ReturnsForAnyArgs(token);

            var customer = new Customer
            {
                Id = "SomeId",
                Email = request.Email
            };

            _customerRepository.GetCustomerAsync(null).ReturnsForAnyArgs(customer);

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

        [Test]
        public void LoginCustomerAsync_EmailAndPasswordCombinationNotFound_ThrowsIdentityException()
        {
            var request = new LoginRequest
            {
                Email = "test@domain.com",
                Password = "Password1!"
            };

            var signinResult = SignInResult.Failed;
            _identityUserRepository.SignInAsync(null, null).ReturnsForAnyArgs(signinResult);

            Assert.That(() => _customerService.LoginCustomerAsync(request), Throws.TypeOf<IdentityException>());
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
            _customerRepository.GetRidesAsync(Arg.Any<string>()).ReturnsForAnyArgs(rideList);
            await _customerService.GetRidesAsync(null);
            _mapper.Received().Map<List<RideDto>>(rideList);
        }

        [Test]
        public async Task GetRidesAsync__NoRidesFromDatabase_ResponseContainsTheList()
        {
            List<Ride> rideList = new List<Ride>();
            _customerRepository.GetRidesAsync(Arg.Any<string>()).ReturnsForAnyArgs(rideList);

            List<RideDto> rideListDto = new List<RideDto>();
            _mapper.Map<List<RideDto>>(Arg.Any<List<Ride>>()).ReturnsForAnyArgs(rideListDto);

            var response = await _customerService.GetRidesAsync(null);
            Assert.That(response.Rides,Is.EqualTo(rideListDto));
            _mapper.Received().Map<List<RideDto>>(rideList);
        }


        #endregion

        #region DepositAsync


        [Test]
        public void DepositAsync_ReturnedNull_DoesNotThrow()
        {
            Assert.DoesNotThrowAsync(async () => await _customerService.GetRidesAsync(null));
        }

        #endregion

    }
}
