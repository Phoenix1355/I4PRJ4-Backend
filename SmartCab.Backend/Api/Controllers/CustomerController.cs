using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Api.Responses;
using Api.ViewModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Primitives;

namespace Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CustomerController : ControllerBase
    {
        /// <summary>
        /// Registers a new customer account and returns the username for the created account.
        /// </summary>
        /// <remarks>
        /// The following requirements apply to the username:
        /// ---- Some requirement: Some value
        /// 
        /// The following requirements apply to the password:
        /// ---- Some requirement: Some value
        /// </remarks>
        /// <param name="request">The data needed to create the customer account</param>
        /// <returns>The created customer's username.</returns>
        [Produces("application/json")]
        [Route("[action]")]
        [HttpPost]
        [ProducesResponseType(typeof(RegisterResponse), StatusCodes.Status200OK)]
        public async Task<IActionResult> Register(RegisterRequest request)
        {
            //register logic
            return Ok(new RegisterResponse {Username = "Some username"});
        }

        /// <summary>
        /// Validates the user credentials and returns a JWT token if validation is successful.
        /// </summary>
        /// <param name="request">The username and password that will be validated.</param>
        /// <returns>Returns a new JWT token.</returns>
        [Produces("application/json")]
        [Route("[action]")]
        [HttpPost]
        [ProducesResponseType(typeof(LoginResponse), StatusCodes.Status200OK)]
        public async Task<IActionResult> Login(LoginRequest request)
        {
            //check credentials logic
            return Ok(new LoginResponse {Token = "Some generated token"});
        }

        /// <summary>
        /// Updates the customer account with the supplied information.
        /// </summary>
        /// <param name="authorization">A valid JWT token.</param>
        /// <param name="request">The data used to update the customer account</param>
        /// <returns></returns>
        /// <response code="401">If the customer was not logged in already (token was expired)</response>
        [Route("[action]")]
        [HttpPut]
        public async Task<IActionResult> Edit([FromHeader] string authorization, [FromBody] EditCustomerRequest request)
        {
            //update customer logic
            return Ok("Customer account successfully updated.");
        }

        /// <summary>
        /// Returns all rides belonging to the customer associated with the supplied JWT token.
        /// </summary>
        /// <param name="authorization">A valid JWT token.</param>
        /// <returns></returns>
        /// <response code="401">If the customer was not logged in already (token was expired)</response>
        [Produces("application/json")]
        [Route("[action]")]
        [HttpGet]
        [ProducesResponseType(typeof(RidesResponse), StatusCodes.Status200OK)]
        public async Task<IActionResult> Rides([FromHeader] string authorization)
        {
            //Get name from JWT token --> User.Identity.Name --> this will access a claim set on the token
            //Get rides from database and return it
            return Ok();
        }
    }
}
