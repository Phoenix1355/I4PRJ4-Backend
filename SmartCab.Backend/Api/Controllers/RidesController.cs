using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Api.BusinessLogicLayer;
using Api.BusinessLogicLayer.DataTransferObjects;
using Api.BusinessLogicLayer.Interfaces;
using Api.BusinessLogicLayer.Requests;
using Api.BusinessLogicLayer.Responses;
using Api.DataAccessLayer.Models;
using Api.ErrorHandling;
using Api.Requests;
using AutoMapper;
using Castle.DynamicProxy.Generators.Emitters.SimpleAST;
using CustomExceptions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RidesController : ControllerBase
    {
        private readonly IRideService _rideService;

        public RidesController(IRideService rideService)
        {
            _rideService = rideService;
        }

       /// <summary>
        /// Creates a new ride and ties the ride to the customer sending the request.
        /// </summary>
        /// <remarks>
        /// Required role: "Customer".<br/>
        /// If the ride is a solo ride the type should have the value "SoloRide".<br/>
        /// If the ride is a shared ride the type must have the value "SharedRide".
        /// </remarks>
        /// <param name="authorization">A valid JWT token.</param>
        /// <param name="request">Information about the ride that should be updated.</param>
        /// <returns>The created ride.</returns>
        /// <response code="400">If the supplied request wasn't valid.</response>
        /// <response code="401">If the customer was not logged in already (token was expired)</response>
        /// <response code="500">If an internal server error occured.</response>
        [Authorize(Roles = nameof(Customer))]
        [Produces("application/json")]
        [ProducesResponseType(typeof(CreateRideResponse), StatusCodes.Status200OK)]
        [Route("[action]")]
        [HttpPost]
        public async Task<IActionResult> Create([FromHeader] string authorization, [FromBody] CreateRideRequest request)
        {
            //Get the customerId that is stored as a claim in the token
            var customerId = User.Claims.FirstOrDefault(x => x.Type == Constants.UserIdClaim)?.Value;

            //This should never happen, but better safe than sorry
            if (string.IsNullOrEmpty(customerId))
            {
                throw new UserIdInvalidException(
                    $"The supplied JSON Web Token does not contain a valid value in the '{ Constants.UserIdClaim }' claim.");
            }

            var response = await _rideService.AddRideAsync(request, customerId);
            return Ok(response);
        }

        /// <summary>
        /// Calculates and returns the price for a taxi ride based on two addresses.
        /// </summary>
        /// <remarks>
        /// Currently now authorization is required to make this request.
        /// </remarks>
        /// <param name="authorization">A valid JWT token.</param>
        /// <param name="request">Consists of an start address and an end address.</param>
        /// <returns> the price</returns>
        /// <response code="400">If the supplied request wasn't valid.</response>
        /// <response code="401">If the token was expired</response>
        /// <response code="500">If an internal server error occured.</response>
        [Authorize(Roles = nameof(Customer))]
        [Produces("application/json")]
        [Route("[action]")]
        [ProducesResponseType(typeof(PriceResponse), StatusCodes.Status200OK)]
        [HttpPost]
        public async Task<IActionResult> Price([FromHeader] string authorization, [FromBody] PriceRequest request)
        {
            //Get the customerId, stored as a claim in the token
            var customerId = User.Claims.FirstOrDefault(x => x.Type == Constants.UserIdClaim)?.Value;

            //should never happen
            if (string.IsNullOrEmpty(customerId))
            {
                throw new UserIdInvalidException(
                    $"The supplied JSON Web Token does not contain a valid value in the '{ Constants.UserIdClaim }' claim.");
            }

            var calculatedPrice = await
                _rideService.CalculatePriceAsync(request.StartAddress, request.EndAddress, request.RideType);

            var response = new PriceResponse
            {
                Price = calculatedPrice
            };

            return Ok(response);
        }
    }
}