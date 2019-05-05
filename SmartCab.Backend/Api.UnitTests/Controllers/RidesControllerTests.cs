using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Api.BusinessLogicLayer;
using Api.BusinessLogicLayer.Enums;
using Api.BusinessLogicLayer.Interfaces;
using Api.BusinessLogicLayer.Requests;
using Api.BusinessLogicLayer.Responses;
using Api.Controllers;
using CustomExceptions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NSubstitute;
using NUnit.Framework;

namespace Api.UnitTests.Controllers
{
    [TestFixture]
    public class RidesControllerTests
    {
        private IRideService _rideService;
        private RidesController _ridesController;

        [SetUp]
        public void Setup()
        {
            _rideService = Substitute.For<IRideService>();
            _ridesController = new RidesController(_rideService);
        }

        [Test]
        public async Task Create_Success_ReturnsOkResponse()
        {
            _rideService.AddRideAsync(null, null).ReturnsForAnyArgs(new CreateRideResponse());

            //Setup the user so we have access to the claim stored in 'Constants.UserIdClaim'
            //Source: https://hudosvibe.net/post/mock-user-identity-in-asp.net-mvc-core-controller-tests
            _ridesController.ControllerContext = new ControllerContext
            {
                HttpContext = new DefaultHttpContext
                {
                    User = new ClaimsPrincipal(new ClaimsIdentity(new Claim[]
                    {
                        new Claim(Constants.UserIdClaim, "SomeCustomerId") //This would be the id of the customer 
                    }))
                }
            };

            //Authorization would normally come from the header, but we have set the 'User' property above, so the value
            //of the authorization param is not needed in this test.(the authorization param is purely added to be able
            //to make the swagger documentation...
            var response = await _ridesController.Create(null, null) as ObjectResult;

            Assert.That(response.StatusCode, Is.EqualTo(StatusCodes.Status200OK));
        }

        [Test]
        public void Create_CustomerIdIsEmpty_ThrowsUserIdInvalidException()
        {
            //So this time we do not set the Controller Context --> The user is null --> customer id is invalid
            _rideService.AddRideAsync(null, null).ReturnsForAnyArgs(new CreateRideResponse());

            //Setup the user so we have access to the claim stored in 'Constants.UserIdClaim'
            //Source: https://hudosvibe.net/post/mock-user-identity-in-asp.net-mvc-core-controller-tests
            _ridesController.ControllerContext = new ControllerContext
            {
                HttpContext = new DefaultHttpContext
                {
                    User = new ClaimsPrincipal(new ClaimsIdentity(new Claim[]
                    {
                        new Claim(Constants.UserIdClaim, "") //Id set to empty string 
                    }))
                }
            };

            Assert.That(() => _ridesController.Create(null, null), Throws.TypeOf<UserIdInvalidException>());
        }

        [Test]
        public void Create_CustomerIdIsNull_ThrowsUserIdInvalidException()
        {
            //So this time we do not set the Controller Context --> The user is null --> customer id is invalid
            _rideService.AddRideAsync(null, null).ReturnsForAnyArgs(new CreateRideResponse());

            //Setup the user so we have access to the claim stored in 'Constants.UserIdClaim'
            //Source: https://hudosvibe.net/post/mock-user-identity-in-asp.net-mvc-core-controller-tests
            _ridesController.ControllerContext = new ControllerContext
            {
                HttpContext = new DefaultHttpContext
                {
                    User = new ClaimsPrincipal(new ClaimsIdentity()) //the claim is not set at all --> it will be null
                }
            };

            Assert.That(() => _ridesController.Create(null, null), Throws.TypeOf<UserIdInvalidException>());
        }

        #region Price

        [Test]
        public async Task Price_Success_ReturnsOkResponse()
        {
            decimal dec = 1;
            _rideService.CalculatePriceAsync(null, null, RideType.SoloRide).ReturnsForAnyArgs(dec);

            //Setup the user so we have access to the claim stored in 'Constants.UserIdClaim'
            _ridesController.ControllerContext = new ControllerContext
            {
                HttpContext = new DefaultHttpContext
                {
                    User = new ClaimsPrincipal(new ClaimsIdentity(new Claim[]
                    {
                        new Claim(Constants.UserIdClaim, "SomeCustomerId") //This would be the id of the customer 
                    }))
                }
            };

            var request = new PriceRequest
            {
                EndAddress = null,
                RideType = RideType.SoloRide,
                StartAddress = null
            };

            var response = await _ridesController.Price(null, request) as ObjectResult;

            Assert.That(response.StatusCode, Is.EqualTo(StatusCodes.Status200OK));
        }

        [Test]
        public void Price_CustomerIdIsEmpty_ThrowsUserIdInvalidException()
        {
            decimal dec = 1;
            _rideService.CalculatePriceAsync(null, null, RideType.SoloRide).ReturnsForAnyArgs(dec);

            //Setup the user so we have access to the claim stored in 'Constants.UserIdClaim'
            _ridesController.ControllerContext = new ControllerContext
            {
                HttpContext = new DefaultHttpContext
                {
                    User = new ClaimsPrincipal(new ClaimsIdentity(new Claim[]
                    {
                        new Claim(Constants.UserIdClaim, "") //This would be the id of the customer 
                    }))
                }
            };

            var request = new PriceRequest
            {
                EndAddress = null,
                RideType = RideType.SoloRide,
                StartAddress = null
            };

            Assert.That(() => _ridesController.Price(null, request), Throws.TypeOf<UserIdInvalidException>());
        }

        [Test]
        public async Task Price_SuccessForShareRide_ReturnsOkResponse()
        {
            decimal dec = 2;
            _rideService.CalculatePriceAsync(null, null, RideType.SharedRide).ReturnsForAnyArgs(dec);

            //Setup the user so we have access to the claim stored in 'Constants.UserIdClaim'
            _ridesController.ControllerContext = new ControllerContext
            {
                HttpContext = new DefaultHttpContext
                {
                    User = new ClaimsPrincipal(new ClaimsIdentity(new Claim[]
                    {
                        new Claim(Constants.UserIdClaim, "CustomerIdNewGuy") //This would be the id of the customer 
                    }))
                }
            };

            var request = new PriceRequest
            {
                EndAddress = null,
                RideType = RideType.SharedRide,
                StartAddress = null
            };

            var response = await _ridesController.Price(null, request) as ObjectResult;

            Assert.That(response.StatusCode, Is.EqualTo(StatusCodes.Status200OK));
        }

        [Test]
        public void Price_CustomerIdIsNull_ThrowsUserIdInvalidException()
        {
            decimal dec = 1;
            _rideService.CalculatePriceAsync(null, null, RideType.SoloRide).ReturnsForAnyArgs(dec);

            //Setup the user so we have access to the claim stored in 'Constants.UserIdClaim'
            _ridesController.ControllerContext = new ControllerContext
            {
                HttpContext = new DefaultHttpContext
                {
                    User = new ClaimsPrincipal(new ClaimsIdentity()) // The claim is not set, thereby null
                }
            };

            var request = new PriceRequest
            {
                EndAddress = null,
                RideType = RideType.SoloRide,
                StartAddress = null
            };

            Assert.That(() => _ridesController.Price(null, request), Throws.TypeOf<UserIdInvalidException>());
        }

        #endregion
    }
}