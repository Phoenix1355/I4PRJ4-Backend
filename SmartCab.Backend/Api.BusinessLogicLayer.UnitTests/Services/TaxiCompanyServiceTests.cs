using System;
using System.Threading.Tasks;
using Api.BusinessLogicLayer.DataTransferObjects;
using Api.BusinessLogicLayer.Interfaces;
using Api.BusinessLogicLayer.Requests;
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
    public class TaxiCompanyServiceTests
    {
        #region Setup

        private IIdentityUserRepository _identityUserRepository;
        private IJwtService _jwtService;
        private ITaxiCompanyRepository _taxiCompanyRepository;
        private IMapper _mapper;
        private TaxiCompanyService _taxiCompanyService;

        [SetUp]
        public void Setup()
        {
            _identityUserRepository = Substitute.For<IIdentityUserRepository>();
            _jwtService = Substitute.For<IJwtService>();
            _taxiCompanyRepository = Substitute.For<ITaxiCompanyRepository>();
            _mapper = Substitute.For<IMapper>();
            var _unitofWork = Substitute.For<IUoW>();
            _taxiCompanyService = new TaxiCompanyService(_mapper, _jwtService, _unitofWork);
        }

        #endregion
        /*
        #region AddTaxiCompanyAsync
        
        [Test]
        public async Task AddTaxiCompanyAsync_AddingTaxiCompanySucceeds_ReturnsARegisterResponseTaxiCompanyThatContainsTheExpectedToken()
        {
            var request = new RegisterRequest
            {
                Email = "test@domain.com",
                Name = "Name",
                Password = "Qwer111!",
                PasswordRepeated = "Qwer111!",
                PhoneNumber = "12345678"
            };

            var taxiCompany = new TaxiCompany
            {
                Email = request.Email,
                Name = request.Name,
                PhoneNumber = request.PhoneNumber
            };

            _taxiCompanyRepository.AddTaxiCompanyAsync(null, null).ReturnsForAnyArgs(taxiCompany);
            _jwtService.GenerateJwtToken(null, null, null).ReturnsForAnyArgs("TheGeneratedToken");

            var response = await _taxiCompanyService.AddTaxiCompanyAsync(request);

            Assert.That(response.Token, Is.EqualTo("TheGeneratedToken"));
        }

        [Test]
        public async Task AddTaxiCompanyAsync_AddingTaxiCompanySucceeds_ReturnsARegisterResponseTaxiCompanyThatContainsTheExpectedTaxiCompanyDto()
        {
            var request = new RegisterRequest
            {
                Email = "test@domain.com",
                Name = "Name",
                Password = "Qwer111!",
                PasswordRepeated = "Qwer111!",
                PhoneNumber = "12345678"
            };

            var taxiCompany = new TaxiCompany
            {
                Email = request.Email,
                Name = request.Name,
                PhoneNumber = request.PhoneNumber
            };

            _taxiCompanyRepository.AddTaxiCompanyAsync(null, null).ReturnsForAnyArgs<TaxiCompany>(taxiCompany);

            var taxiCompanyDto = new TaxiCompanyDto
            {
                Email = request.Email,
                Name = request.Name,
                PhoneNumber = request.PhoneNumber
            };

            _mapper.Map<TaxiCompanyDto>(null).ReturnsForAnyArgs(taxiCompanyDto);

            var response = await _taxiCompanyService.AddTaxiCompanyAsync(request);

            Assert.That(response.TaxiCompany, Is.EqualTo(taxiCompanyDto));
        }

        #endregion

        #region LoginTaxiCompanyAsync

        [Test]
        public async Task LoginTaxiCompanyAsync_LoginSucceeds_ReturnsLoginResponseThatContainsTheExpectedToken()
        {
            // Arrange
            var request = new LoginRequest
            {
                Email = "test@domain.com",
                Password = "Password1!"
            };

            var token = "Token";
            _identityUserRepository.SignInAsync(null, null).ReturnsForAnyArgs(SignInResult.Success);
            _jwtService.GenerateJwtToken(null, null, null).ReturnsForAnyArgs(token);

            var taxiCompany = new TaxiCompany
            {
                Id = "Some Id",
                Email = request.Email
            };

            _taxiCompanyRepository.GetTaxiCompanyAsync(null).ReturnsForAnyArgs(taxiCompany);

            var taxiCompanyDto = new TaxiCompanyDto
            {
                Email = request.Email,
                Name = "Some Name",
                PhoneNumber = "12345678"
            };

            _mapper.Map<TaxiCompanyDto>(null).ReturnsForAnyArgs(taxiCompanyDto);

            // Act
            var response = await _taxiCompanyService.LoginTaxiCompanyAsync(request);

            // Assert
            Assert.That(response.Token, Is.EqualTo(token));
        }

        [Test]
        public async Task LoginTaxiCompanyAsync_LoginSucceeds_ReturnsLoginResponseThatContainsTheExpectedTaxiCompanyDto()
        {
            // Arrange
            var request = new LoginRequest
            {
                Email = "test@domain.com",
                Password = "Password1!"
            };

            var token = "Token";
            _identityUserRepository.SignInAsync(null, null).ReturnsForAnyArgs(SignInResult.Success);
            _jwtService.GenerateJwtToken(null, null, null).ReturnsForAnyArgs(token);

            var taxiCompany = new TaxiCompany
            {
                Id = "Some Id",
                Email = request.Email
            };

            _taxiCompanyRepository.GetTaxiCompanyAsync(null).ReturnsForAnyArgs(taxiCompany);

            var taxiCompanyDto = new TaxiCompanyDto
            {
                Email = request.Email,
                Name = "Some Name",
                PhoneNumber = "12345678"
            };

            _mapper.Map<TaxiCompanyDto>(null).ReturnsForAnyArgs(taxiCompanyDto);

            // Act
            var response = await _taxiCompanyService.LoginTaxiCompanyAsync(request);

            // Assert
            Assert.That(response.TaxiCompany, Is.EqualTo(taxiCompanyDto));
        }

        [Test]
        public void LoginTaxiCompanyAsync_EmailAndPasswordCombinationNotFound_ThrowsIdentityException()
        {
            // Assert
            var request = new LoginRequest
            {
                Email = "test@domain.com",
                Password = "Password1!"
            };

            // Act
            var signInResult = SignInResult.Failed;
            _identityUserRepository.SignInAsync(null, null).ReturnsForAnyArgs(signInResult);

            //Assert
            Assert.That(() => _taxiCompanyService.LoginTaxiCompanyAsync(request), Throws.TypeOf<IdentityException>());
        }

        #endregion
    */
    }
}