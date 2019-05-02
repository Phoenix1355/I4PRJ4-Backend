using Api.BusinessLogicLayer.Models;
using NUnit.Framework;

namespace Api.BusinessLogicLayer.UnitTests.Models
{
    [TestFixture]
    public class PushNotificationTests
    {
        private PushNotification _uut;

        [SetUp]
        public void SetUp()
        {
            _uut = new PushNotification();
        }

        [Test]
        public void AddDeviceId_WhenDeviceListIsEmpty_AddsTheDeviceId()
        {
            // Arrange
            var deviceId = "Some device ID";

            // Act
            _uut.AddDeviceId(deviceId);

            // Assert
            Assert.That(_uut.Devices, Has.Exactly(1).EqualTo(deviceId));
        }

        [Test]
        public void AddDeviceId_TwoTimes_AddsBothDeviceIds()
        {
            // Arrange
            var deviceId1 = "Some device ID";
            var deviceId2 = "Another device ID";

            // Act
            _uut.AddDeviceId(deviceId1);
            _uut.AddDeviceId(deviceId2);

            // Assert
            Assert.Multiple(() =>
            {
                Assert.That(_uut.Devices, Has.Exactly(1).EqualTo(deviceId1));
                Assert.That(_uut.Devices, Has.Exactly(1).EqualTo(deviceId2));
            });
        }

        [Test]
        public void AddCustomData_WhenCustomDataIsEmpty_AddsTheCustomData()
        {
            // Arrange
            var key = "Identifier";
            var value = "value";

            // Act
            _uut.AddCustomData(key, value);

            // Assert
            Assert.Multiple(() =>
            {
                Assert.That(_uut.CustomData, Contains.Key(key));
                Assert.That(_uut.CustomData, Contains.Value(value));
            });
        }

        [Test]
        public void AddCustomData_TwoTimes_AddsBothSets()
        {
            // Arrange
            var key1 = "Identifier";
            var value1 = "value";

            var key2 = "custom";
            var value2 = "data";

            // Act
            _uut.AddCustomData(key1, value1);
            _uut.AddCustomData(key2, value2);

            // Assert
            Assert.Multiple(() =>
            {
                Assert.That(_uut.CustomData, Contains.Key(key1));
                Assert.That(_uut.CustomData, Contains.Value(value1));

                Assert.That(_uut.CustomData, Contains.Key(key2));
                Assert.That(_uut.CustomData, Contains.Value(value2));
            });
        }
    }
}