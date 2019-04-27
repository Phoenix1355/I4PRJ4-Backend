using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Api.BusinessLogicLayer;
using Api.BusinessLogicLayer.Interfaces;
using Api.BusinessLogicLayer.Responses;
using Api.BusinessLogicLayer.Services;
using Api.DataAccessLayer.Models;
using CustomExceptions;
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
        /// Updates the order with the supplied ID, and updates associated rides so they are accepted.
        /// </summary>
        /// <remarks>
        /// Required role: "TaxiCompany"<br/>
        /// When an order is successfully accepted all related customers receives a notification telling them,<br/>
        /// that the ride they ordered has been accepted. The customer's account will also be debited for the cost of the ride.
        /// </remarks>
        /// <param name="authorization">A valid JWT token.</param>
        /// <param name="id">The id of the order that should be accepted.</param>
        /// <returns></returns>
        /// <response code="400">Could mean that the Order was already in accepted state when the request made it to the server</response>
        /// <response code="401">
        /// An invalid JWT token was provided in the authorization header.<br/>
        /// This can happen if the supplied token is expired or because the user associated with the token does not have the required role needed to make the request.
        /// </response>
        [Authorize(Roles = nameof(TaxiCompany))]
        [Produces("application/json")]
        [Route("{id}/[action]")]
        [HttpPut]
        [ProducesResponseType(typeof(AcceptOrderResponse), StatusCodes.Status200OK)]
        public async Task<IActionResult> Accept([FromHeader] string authorization, int id)
        {
            //Get the taxiCompanyId that is stored as a claim in the token
            var taxiCompanyId = User.Claims.FirstOrDefault(x => x.Type == Constants.UserIdClaim)?.Value;

            //This should never happen, but better safe than sorry
            if (string.IsNullOrEmpty(taxiCompanyId))
            {
                throw new UserIdInvalidException(
                    $"The supplied JSON Web Token does not contain a valid value in the '{ Constants.UserIdClaim }' claim.");
            }

            var response = await _orderService.AcceptOrderAsync(taxiCompanyId, id);
            return Ok(response);
        }
    }
}