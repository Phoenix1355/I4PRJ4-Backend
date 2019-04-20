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
    }
}