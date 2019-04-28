﻿using System;
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
        /// Returns all available details about a specific ride.
        /// </summary>
        /// <param name="authorization">A valid JWT token that is associated with a taxi company account.</param>
        /// <param name="id">The id of the ride for which the details is returned.</param>
        /// <returns>Details about a specific ride.</returns>
        /// <response code="401">
        /// An invalid JWT token was provided in the authorization header.<br/>
        /// This can happen if the supplied token is expired or because the user associated with the token does not have the required role needed to make the request.
        /// </response>
        /// <response code="500">If an internal server error occured.</response>
        [Produces("application/json")]
        [Route("{id}/[action]")]
        [HttpGet]
        [ProducesResponseType(typeof(SoloRide), StatusCodes.Status200OK)]
        public async Task<IActionResult> Details([FromHeader] string authorization, int id)
        {
            try
            {
                throw new NotImplementedException("Not implemented yet");
                var ride = new Ride(); //todo fetch details
                return Ok(ride);
            }
            catch (Exception e)
            {
                Debug.WriteLine(e.Message);
                var response = new ErrorMessage();
                return StatusCode(StatusCodes.Status500InternalServerError, response);
            }
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
    }
}