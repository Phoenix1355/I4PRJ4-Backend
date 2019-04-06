using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Api.BusinessLogicLayer.Requests;
using NUnit.Framework;

namespace Api.IntegrationTests.Ride
{
    [TestFixture]
    class CreateTests : IntegrationSetup
    {
        [Test]
        public async Task Create_WhenCreateIsCalled_RideIsCreated()
        {
            var request = new CreateRideRequest()
            {
                ConfirmationDeadline = DateTime.Now,
                DepartureTime = DateTime.Now,
                
            };
           var response = await PostAsync("api/rides/create", request);

           Console.WriteLine(await response.Content.ReadAsStringAsync());
        }

    }
}
