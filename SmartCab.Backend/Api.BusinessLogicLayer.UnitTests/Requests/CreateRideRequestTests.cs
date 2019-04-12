using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Api.BusinessLogicLayer.Requests;
using Api.DataAccessLayer.Models;
using NUnit.Framework;

namespace Api.BusinessLogicLayer.UnitTests.Requests
{
    [TestFixture]
    public class CreateRideRequestTests
    {
        private readonly DateTime _validDepartureTime = DateTime.Now.AddHours(1);
        private readonly DateTime _validConfirmationTime = DateTime.Now.AddHours(1);
        private readonly Address _validAddress = new Address("City", 8210, "Street", 1);
        private readonly int _validPassengerCount = 2;
        private readonly bool _validIsShared = false;
        
        [TestCase(false, 0)]
        [TestCase(true, 0)]
        [TestCase(null, 0)] //will default to false
        public void IsShared_WhenSet_ValidatesInput(bool isShared, int numberOfErrors)
        {
            var request = new CreateRideRequest
            {
                IsShared = isShared,
                DepartureTime = _validDepartureTime,
                ConfirmationDeadline = _validConfirmationTime,
                PassengerCount = _validPassengerCount,
                StartDestination = _validAddress,
                EndDestination = _validAddress
            };

            Assert.That(ValidateModelHelper.ValidateModel(request).Count, Is.EqualTo(numberOfErrors));
        }

        [Test]
        public void DepartureTime_WhenSetToExcatlyNow_ValidationFails()
        {
            var expectedNumberOfErrors = 1;
            var request = new CreateRideRequest
            {
                IsShared = _validIsShared,
                DepartureTime = DateTime.Now,
                ConfirmationDeadline = _validConfirmationTime,
                PassengerCount = _validPassengerCount,
                StartDestination = _validAddress,
                EndDestination = _validAddress
            };

            Assert.That(ValidateModelHelper.ValidateModel(request).Count, Is.EqualTo(expectedNumberOfErrors));
        }

        [Test]
        public void DepartureTime_WhenSetToExcatlyNowMinusOneSecond_ValidationFails()
        {
            var expectedNumberOfErrors = 1;
            var request = new CreateRideRequest
            {
                IsShared = _validIsShared,
                DepartureTime = DateTime.Now.AddSeconds(-1),
                ConfirmationDeadline = _validConfirmationTime,
                PassengerCount = _validPassengerCount,
                StartDestination = _validAddress,
                EndDestination = _validAddress
            };

            Assert.That(ValidateModelHelper.ValidateModel(request).Count, Is.EqualTo(expectedNumberOfErrors));
        }

        [Test]
        public void DepartureTime_WhenSetToExcatlyNowPlusOneSecond_ValidationSucceeds()
        {
            var expectedNumberOfErrors = 0;
            var request = new CreateRideRequest
            {
                IsShared = _validIsShared,
                DepartureTime = DateTime.Now.AddSeconds(1),
                ConfirmationDeadline = _validConfirmationTime,
                PassengerCount = _validPassengerCount,
                StartDestination = _validAddress,
                EndDestination = _validAddress
            };

            Assert.That(ValidateModelHelper.ValidateModel(request).Count, Is.EqualTo(expectedNumberOfErrors));
        }

        [Test]
        public void ConfirmationTime_WhenSetToExcatlyNow_ValidationFails()
        {
            var expectedNumberOfErrors = 1;
            var request = new CreateRideRequest
            {
                IsShared = _validIsShared,
                DepartureTime = _validDepartureTime,
                ConfirmationDeadline = DateTime.Now,
                PassengerCount = _validPassengerCount,
                StartDestination = _validAddress,
                EndDestination = _validAddress
            };

            Assert.That(ValidateModelHelper.ValidateModel(request).Count, Is.EqualTo(expectedNumberOfErrors));
        }

        [Test]
        public void ConfirmationTime_WhenSetToExcatlyNowMinusOneSecond_ValidationFails()
        {
            var expectedNumberOfErrors = 1;
            var request = new CreateRideRequest
            {
                IsShared = _validIsShared,
                DepartureTime = _validDepartureTime,
                ConfirmationDeadline = DateTime.Now.AddSeconds(-1),
                PassengerCount = _validPassengerCount,
                StartDestination = _validAddress,
                EndDestination = _validAddress
            };

            Assert.That(ValidateModelHelper.ValidateModel(request).Count, Is.EqualTo(expectedNumberOfErrors));
        }

        [Test]
        public void ConfirmationTime_WhenSetToExcatlyNowPlusOneSecond_ValidationSucceeds()
        {
            var expectedNumberOfErrors = 0;
            var request = new CreateRideRequest
            {
                IsShared = _validIsShared,
                DepartureTime = _validDepartureTime,
                ConfirmationDeadline = DateTime.Now.AddSeconds(1),
                PassengerCount = _validPassengerCount,
                StartDestination = _validAddress,
                EndDestination = _validAddress
            };

            Assert.That(ValidateModelHelper.ValidateModel(request).Count, Is.EqualTo(expectedNumberOfErrors));
        }

        [TestCase(-1, 1)]
        [TestCase(-0, 1)]
        [TestCase(1, 0)]
        [TestCase(2, 0)]
        [TestCase(3, 0)]
        [TestCase(4, 0)]
        [TestCase(5, 1)]
        public void PassengerCount_WhenSet_ValidatesInput(int passengerCount, int numberOfErrors)
        {
            var request = new CreateRideRequest
            {
                IsShared = _validIsShared,
                DepartureTime = _validDepartureTime,
                ConfirmationDeadline = _validConfirmationTime,
                PassengerCount = passengerCount,
                StartDestination = _validAddress,
                EndDestination = _validAddress
            };

            Assert.That(ValidateModelHelper.ValidateModel(request).Count, Is.EqualTo(numberOfErrors));
        }

        [Test]
        public void StartDestination_WhenSetToNull_ValidationFails()
        {
            var request = new CreateRideRequest
            {
                IsShared = _validIsShared,
                DepartureTime = _validDepartureTime,
                ConfirmationDeadline = _validConfirmationTime,
                PassengerCount = _validPassengerCount,
                StartDestination = null,
                EndDestination = _validAddress
            };

            Assert.That(ValidateModelHelper.ValidateModel(request).Count, Is.EqualTo(1));
        }

        [Test]
        public void EndDestination_WhenSetToNull_ValidationFails()
        {
            var request = new CreateRideRequest
            {
                IsShared = _validIsShared,
                DepartureTime = _validDepartureTime,
                ConfirmationDeadline = _validConfirmationTime,
                PassengerCount = _validPassengerCount,
                StartDestination = _validAddress,
                EndDestination = new Address("City", 8210, "Street2", 1)
            };
            Assert.That(ValidateModelHelper.ValidateModel(request).Count, Is.EqualTo(1));
        }
    }
}