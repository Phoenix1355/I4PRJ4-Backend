using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Api.BusinessLogicLayer.Interfaces;
using Api.DataAccessLayer.Models;
using Api.Requests;
using AutoMapper;
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
        /// <param name="authorization">A valid JWT token that is associated to a taxi company account</param>
        /// <returns>All open rides stored in the system</returns>
        /// <response code="401">If the customer was not logged in already (token was expired)</response>
        [Produces("application/json")]
        [Route("[action]")]
        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<Ride>), StatusCodes.Status200OK)]
        public async Task<IActionResult> Open([FromHeader] string authorization)
        {
            var rides = new List<Ride>(); //TODO make some call to service layer
            return Ok(rides);
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