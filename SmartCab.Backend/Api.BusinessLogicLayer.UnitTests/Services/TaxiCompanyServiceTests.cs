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

        private IJwtService _jwtService;
        private IUoW _unitofWork;
        private IMapper _mapper;
        private TaxiCompanyService _taxiCompanyService;

        [SetUp]
        public void Setup()
        {
            _jwtService = Substitute.For<IJwtService>();
            _mapper = Substitute.For<IMapper>();
            _unitofWork = Substitute.For<IUoW>();
            _taxiCompanyService = new TaxiCompanyService(_mapper, _jwtService, _unitofWork);
        }

        #endregion
        
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
            
            _unitofWork.TaxiCompanyRepository.AddAsync( null).ReturnsForAnyArgs(taxiCompany);
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

            _unitofWork.TaxiCompanyRepository.AddAsync(null).ReturnsForAnyArgs(taxiCompany);

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
            _unitofWork.IdentityUserRepository.SignInAsync(null, null).ReturnsForAnyArgs(SignInResult.Success);
            _jwtService.GenerateJwtToken(null, null, null).ReturnsForAnyArgs(token);

            var taxiCompany = new TaxiCompany
            {
                Id = "Some Id",
                Email = request.Email
            };

            _unitofWork.TaxiCompanyRepository.FindByEmail(null).ReturnsForAnyArgs(taxiCompany);

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
            _unitofWork.IdentityUserRepository.SignInAsync(null, null).ReturnsForAnyArgs(SignInResult.Success);
            _jwtService.GenerateJwtToken(null, null, null).ReturnsForAnyArgs(token);

            var taxiCompany = new TaxiCompany
            {
                Id = "Some Id",
                Email = request.Email
            };

            _unitofWork.TaxiCompanyRepository.FindByEmail(null).ReturnsForAnyArgs(taxiCompany);

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
        #endregion
    
    }
}