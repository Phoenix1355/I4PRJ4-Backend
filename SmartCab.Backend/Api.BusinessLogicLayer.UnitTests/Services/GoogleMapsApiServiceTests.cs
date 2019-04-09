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
            _googleMapsApiService = new GoogleMapsApiService(configuration);
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
            var distanceMatrixStatus = "Something else than 'OK'";
            var response = new HttpResponseMessage { StatusCode = HttpStatusCode.OK };
            response.Content = new StringContent("{\n   \"destination_addresses\" : [ \"Ridderstræde 1, 8000 Aarhus, Denmark\" ],\n   \"origin_addresses\" : [ \"Lundbyesgade 8, 8000 Aarhus, Denmark\" ],\n   \"rows\" : [\n      {\n         \"elements\" : [\n            {\n               \"distance\" : {\n                  \"text\" : \"2.0 km\",\n                  \"value\" : 1950\n               },\n               \"duration\" : {\n                  \"text\" : \"9 mins\",\n                  \"value\" : 528\n               },\n               \"status\" : \"OK\"\n            }\n         ]\n      }\n   ],\n   \"status\" : \"" + distanceMatrixStatus + "\"\n}\n");

            _client.GetAsync(null).ReturnsForAnyArgs(response);
            var startDest = "Some address";
            var endDest = "Another address";

            Assert.That(() => _googleMapsApiService.GetDistanceInKmAsync(startDest, endDest), Throws.TypeOf<GoogleMapsApiException>());
        }
    }
}