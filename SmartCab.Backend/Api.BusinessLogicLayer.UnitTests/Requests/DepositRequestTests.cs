using Api.BusinessLogicLayer.Requests;
using NUnit.Framework;

namespace Api.BusinessLogicLayer.UnitTests.Requests
{
    [TestFixture]
    public class DepositRequestTests
    {
        [TestCase(-100000, 1)]
        [TestCase(-1, 1)]
        [TestCase(0, 1)]
        [TestCase((double)Constants.MinDepositAmount, 0)]
        [TestCase(1, 0)]
        [TestCase(1000, 0)]
        [TestCase(100000, 0)]
        [TestCase((double)Constants.MaxDepositAmount, 0)]
        [TestCase((double)Constants.MaxDepositAmount + 0.01, 1)]
        public void Deposit_WhenSet_ValidatesInput(decimal amount, int numberOfErrors)
        {
            var request = new DepositRequest
            {
                Deposit = amount
            };

            Assert.That(ValidateModelHelper.ValidateModel(request).Count, Is.EqualTo(numberOfErrors));
        }
    }
}