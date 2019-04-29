using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Api.BusinessLogicLayer.Requests;
using Api.DataAccessLayer.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PriceController : ControllerBase
    {

        /// <summary>
        /// Calculates and returns the price for a taxi ride based on two addresses.
        /// </summary>
        /// <remarks>
        /// Currently now authorization is required to make this request.
        /// </remarks>
        /// <param name="request">Consists of an start address and an end address.</param>
        /// <returns>Th</returns>
        [HttpPost]
        [ProducesResponseType(typeof(decimal), StatusCodes.Status200OK)]
        public async Task<IActionResult> Post([FromBody] PriceRequest request)
        {
            var someCalculatedPrice = 199.9;
            return Ok(someCalculatedPrice);
        }
    }
}