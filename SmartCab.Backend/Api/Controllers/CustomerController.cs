using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Api.BusinessLogicLayer;
using Api.BusinessLogicLayer.Interfaces;
using Api.BusinessLogicLayer.Requests;
using Api.BusinessLogicLayer.Responses;
using Api.DataAccessLayer;
using Api.DataAccessLayer.Models;
using Api.Requests;
using Api.Responses;
using CustomExceptions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Primitives;

namespace Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CustomerController : ControllerBase
    {
        private readonly ICustomerService _customerService;

        public CustomerController(ICustomerService customerService)
        {
            _customerService = customerService;
        }

        /// <summary>
        /// Registers a new customer and issues a token to the calling client.
        /// </summary>
        /// <remarks>
        /// When a customer is successfully created a status 200 will be returned.
        /// This response will contain a JWT token that is tied to the created account.
        /// The token must be used when making requests to other endpoints in this API.
        /// <br/>
        /// The following requirements apply to the password:
        /// ---- Minimum 8 characters long
        /// ---- Minimum one lower case letter
        /// ---- Minimum one upper case letter
        /// ---- Minimum one number
        /// ---- Minimum one non-alphanumeric letter
        /// </remarks>
        /// <param name="request">The data needed to create the customer</param>
        /// <returns>A valid JWT token that is tied to the created customer</returns>
        /// <response code="400">If the supplied request wasn't valid.</response>
        /// <response code="500">If an internal server error occured.</response>
        [Produces("application/json")]
        [Route("[action]")]
        [HttpPost]
        [ProducesResponseType(typeof(RegisterResponse), StatusCodes.Status200OK)]
        public async Task<IActionResult> Register(RegisterRequest request)
        {
            var response = await _customerService.AddCustomerAsync(request);
            return Ok(response);
        }

        /// <summary>
        /// Validates the user credentials and returns a JWT token if validation is successful.
        /// </summary>
        /// <param name="request">The email and password that will be validated.</param>
        /// <returns>Returns a new JWT token.</returns>
        /// <response code="400">If the supplied request wasn't valid.</response>
        /// <response code="500">If an internal server error occured.</response>
        [Produces("application/json")]
        [Route("[action]")]
        [HttpPost]
        [ProducesResponseType(typeof(LoginResponse), StatusCodes.Status200OK)]
        public async Task<IActionResult> Login(LoginRequest request)
        {
            var response = await _customerService.LoginCustomerAsync(request);
            return Ok(response);
        }

        /// <summary>
        /// Updates the customer account with the supplied information.
        /// </summary>
        /// <param name="authorization">A valid JWT token.</param>
        /// <param name="request">The data used to update the customer account</param>
        /// <returns></returns>
        /// <response code="401">If the customer was not logged in already</response>
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
        /// <returns></returns>
        /// <response code="401">If the customer was not logged in already (token was expired)</response>
        [Authorize(Roles = nameof(Customer))]
        [Produces("application/json")]
        [Route("[action]")]
        [HttpGet]
        [ProducesResponseType(typeof(RidesResponse), StatusCodes.Status200OK)]
        public async Task<IActionResult> Rides()
        {
            var email = User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.Email)?.Value;
            var expiration = User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.Expiration)?.Value;
            //Get name from JWT token --> User.Identity.Name --> this will access a claim set on the token
            //Get rides from database and return it
            return Ok(new
            {
                Email = email,
                Expiration = expiration
            });
        }

        /// <summary>
        /// Deposit amount in request to the customer associated with the supplied JWT token.
        /// </summary>
        /// <returns></returns>
        /// <response code="401">If the customer was not logged in already (token was expired)</response>
        [Authorize(Roles = nameof(Customer))]
        [Produces("application/json")]
        [Route("[action]")]
        [HttpPut]
        [ProducesResponseType(typeof(RidesResponse), StatusCodes.Status200OK)]
        public async Task<IActionResult> Deposit([FromHeader] string authorization, [FromBody] DepositRequest request)
        {
            var customerId = User.Claims.FirstOrDefault(x => x.Type == Constants.UserIdClaim)?.Value;

            if (string.IsNullOrEmpty(customerId))
            {
                throw new UserIdInvalidException(
                    $"The supplied JSON Web Token does not contain a valid value in the '{ Constants.UserIdClaim }' claim.");
            }

            await _customerService.DepositAsync(request, customerId);

            return Ok();
        }
    }
}
