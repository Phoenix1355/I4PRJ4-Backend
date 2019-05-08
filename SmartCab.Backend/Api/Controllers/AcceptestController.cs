using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;
using Api.BusinessLogicLayer.Interfaces;
using Api.BusinessLogicLayer.Requests;
using Api.DataAccessLayer;
using Api.DataAccessLayer.UnitOfWork;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers
{
    /// <summary>
    /// For accept test purposes only
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class AcceptestController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ICustomerService _customerService;
        private readonly ITaxiCompanyService _taxiCompanyService;
        private readonly IRideService _rideService;
        private readonly ApplicationContext _context;
        /// <summary>
        /// Directly inject unit of work
        /// </summary>
        /// <param name="unitOfWork"></param>
        public AcceptestController(IUnitOfWork unitOfWork,
            ApplicationContext context, ITaxiCompanyService taxiCompanyService, IRideService rideService)
        {
            _unitOfWork = unitOfWork;
            _context = context;
            _taxiCompanyService = taxiCompanyService;
            _rideService = rideService;
        }

        [HttpGet]
        public async Task<ActionResult> CleanAndCreate()
        {
            //Delete all customers and taxi companies
            _context.Users.RemoveRange(_context.Users);
            //Delete all orders
            _context.Orders.RemoveRange(_context.Orders);
            //Delete all rides
            _context.Rides.RemoveRange(_context.Rides);
            //Save these changes
            _context.SaveChanges();


            await _customerService.AddCustomerAsync(new RegisterRequest()
            {
                Name = "Name",
                PhoneNumber = "Number",
                Email = "Email@Email.com",
                Password = "Qwer111!"
            });


            //Create required data


           await _unitOfWork.SaveChangesAsync();
           return Ok("Created accepttest preconditions");
        }


    }
}