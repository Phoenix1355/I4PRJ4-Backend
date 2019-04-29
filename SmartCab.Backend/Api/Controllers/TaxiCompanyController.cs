using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Api.BusinessLogicLayer.Interfaces;
using Api.BusinessLogicLayer.Requests;
using Api.BusinessLogicLayer.Responses;
using Api.Responses;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TaxiCompanyController : ControllerBase
    {
        private readonly ITaxiCompanyService _taxiCompanyService;

        public TaxiCompanyController(ITaxiCompanyService taxiCompanyService)
        {
            _taxiCompanyService = taxiCompanyService;
        }

        /// <summary>
        /// Registers a new taxi company account and returns a token to the calling client
        /// </summary>
        /// <remarks>
        /// When a taxi company is successfully created, a status code 200 will be returned
        /// This response will contain a JWT token that is tied to the taxi company account
        /// <br/>
        /// The following requirements apply to the password:
        /// ---- Minimum 8 characters long
        /// ---- Minimum one lower case letter
        /// ---- Minimum one upper case letter
        /// ---- Minimum one number
        /// ---- Minimum one non-alphanumeric letter
        /// </remarks>
        /// <param name="request">The data needed to create the taxi company account.</param>
        /// <returns>The valid JWT token that is tied to the taxi company</returns>
        /// <response code="400">If the supplied request wasn't valid</response>
        /// <response code="500">If an internal server error occured</response>
        [Produces("application/json")]
        [Route("[action]")]
        [HttpPost]
        [ProducesResponseType(typeof(RegisterResponseTaxiCompany), StatusCodes.Status200OK)]
        public async Task<IActionResult> Register(RegisterRequest request)
        {
            var response = await _taxiCompanyService.AddTaxiCompanyAsync(request);
            return Ok(response);
        }

        /// <summary>
        /// Validates the user credentials and returns a JWT token if validation is successful.
        /// </summary>
        /// <param name="request">The username and password that will be validated.</param>
        /// <returns>Returns a new JWT token.</returns>
        /// <response code="400">If the supplied request wasn't valid.</response>
        /// <response code="500">If an internal server error occured.</response>
        [Produces("application/json")]
        [Route("[action]")]
        [HttpPost]
        [ProducesResponseType(typeof(LoginResponseTaxiCompany), StatusCodes.Status200OK)]
        public async Task<IActionResult> Login(LoginRequest request)
        {
            var response = await _taxiCompanyService.LoginTaxiCompanyAsync(request);
            return Ok(response);
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
