using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;
using Api.BusinessLogicLayer.Enums;
using Api.BusinessLogicLayer.Interfaces;
using Api.BusinessLogicLayer.Requests;
using Api.DataAccessLayer;
using Api.DataAccessLayer.Models;
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
            ApplicationContext context, ITaxiCompanyService taxiCompanyService, IRideService rideService, ICustomerService customerService)
        {
            _unitOfWork = unitOfWork;
            _context = context;
            _taxiCompanyService = taxiCompanyService;
            _rideService = rideService;
            _customerService = customerService;
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
            
            //Customer
            await _customerService.AddCustomerAsync(new RegisterRequest()
            {
                Name = "Tester UC 1.1",
                PhoneNumber = "12345678",
                Email = "hello@gmail.com",
                Password = "Qwer1234#"
            });
            
            //Find above inserted customer
            await _customerService.DepositAsync(new DepositRequest()
            {
                Deposit = 1000
            }, _context.Customers.First(c => c.Email == "hello@gmail.com").Id);


            await _customerService.AddCustomerAsync(new RegisterRequest()
            {
                Name = "Tester UC 2 og 4.1",
                PhoneNumber = "12345678",
                Email = "test1234@gmail.com",
                Password = "Qwer1234!"
            });
            
            await _customerService.AddCustomerAsync(new RegisterRequest()
            {
                Name = "Tester UC 2.1 og 3",
                PhoneNumber = "12345678",
                Email = "test1111@gmail.com",
                Password = "Qwer1111#"
            });

            await _customerService.AddCustomerAsync(new RegisterRequest()
            {
                Name = "Per Henriksen",
                PhoneNumber = "99998888",
                Email = "per@henriksen.dk",
                Password = "Qwer1111!"
            });

            //Second customer section
            await _customerService.AddCustomerAsync(new RegisterRequest()
            {
                Name = "Tester UC 10, 10.1, 10.2",
                PhoneNumber = "12345678",
                Email = "hans@jensen.dk",
                Password = "LukMigInd24!"
            });

            //Find above inserted customer
            await _customerService.DepositAsync(new DepositRequest()
            {
                Deposit = 1000
            }, _context.Customers.First(c => c.Email == "hans@jensen.dk").Id);

            await _customerService.AddCustomerAsync(new RegisterRequest()
            {
                Name = "Tester 10.3",
                PhoneNumber = "12345678",
                Email = "michael@firma.dk",
                Password = "LukMigInd24!"
            });

            await _customerService.AddCustomerAsync(new RegisterRequest()
            {
                Name = "Tester 11, 11.1, 11.2",
                PhoneNumber = "12345678",
                Email = "jesper@fodbold.dk",
                Password = "LukMigInd24!"
            });

            
            await _customerService.DepositAsync(new DepositRequest()
            {
                Deposit = 1000
            }, _context.Customers.First(c => c.Email == "jesper@fodbold.dk").Id);


            await _customerService.AddCustomerAsync(new RegisterRequest()
            {
                Name = "Tester 11.3",
                PhoneNumber = "12345678",
                Email = "axel@axel.dk",
                Password = "LukMigInd24!"
            });

            await _customerService.AddCustomerAsync(new RegisterRequest()
            {
                Name = "Tester 11.4",
                PhoneNumber = "12345678",
                Email = "holger@madsen.dk",
                Password = "LukMigInd24!"
            });

            await _customerService.DepositAsync(new DepositRequest()
            {
                Deposit = 1000
            }, _context.Customers.First(c => c.Email == "holger@madsen.dk").Id);

            
            await _customerService.AddCustomerAsync(new RegisterRequest()
            {
                Name = "Tester UC 13,14,15",
                PhoneNumber = "12345678",
                Email = "kunde@kunde.dk",
                Password = "DFgh5432*"
            });

            await _customerService.AddCustomerAsync(new RegisterRequest()
            {
                Name = "Tester UC 16",
                PhoneNumber = "12345678",
                Email = "henning@poulsen.dk",
                Password = "Fodbold321#"
            });


            await _customerService.DepositAsync(new DepositRequest()
            {
                Deposit = 1000
            }, _context.Customers.First(c => c.Email == "henning@poulsen.dk").Id);

            //Create taxicompanies

            await _taxiCompanyService.AddTaxiCompanyAsync(new RegisterRequest()
            {
                Name = "Tester UC 6.1",
                PhoneNumber = "12345678",
                Email = "horsens@taxi.dk",
                Password = "8888Taxi!"
            });

            await _taxiCompanyService.AddTaxiCompanyAsync(new RegisterRequest()
            {
                Name = "Tester UC 7,8",
                PhoneNumber = "12345678",
                Email = "pallevognmand@gmail.com",
                Password = "Klok9876!"
            });

            
            await _taxiCompanyService.AddTaxiCompanyAsync(new RegisterRequest()
            {
                Name = "Hans Eriksen",
                PhoneNumber = "44668822",
                Email = "hans@eriksen.dk",
                Password = "Rtyu6543!"
            });

            await _taxiCompanyService.AddTaxiCompanyAsync(new RegisterRequest()
            {
                Name = "Tester 9.1",
                PhoneNumber = "12345678",
                Email = "vognmandper@hotmail.com",
                Password = "RHule4488&"
            });

            await _taxiCompanyService.AddTaxiCompanyAsync(new RegisterRequest()
            {
                Name = "Tester 12",
                PhoneNumber = "12345678",
                Email = "bruger@nybruger.dk",
                Password = "LukMigInd24!"
            });

            await _taxiCompanyService.AddTaxiCompanyAsync(new RegisterRequest()
            {
                Name = "Tester T UC 13,14,15",
                PhoneNumber = "12345678",
                Email = "henrik@jubii.dk",
                Password = "Flyverskjul3#"
            });
            
            //Insert rides
            await _rideService.AddRideAsync(new CreateRideRequest()
            {
                RideType = RideType.SharedRide,
                ConfirmationDeadline = DateTime.Now.AddHours(1),
                StartDestination = new Address("Aarhus C", 8000, "Ridderstræde", 1),
                EndDestination = new Address("Aarhus C", 8000, "Lundbyesgade", 8),
                PassengerCount = 1,
                DepartureTime = DateTime.Now.AddHours(1)
            }, _context.Customers.First(c => c.Email == "hello@gmail.com").Id);
            
            await _rideService.AddRideAsync(new CreateRideRequest()
            {
                RideType = RideType.SoloRide,
                ConfirmationDeadline = DateTime.Now.AddHours(1),
                StartDestination = new Address("Aarhus", 8000, "Samsøgade", 75),
                EndDestination = new Address("Egå", 8250, "Skæring Sandager", 40),
                PassengerCount = 1,
                DepartureTime = DateTime.Now.AddHours(1)
            }, _context.Customers.First(c => c.Email == "hello@gmail.com").Id);

            await _rideService.AddRideAsync(new CreateRideRequest()
            {
                RideType = RideType.SoloRide,
                ConfirmationDeadline = DateTime.Now.AddHours(1),
                StartDestination = new Address("Aarhus", 8200, "Finlandsgade", 20),
                EndDestination = new Address("Aarhus", 8210, "Snogebæksvej", 21),
                PassengerCount = 2,
                DepartureTime = DateTime.Now.AddHours(1)
            }, _context.Customers.First(c => c.Email == "hello@gmail.com").Id);


           await _rideService.AddRideAsync(new CreateRideRequest()
            {
                    RideType = RideType.SoloRide,
                    ConfirmationDeadline = DateTime.Now.AddHours(1),
                    StartDestination = new Address("Egå", 8250, "Kaprifolievej", 6),
                    EndDestination = new Address("Tranbjerg", 8310, "Egevænget", 122),
                    PassengerCount = 1,
                    DepartureTime = DateTime.Now.AddHours(1)
            }, _context.Customers.First(c => c.Email == "henning@poulsen.dk").Id);
           
            return Ok("Created accepttest preconditions");
        }


    }
}