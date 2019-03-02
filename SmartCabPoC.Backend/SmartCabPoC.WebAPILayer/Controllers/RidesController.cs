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
        /// Returns all rides in stored in the system.
        /// </summary>
        /// <remarks>Iam some remark that can be use to supply some additional information to the consumer of the API.</remarks>
        /// <returns></returns>
        /// <response code="200">Success</response>
        /// <response code="401">Unauthorized</response>
        [HttpGet]
        public async Task<IEnumerable<Ride>> Get()
        {
            return await _rideService.GetAllRidesAsync();
        }

        /// <summary>
        /// Adds a new ride to the database.
        /// </summary>
        /// <remarks>
        /// <para>Iam some remark that can be use to supply some additional information to the consumer of the API.</para>
        /// <para></para>
        /// <para>Another line</para>
        /// </remarks>
        /// <param name="ride">The ride that should be added to the database</param>
        /// <returns></returns>
        /// <response code="200">Success</response>
        /// <response code="401">Unauthorized</response>
        [HttpPost]
        public async Task Post([FromBody] Ride ride)
        {
            await _rideService.AddRideAsync(ride);
        }
    }
}
