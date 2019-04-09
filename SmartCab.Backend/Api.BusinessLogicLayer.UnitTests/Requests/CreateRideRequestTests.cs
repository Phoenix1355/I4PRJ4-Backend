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


        [TestCase(8210, 0)]
        [TestCase(1000, 0)]
        [TestCase(999, 1)]
        [TestCase(1001, 0)]
        [TestCase(9999, 0)]
        [TestCase(10000, 1)]
        [TestCase(5000, 0)]
        [TestCase(0, 1)]
        [TestCase(-1000, 1)]
        public void StartDestination_WhenPostalCodeIsSet_ValidationFailsWhenInvalid(int postalCode, int numberOfErrors)
        {
            var request = new CreateRideRequest
            {
                IsShared = _validIsShared,
                DepartureTime = _validDepartureTime,
                ConfirmationDeadline = _validConfirmationTime,
                PassengerCount = _validPassengerCount,
                StartDestination = new Address("Name", postalCode, "street", 21),
                EndDestination = _validAddress
            };

            Assert.That(ValidateModelHelper.ValidateModel(request).Count, Is.EqualTo(numberOfErrors));
        }

        [TestCase(8210, 0)]
        [TestCase(1000, 0)]
        [TestCase(999, 1)]
        [TestCase(1001, 0)]
        [TestCase(9999, 0)]
        [TestCase(10000, 1)]
        [TestCase(5000, 0)]
        [TestCase(0, 1)]
        [TestCase(-1000, 1)]
        public void EndDestination_WhenPostalCodeIsSet_ValidationFailsWhenInvalid(int postalCode, int numberOfErrors)
        {
            var request = new CreateRideRequest
            {
                IsShared = _validIsShared,
                DepartureTime = _validDepartureTime,
                ConfirmationDeadline = _validConfirmationTime,
                PassengerCount = _validPassengerCount,
                StartDestination = _validAddress,
                EndDestination = new Address("Name", postalCode, "street", 21)
            };

            Assert.That(ValidateModelHelper.ValidateModel(request).Count, Is.EqualTo(numberOfErrors));
        }

        [TestCase("Invalid char is 1")]
        [TestCase("Invalid char is ! ")]
        [TestCase("Invalid char is #")]
        [TestCase("Invalid char is ¤")]
        [TestCase("Invalid char is %")]
        [TestCase("Invalid char is &")]
        [TestCase("Invalid char is /")]
        [TestCase("Invalid char is (")]
        [TestCase("Invalid char is )")]
        [TestCase("Invalid char is =")]
        [TestCase("Invalid char is ´")]
        [TestCase("Invalid char is `")]
        [TestCase("Invalid char is @")]
        [TestCase("Invalid char is £")]
        [TestCase("Invalid char is $")]
        [TestCase("Invalid char is €")]
        public void StartDestination_WhenCityNameInvalid_ModelIsInvalid(string cityName)
        {
            var request = new CreateRideRequest
            {
                IsShared = _validIsShared,
                DepartureTime = _validDepartureTime,
                ConfirmationDeadline = _validConfirmationTime,
                PassengerCount = _validPassengerCount,
                StartDestination = new Address(cityName, 8219, "street", 21),
                EndDestination = _validAddress
            };

            Assert.That(ValidateModelHelper.ValidateModel(request).Count, Is.EqualTo(1));
        }

        [TestCase("Invalid char is 1")]
        [TestCase("Invalid char is ! ")]
        [TestCase("Invalid char is #")]
        [TestCase("Invalid char is ¤")]
        [TestCase("Invalid char is %")]
        [TestCase("Invalid char is &")]
        [TestCase("Invalid char is /")]
        [TestCase("Invalid char is (")]
        [TestCase("Invalid char is )")]
        [TestCase("Invalid char is =")]
        [TestCase("Invalid char is ´")]
        [TestCase("Invalid char is `")]
        [TestCase("Invalid char is @")]
        [TestCase("Invalid char is £")]
        [TestCase("Invalid char is $")]
        [TestCase("Invalid char is €")]
        public void EndDestination_WhenCityNameInvalid_ModelIsInvalid(string cityName)
        {
            var request = new CreateRideRequest
            {
                IsShared = _validIsShared,
                DepartureTime = _validDepartureTime,
                ConfirmationDeadline = _validConfirmationTime,
                PassengerCount = _validPassengerCount,
                StartDestination = _validAddress,
                EndDestination = new Address(cityName, 8219, "street", 21)
            };

            Assert.That(ValidateModelHelper.ValidateModel(request).Count, Is.EqualTo(1));
        }

        [TestCase("Invalid char is 1")]
        [TestCase("Invalid char is ! ")]
        [TestCase("Invalid char is #")]
        [TestCase("Invalid char is ¤")]
        [TestCase("Invalid char is %")]
        [TestCase("Invalid char is &")]
        [TestCase("Invalid char is /")]
        [TestCase("Invalid char is (")]
        [TestCase("Invalid char is )")]
        [TestCase("Invalid char is =")]
        [TestCase("Invalid char is ´")]
        [TestCase("Invalid char is `")]
        [TestCase("Invalid char is @")]
        [TestCase("Invalid char is £")]
        [TestCase("Invalid char is $")]
        [TestCase("Invalid char is €")]
        public void StartDestination_WhenStreetNameInvalid_ModelIsInvalid(string streetName)
        {
            var request = new CreateRideRequest
            {
                IsShared = _validIsShared,
                DepartureTime = _validDepartureTime,
                ConfirmationDeadline = _validConfirmationTime,
                PassengerCount = _validPassengerCount,
                StartDestination = new Address("Name", 8219, streetName, 21),
                EndDestination = _validAddress
            };

            Assert.That(ValidateModelHelper.ValidateModel(request).Count, Is.EqualTo(1));
        }

        [TestCase("Invalid char is 1")]
        [TestCase("Invalid char is ! ")]
        [TestCase("Invalid char is #")]
        [TestCase("Invalid char is ¤")]
        [TestCase("Invalid char is %")]
        [TestCase("Invalid char is &")]
        [TestCase("Invalid char is /")]
        [TestCase("Invalid char is (")]
        [TestCase("Invalid char is )")]
        [TestCase("Invalid char is =")]
        [TestCase("Invalid char is ´")]
        [TestCase("Invalid char is `")]
        [TestCase("Invalid char is @")]
        [TestCase("Invalid char is £")]
        [TestCase("Invalid char is $")]
        [TestCase("Invalid char is €")]
        public void EndDestination_WhenStreetNameInvalid_ModelIsInvalid(string streetName)
        {
            var request = new CreateRideRequest
            {
                IsShared = _validIsShared,
                DepartureTime = _validDepartureTime,
                ConfirmationDeadline = _validConfirmationTime,
                PassengerCount = _validPassengerCount,
                StartDestination = _validAddress,
                EndDestination = new Address("Name", 8219, streetName, 21)
            };

            Assert.That(ValidateModelHelper.ValidateModel(request).Count, Is.EqualTo(1));
        }

        [TestCase(10, 0)]
        [TestCase(25, 0)]
        [TestCase(0, 1)]
        [TestCase(1, 0)]
        [TestCase(-0, 1)]
        [TestCase(-1230, 1)]
        public void StartDestination_WhenStreetIsSet_ValidationFailsWhenInvalid(int streetNumber, int numberOfErrors)
        {
            var request = new CreateRideRequest
            {
                IsShared = _validIsShared,
                DepartureTime = _validDepartureTime,
                ConfirmationDeadline = _validConfirmationTime,
                PassengerCount = _validPassengerCount,
                StartDestination = new Address("Name", 8219, "street", streetNumber),
                EndDestination = _validAddress
            };

            Assert.That(ValidateModelHelper.ValidateModel(request).Count, Is.EqualTo(numberOfErrors));
        }

        [TestCase(10, 0)]
        [TestCase(25, 0)]
        [TestCase(0, 1)]
        [TestCase(1, 0)]
        [TestCase(-0, 1)]
        [TestCase(-1230, 1)]
        public void EndDestination_WhenStreetIsSet_ValidationFailsWhenInvalid(int streetNumber, int numberOfErrors)
        {
            var request = new CreateRideRequest
            {
                IsShared = _validIsShared,
                DepartureTime = _validDepartureTime,
                ConfirmationDeadline = _validConfirmationTime,
                PassengerCount = _validPassengerCount,
                StartDestination = _validAddress,
                EndDestination = new Address("Name", 8219, "street", streetNumber)
            };

            Assert.That(ValidateModelHelper.ValidateModel(request).Count, Is.EqualTo(numberOfErrors));
        }
    }
}