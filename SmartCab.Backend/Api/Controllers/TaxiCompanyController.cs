﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Api.Responses;
using Api.ViewModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TaxiCompanyController : ControllerBase
    {
        /// <summary>
        /// Registers a new taxi company account and returns the username for the created account.
        /// </summary>
        /// <remarks>
        /// The following requirements apply to the username:
        /// ---- Some requirement: Some value
        /// 
        /// The following requirements apply to the password:
        /// ---- Some requirement: Some value
        /// </remarks>
        /// <param name="model">The data needed to create the taxi company account.</param>
        /// <returns>The created taxi company's username.</returns>
        [Produces("application/json")]
        [Route("[action]")]
        [HttpPost]
        [ProducesResponseType(typeof(RegisterResponse), StatusCodes.Status200OK)]
        public async Task<IActionResult> Register(RegisterRequest model)
        {
            //register logic
            return Ok(new RegisterResponse { Username = "Some username" });
        }

        /// <summary>
        /// Validates the user credentials and returns a JWT token if validation is successful.
        /// </summary>
        /// <param name="model">The username and password that will be validated.</param>
        /// <returns>Returns a new JWT token.</returns>
        [Produces("application/json")]
        [Route("[action]")]
        [HttpPost]
        [ProducesResponseType(typeof(LoginResponse), StatusCodes.Status200OK)]
        public async Task<IActionResult> Login(LoginRequest model)
        {
            //check credentials logic
            return Ok(new LoginResponse { Token = "Some generated token" });
        }

        /// <summary>
        /// Cancels the supplied JWT token.
        /// </summary>
        /// <param name="authorization">A valid JWT token.</param>
        /// <returns>200 if the supplied JWT token is valid otherwise 401</returns>
        /// <response code="401">If the taxi company was not logged in already (token was expired)</response>
        [Route("[action]")]
        [HttpPost]
        public async Task<IActionResult> Logout([FromHeader] string authorization)
        {
            //Add token to blacklist
            //https://piotrgankiewicz.com/2018/04/25/canceling-jwt-tokens-in-net-core/
            return Ok("Logout successful");
        }

        /// <summary>
        /// Updates the taxi company account with the supplied information.
        /// </summary>
        /// <param name="authorization">A valid JWT token.</param>
        /// <param name="model">The data used to update the taxi company account</param>
        /// <returns></returns>
        /// <response code="401">If the taxi company was not logged in already (token was expired)</response>
        [Route("[action]")]
        [HttpPut]
        public async Task<IActionResult> Edit([FromHeader] string authorization, [FromBody] EditTaxiCompanyRequest model)
        {
            //update customer logic
            return Ok("Customer account successfully updated.");
        }
    }
}
