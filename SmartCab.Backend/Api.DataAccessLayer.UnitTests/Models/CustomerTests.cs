using System;
using System.Collections.Generic;
using System.Text;
using Api.DataAccessLayer.Models;
using NUnit.Framework;
using NUnit.Framework.Internal;

namespace Api.DataAccessLayer.UnitTests.Models
{
    [TestFixture()]
    public class CustomerTests
    {

        [Test]
        public void ctor_ConstructusCustomer_BalanceEqualZero()
        {
            var customer = new Customer();
            Assert.That(customer.Balance, Is.EqualTo(0));
        }

        [Test]
        public void ctor_ConstructusCustomer_ReservedEqualZero()
        {
            var customer = new Customer();
            Assert.That(customer.ReservedAmount, Is.EqualTo(0));
        }

        [Test]
        public void ctor_ConstructusCustomer_ListEmpty()
        {
            var customer = new Customer();
            Assert.IsEmpty(customer.Rides);
        }
    }
}
