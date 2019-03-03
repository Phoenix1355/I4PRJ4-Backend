using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Backend.BusinessLogicLayer.DataTransferObjects;
using Backend.BusinessLogicLayer.Interfaces;
using Backend.DataAccessLayer.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace Backend.Api.Controllers
{
    [Route("api/[controller]/")]
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
        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<Ride>), StatusCodes.Status200OK)]
        public async Task<IActionResult> Get()
        {
            return Ok(await _rideService.GetAllRidesAsync());
        }

        /// <summary>
        /// Returns a specific ride with the given id
        /// </summary>
        /// <param name="id">The id of the ride that is returned</param>
        /// <returns></returns>
        [HttpGet("{id}")]
        [ProducesResponseType(typeof(Ride), StatusCodes.Status200OK)]
        public async Task<ActionResult<Ride>> Get(int id)
        {
            var ride = await _rideService.GetRideByIdAsync(id);

            if (ride == null)
            {
                return NotFound("The ride does not exist");
            }

            return Ok(ride);
        }

        /// <summary>
        /// Adds a new ride to the database.
        /// </summary>
        /// <remarks>
        /// <para>Iam some remark that can be use to supply some additional information to the consumer of the API.</para>
        /// <para></para>
        /// <para>Another line</para>
        /// </remarks>
        /// <param name="rideDTO">The ride that should be added to the database</param>
        /// <returns></returns>
        [HttpPost]
        [ProducesResponseType(typeof(Ride), StatusCodes.Status201Created)]
        public async Task<IActionResult> Post([FromBody] RideDTO rideDTO)
        {
            var createdRide = await _rideService.AddRideAsync(rideDTO);

            if (createdRide == null)
            {
                return BadRequest("Something went wrong, try again.");
            }

            return Created($"/api/rides/{createdRide.Id}", createdRide);
        }

        /// <summary>
        /// Modifies an existing ride and returns the updated ride
        /// </summary>
        /// <param name="ride">The ride to update</param>
        /// <returns></returns>
        [HttpPut]
        [ProducesResponseType(typeof(Ride), StatusCodes.Status200OK)]
        public async Task<IActionResult> Put([FromBody] Ride ride)
        {
            try
            {
                var updatedRide = await _rideService.UpdateRideAsync(ride);
                return Ok(updatedRide);
            }
            catch (Exception e)
            {
                return BadRequest("The ride could not be updated, as it appears it does not exist. \n\n" + e);
            }
        }

        /// <summary>
        /// Deletes a ride and returns the deleted ride
        /// </summary>
        /// <param name="id">The id of the ride to delete</param>
        /// <returns></returns>
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                return Ok(await _rideService.DeleteRideAsync(id));
            }
            catch (Exception e)
            {
                return BadRequest("The ride could not be deleted, as it appears it does not exist. \n\n" + e);
            }
            
        }
    }
}
