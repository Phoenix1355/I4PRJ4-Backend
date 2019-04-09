using System;
using System.Collections.Generic;
using System.Text;
using Api.BusinessLogicLayer.Requests;
using Api.BusinessLogicLayer.UnitTests.Requests;
using Api.DataAccessLayer.Models;
using NUnit.Framework;

namespace Api.DataAccessLayer.UnitTests.Models
{
    [TestFixture]
    class AddressTests
    {
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
            var address = new Address("City", postalCode, "Street", 1);

            Assert.That(ValidateModelHelper.ValidateModel(address).Count, Is.EqualTo(numberOfErrors));
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
            var address = new Address(cityName, 8210, "Street", 1);

            Assert.That(ValidateModelHelper.ValidateModel(address).Count, Is.EqualTo(1));
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
            var address = new Address("City", 8210, streetName, 1);

            Assert.That(ValidateModelHelper.ValidateModel(address).Count, Is.EqualTo(1));
        }


        [TestCase(10, 0)]
        [TestCase(25, 0)]
        [TestCase(0, 1)]
        [TestCase(1, 0)]
        [TestCase(-0, 1)]
        [TestCase(-1230, 1)]
        public void StartDestination_WhenStreetIsSet_ValidationFailsWhenInvalid(int streetNumber, int numberOfErrors)
        {
            var address = new Address("City", 8210, "Street", streetNumber);

            Assert.That(ValidateModelHelper.ValidateModel(address).Count, Is.EqualTo(numberOfErrors));
        }
    }
}