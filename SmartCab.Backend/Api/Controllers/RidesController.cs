using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Api.BusinessLogicLayer.DataTransferObjects;
using Api.BusinessLogicLayer.Interfaces;
using Api.BusinessLogicLayer.Responses;
using Api.DataAccessLayer.Models;
using Api.Requests;
using AutoMapper;
using Castle.DynamicProxy.Generators.Emitters.SimpleAST;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RidesController : ControllerBase
    {
        private readonly IRideService _rideService;
        private readonly IMapper _mapper;

        public RidesController(IRideService rideService, IMapper mapper)
        {
            _rideService = rideService;
            _mapper = mapper;
        }

        /// <summary>
        /// Returns all open rides stored in the system.
        /// </summary>
        /// <param name="authorization">A valid JWT token that is associated with a taxi company account.</param>
        /// <returns>All open rides stored in the system</returns>
        /// <response code="401">
        /// An invalid JWT token was provided in the authorization header.<br/>
        /// This can happen if the supplied token is expired or because the user associated with the token does not have the required role needed to make the request.
        /// </response>
        /// <response code="500">If an internal server error occured.</response>
        [Produces("application/json")]
        [Route("[action]")]
        [HttpGet]
        [ProducesResponseType(typeof(OpenRidesResponse), StatusCodes.Status200OK)]
        public async Task<IActionResult> Open([FromHeader] string authorization)
        {
            try
            {
                var rides = await _rideService.GetAllOpenRidesAsync();
                return Ok(rides);
            }
            catch (Exception e)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "An unknown error occured on the server");
            }
        }

        /// <summary>
        /// Returns all available details about a specific ride.
        /// </summary>
        /// <param name="authorization">A valid JWT token that is associated with a taxi company account.</param>
        /// <param name="id">The id of the ride for which the details is returned.</param>
        /// <returns>Details about a specific ride.</returns>
        /// <response code="401">
        /// An invalid JWT token was provided in the authorization header.<br/>
        /// This can happen if the supplied token is expired or because the user associated with the token does not have the required role needed to make the request.
        /// </response>
        /// <response code="500">If an internal server error occured.</response>
        [Produces("application/json")]
        [Route("{id}/[action]")]
        [HttpGet]
        [ProducesResponseType(typeof(SoloRide), StatusCodes.Status200OK)]
        public async Task<IActionResult> Details([FromHeader] string authorization, int id)
        {
            try
            {
                var ride = new Ride(); //todo fetch details
                return Ok(ride);
            }
            catch (Exception e)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "An unknown error occured on the server");
            }
        }


        /// <summary>
        /// Creates a new ride
        /// </summary>
        /// <param name="authorization">A valid JWT token.</param>
        /// <param name="request">Information about the ride that should be updated.</param>
        /// <returns>The created ride.</returns>
        /// <response code="401">If the customer was not logged in already (token was expired)</response>
        [Produces("application/json")]
        [ProducesResponseType(typeof(Ride), StatusCodes.Status200OK)]
        [Route("[action]")]
        [HttpPost]
        public async Task<IActionResult> Create([FromHeader] string authorization, [FromBody] CreateRideRequest request)
        {
            var ride = new Ride(); //TODO call service layer
            return Ok(ride);
        }

        /// <summary>
        /// Updates the ride with the supplied ID so it is accepted.
        /// </summary>
        /// <param name="authorization">A valid JWT token.</param>
        /// <param name="id">The id of the ride that should be accepted.</param>
        /// <returns></returns>
        /// <response code="400">Could mean that the ride was no longer in an "accepted" state when the request made it to the server</response>
        /// <response code="401">If the customer was not logged in already (token was expired)</response>
        [Route("{id}/[action]")]
        [HttpPut]
        public async Task<ActionResult<Ride>> Accept([FromHeader] string authorization, int id)
        {
            return Ok($"The ride with {id} is now successfully marked as accepted.");
        }
    }
}