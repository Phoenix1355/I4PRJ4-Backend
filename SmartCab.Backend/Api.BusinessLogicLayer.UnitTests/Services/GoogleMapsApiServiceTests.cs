using System;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Api.BusinessLogicLayer.Interfaces;
using Api.BusinessLogicLayer.Services;
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

        [Test]
        public void GetDistanceInKmAsync_ResponseCodeNotASuccess_ThrowsException()
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
            response.Content = new StringContent(GetGoogleMapsRequestContent(OkStatus, NotOkStatus, ValidDistance));

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
            response.Content = new StringContent(GetGoogleMapsRequestContent(OkStatus, OkStatus, distanceInMeters));

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
            response.Content = new StringContent(GetGoogleMapsRequestContent(OkStatus, OkStatus, distanceInMeters));

            _client.GetAsync(null).ReturnsForAnyArgs(response);
            var startDest = "Some address";
            var endDest = "Another address";

            var returnedDistance = await _googleMapsApiService.GetDistanceInKmAsync(startDest, endDest);

            Assert.That(returnedDistance, Is.EqualTo(distanceInMeters / 1000));
        }

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
        private string GetGoogleMapsRequestContent(string googleMapsReponseStatus, string distanceMatrixStatus, double distance)
        {
            return "{\n   \"destination_addresses\" : [ \"Ridderstræde 1, 8000 Aarhus, Denmark\" ],\n   \"origin_addresses\" : [ \"Lundbyesgade 8, 8000 Aarhus, Denmark\" ],\n   \"rows\" : [\n      {\n         \"elements\" : [\n            {\n               \"distance\" : {\n                  \"text\" : \"2.0 km\",\n                  \"value\" : " + distance + "\n               },\n               \"duration\" : {\n                  \"text\" : \"9 mins\",\n                  \"value\" : 528\n               },\n               \"status\" : \"" + googleMapsReponseStatus + "\"\n            }\n         ]\n      }\n   ],\n   \"status\" : \"" + distanceMatrixStatus + "\"\n}\n";
        }
    }
}