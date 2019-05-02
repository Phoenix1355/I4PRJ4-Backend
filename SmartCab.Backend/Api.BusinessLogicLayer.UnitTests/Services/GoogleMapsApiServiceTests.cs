using System;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Api.BusinessLogicLayer.Interfaces;
using Api.BusinessLogicLayer.Services;
using Api.DataAccessLayer.Models;
using CustomExceptions;
using Microsoft.Extensions.Configuration;
using NSubstitute;
using NSubstitute.ExceptionExtensions;
using NUnit.Framework;

namespace Api.BusinessLogicLayer.UnitTests.Services
{
    [TestFixture]
    public class GoogleMapsApiServiceTests
    {
        #region Setup and fields

        private const string DistanceMatrixUrl = "DistanceMatrixUrl";
        private const string DistanceMatrixUrlValue = "http://distancematrixurl.com";
        private const string GeocodingUrl = "GeocodingUrl";
        private const string GeocodingUrlValue = "http://geocodingurl.com";
        private const string GoogleMapsApiKey = "GoogleMapsApiKey";
        private const string GoogleMapsApiKeyValue = "Some-secret-key";

        private const string OkStatus = "OK";
        private const string NotOkStatus = "NOT OK";
        private const double ValidDistance = 1000; //1 km

        private IHttpClient _client;
        private GoogleMapsApiService _googleMapsApiService;


        [SetUp]
        public void Setup()
        {
            var configuration = Substitute.For<IConfiguration>();
            configuration[DistanceMatrixUrl].Returns(DistanceMatrixUrlValue);
            configuration[GeocodingUrl].Returns(GeocodingUrlValue);
            configuration[GoogleMapsApiKey].Returns(GoogleMapsApiKeyValue);
            _client = Substitute.For<IHttpClient>();
            _googleMapsApiService = new GoogleMapsApiService(configuration, _client);
        }

            #endregion

        #region GetDistanceInKm tests

        [Test]
        public void GetDistanceInKmAsync_HttpResponseCodeNotASuccess_ThrowsException()
        {
            var response = new HttpResponseMessage {StatusCode = HttpStatusCode.BadRequest};
            response.Content = new StringContent("some error");

            _client.GetAsync(null).ReturnsForAnyArgs(response);
            var startDest = "Some address";
            var endDest = "Another address";

            Assert.That(()=>_googleMapsApiService.GetDistanceInKmAsync(startDest, endDest), Throws.TypeOf<Exception>());
        }

        [Test]
        public void GetDistanceInKmAsync_DistanceMatrixNotOk_ThrowsGoogleMapsApiException()
        {
            var response = new HttpResponseMessage { StatusCode = HttpStatusCode.OK };
            response.Content = new StringContent(GetDistanceMatrixRequestContent(OkStatus, NotOkStatus, ValidDistance));

            _client.GetAsync(null).ReturnsForAnyArgs(response);
            var startDest = "Some address";
            var endDest = "Another address";

            Assert.That(() => _googleMapsApiService.GetDistanceInKmAsync(startDest, endDest), Throws.TypeOf<GoogleMapsApiException>());
        }

        [TestCase(0)]
        [TestCase(-1)]
        [TestCase(-1000)]
        [TestCase(-10000)]
        [TestCase(-100000)]
        public void GetDistanceInKmAsync_InvalidDistanceInDistanceMatrix_ThrowsGoogleMapsApiException(double distanceInMeters)
        {
            var response = new HttpResponseMessage { StatusCode = HttpStatusCode.OK };
            response.Content = new StringContent(GetDistanceMatrixRequestContent(OkStatus, OkStatus, distanceInMeters));

            _client.GetAsync(null).ReturnsForAnyArgs(response);
            var startDest = "Some address";
            var endDest = "Another address";

            Assert.That(() => _googleMapsApiService.GetDistanceInKmAsync(startDest, endDest), Throws.TypeOf<GoogleMapsApiException>());
        }

        [TestCase(1)]
        [TestCase(1000)]
        [TestCase(10000)]
        [TestCase(100000)]
        public async Task GetDistanceInKmAsync_ValidDistanceInDistanceMatrix_ReturnsTheDistance(double distanceInMeters)
        {
            var response = new HttpResponseMessage { StatusCode = HttpStatusCode.OK };
            response.Content = new StringContent(GetDistanceMatrixRequestContent(OkStatus, OkStatus, distanceInMeters));

            _client.GetAsync(null).ReturnsForAnyArgs(response);
            var startDest = "Some address";
            var endDest = "Another address";

            var returnedDistance = await _googleMapsApiService.GetDistanceInKmAsync(startDest, endDest);

            Assert.That(returnedDistance, Is.EqualTo(distanceInMeters / 1000));
        }

        #endregion

        [Test]
        public void ValidateAddressAsync_HttpResponseCodeNotASuccess_ThrowsException()
        {
            var response = new HttpResponseMessage { StatusCode = HttpStatusCode.BadRequest };
            response.Content = new StringContent("some error");

            _client.GetAsync(null).ReturnsForAnyArgs(response);
            var address = new Address("city",1000,"Street",21);

            Assert.That(() => _googleMapsApiService.ValidateAddressAsync(address), Throws.TypeOf<Exception>());
        }

        [Test]
        public void ValidateAddressAsync_GeocoodingResponseNotOk_ThrowsException()
        {
            var response = new HttpResponseMessage { StatusCode = HttpStatusCode.OK };
            response.Content = new StringContent(GetGeocoodingRequestContent(NotOkStatus, "Location type not relevant for this test"));

            _client.GetAsync(null).ReturnsForAnyArgs(response);
            var address = new Address("city", 1000, "Street", 21);

            Assert.That(() => _googleMapsApiService.ValidateAddressAsync(address), Throws.TypeOf<Exception>());
        }


        [TestCase("GEOMETRIC_CENTER")]
        [TestCase("APPROXIMATE")]
        [TestCase("Some other invalid value")]
        public void ValidateAddressAsync_ReturnedLocationTypeInvalid_ThrowsGoogleMapsApiException(string invalidLocationType)
        {
            var response = new HttpResponseMessage { StatusCode = HttpStatusCode.OK };
            response.Content = new StringContent(GetGeocoodingRequestContent(OkStatus, invalidLocationType));

            _client.GetAsync(null).ReturnsForAnyArgs(response);
            var address = new Address("city", 1000, "Street", 21);

            Assert.That(() => _googleMapsApiService.ValidateAddressAsync(address), Throws.TypeOf<GoogleMapsApiException>());
        }

        [TestCase("ROOFTOP")]
        [TestCase("RANGE_INTERPOLATED")]
        public void ValidateAddressAsync_ReturnedLocationTypeInvalid_DoesNotThrow(string validLocationType)
        {
            var response = new HttpResponseMessage { StatusCode = HttpStatusCode.OK };
            response.Content = new StringContent(GetGeocoodingRequestContent(OkStatus, validLocationType));

            _client.GetAsync(null).ReturnsForAnyArgs(response);
            var address = new Address("city", 1000, "Street", 21);

            Assert.That(() => _googleMapsApiService.ValidateAddressAsync(address), Throws.Nothing);
        }



        #region Helper methods

        /// <summary>
        /// Returns a string that represents the content of a http response from the Google Maps Api returning af single row of data.
        /// <remarks>
        /// Such a response has two status'. One for the entire response. If it's a valid response the status will be 'OK'.<br/>
        /// Each row in the matrix also has a status. If the row is valid the status will be 'OK'.
        /// </remarks>
        /// </summary>
        /// <param name="googleMapsReponseStatus">The status of the entire response.</param>
        /// <param name="distanceMatrixStatus">The status of the first row.</param>
        /// <param name="distance">The distance returned for the row.</param>
        /// <returns></returns>
        private string GetDistanceMatrixRequestContent(string googleMapsReponseStatus, string distanceMatrixStatus, double distance)
        {
            return 
                "{\n   \"destination_addresses\" : [ \"Ridderstræde 1, 8000 Aarhus, Denmark\" ],\n   \"origin_addresses\" : [ \"Lundbyesgade 8, 8000 Aarhus, Denmark\" ],\n   \"rows\" : [\n      {\n         \"elements\" : [\n            {\n               \"distance\" : {\n                  \"text\" : \"2.0 km\",\n                  \"value\" : " + distance + "\n               },\n               \"duration\" : {\n                  \"text\" : \"9 mins\",\n                  \"value\" : 528\n               },\n               \"status\" : \"" + googleMapsReponseStatus + "\"\n            }\n         ]\n      }\n   ],\n   \"status\" : \"" + distanceMatrixStatus + "\"\n}\n";
        }

        private string GetGeocoodingRequestContent(string googleMapsResponseStatus, string locationType)
        {
            return
                "{\n   \"results\" : [\n      {\n         \"address_components\" : [\n            {\n               \"long_name\" : \"8\",\n               \"short_name\" : \"8\",\n               \"types\" : [ \"street_number\" ]\n            },\n            {\n               \"long_name\" : \"Lundbyesgade\",\n               \"short_name\" : \"Lundbyesgade\",\n               \"types\" : [ \"route\" ]\n            },\n            {\n               \"long_name\" : \"Aarhus C\",\n               \"short_name\" : \"Aarhus C\",\n               \"types\" : [ \"political\", \"sublocality\", \"sublocality_level_1\" ]\n            },\n            {\n               \"long_name\" : \"Aarhus\",\n               \"short_name\" : \"Aarhus\",\n               \"types\" : [ \"locality\", \"political\" ]\n            },\n            {\n               \"long_name\" : \"Denmark\",\n               \"short_name\" : \"DK\",\n               \"types\" : [ \"country\", \"political\" ]\n            },\n            {\n               \"long_name\" : \"8000\",\n               \"short_name\" : \"8000\",\n               \"types\" : [ \"postal_code\" ]\n            }\n         ],\n         \"formatted_address\" : \"Lundbyesgade 8, 8000 Aarhus, Denmark\",\n         \"geometry\" : {\n            \"location\" : {\n               \"lat\" : 56.15564,\n               \"lng\" : 10.1959527\n            },\n            \"location_type\" : \"" + locationType + "\",\n            \"viewport\" : {\n               \"northeast\" : {\n                  \"lat\" : 56.15698898029149,\n                  \"lng\" : 10.1973016802915\n               },\n               \"southwest\" : {\n                  \"lat\" : 56.1542910197085,\n                  \"lng\" : 10.1946037197085\n               }\n            }\n         },\n         \"place_id\" : \"ChIJcw2u5-w_TEYR6yOKMFVe_9c\",\n         \"plus_code\" : {\n            \"compound_code\" : \"554W+79 Aarhus, Aarhus Municipality, Denmark\",\n            \"global_code\" : \"9F8G554W+79\"\n         },\n         \"types\" : [ \"street_address\" ]\n      }\n   ],\n   \"status\" : \"" + googleMapsResponseStatus + "\"\n}\n";
        }

        #endregion
    }
}