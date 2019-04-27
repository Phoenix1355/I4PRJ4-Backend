using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Api.BusinessLogicLayer.Interfaces;
using Api.BusinessLogicLayer.Responses;
using Api.BusinessLogicLayer.Services;
using Api.DataAccessLayer.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrderController : ControllerBase
    {
        private readonly IOrderService _orderService;

        public OrderController(IOrderService orderService)
        {
            _orderService = orderService;
        }

        /// <summary>
        /// Returns all open orders stored in the system.
        /// </summary>
        /// <param name="authorization">A valid JWT token that is associated with a taxi company account.</param>
        /// <returns>All open orders stored in the system</returns>
        /// <response code="401">
        /// An invalid JWT token was provided in the authorization header.<br/>
        /// This can happen if the supplied token is expired or because the user associated with the token does not have the required role needed to make the request.
        /// </response>
        /// <response code="500">If an internal server error occured.</response>
        [Authorize(Roles = nameof(TaxiCompany))]
        [Produces("application/json")]
        [Route("[action]")]
        [HttpGet]
        [ProducesResponseType(typeof(OpenOrdersResponse), StatusCodes.Status200OK)]
        public async Task<IActionResult> Open([FromHeader] string authorization)
        {
            var response = await _orderService.GetOpenOrdersAsync();
            return Ok(response);
        }

        /// <summary>
        /// Updates the order with the supplied ID, and associated rides so they are accepted.
        /// </summary>
        /// <remarks>
        /// Required role: "TaxiCompany"
        /// </remarks>
        /// <param name="authorization">A valid JWT token.</param>
        /// <param name="id">The id of the order that should be accepted.</param>
        /// <returns></returns>
        /// <response code="400">Could mean that the Order was already in accepted state when the request made it to the server</response>
        /// <response code="401">If the user was not logged in already (token was expired)</response>
        [Route("{id}/[action]")]
        [Produces("application/json")]
        [HttpPut]
        public async Task<ActionResult<Ride>> Accept([FromHeader] string authorization, int id)
        {
            throw new NotImplementedException("Not implemented yet");
            return Ok($"The ride with {id} is now successfully marked as accepted.");
        }
    }
}