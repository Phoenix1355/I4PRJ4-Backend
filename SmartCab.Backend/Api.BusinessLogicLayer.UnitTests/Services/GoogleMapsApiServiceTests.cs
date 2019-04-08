using System;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Api.BusinessLogicLayer.Interfaces;
using Api.BusinessLogicLayer.Services;
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
    }
}