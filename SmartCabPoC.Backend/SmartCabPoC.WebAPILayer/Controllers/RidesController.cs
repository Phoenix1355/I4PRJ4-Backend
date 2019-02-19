using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using SmartCabPoC.BusinessLayer.Abstractions;
using SmartCabPoC.BusinessLayer.Services;
using SmartCabPoC.DataLayer.Models;
using SmartCabPoC.DataLayer.Repositories;

namespace SmartCabPoC.WebAPILayer.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RidesController : ControllerBase
    {
        private readonly IRideService _rideService;

        public RidesController(IRideService rideService)
        {
            _rideService = rideService;
        }

        /// <summary>
        /// Returns a json string representing an array of rides.
        /// 
        /// Handles GET requests directed to the following url: api/rides.
        /// 
        /// Example of json returned:
        ///
        ///     [
        ///         {
        ///             "id": 1,
        ///             "departureTime": "2019-10-21T20:15:00"
        ///         },
        ///         {
        ///             "id": 2,
        ///             "departureTime": "2019-10-21T20:15:00"
        ///         }
        ///     ]
        /// 
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<IEnumerable<Ride>> Get()
        {
            return await _rideService.GetAllRidesAsync();
        }

        /// <summary>
        /// Adds a new ride to the database.
        ///
        /// Handles POST requests directed to the following url: api/rides.
        /// 
        /// Example of expected json:
        ///
        ///     {
        ///         "DepartureTime": "2019-10-21 20:15"
        ///     }
        /// 
        /// </summary>
        /// <param name="ride">A ride represented as a json string.</param>
        /// <returns></returns>
        [HttpPost]
        public async Task Post([FromBody] Ride ride)
        {
            await _rideService.AddRideAsync(ride);
        }


        

        

}
}
