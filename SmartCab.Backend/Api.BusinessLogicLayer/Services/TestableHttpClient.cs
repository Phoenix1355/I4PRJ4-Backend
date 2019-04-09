using System;
using System.Net.Http;
using Api.BusinessLogicLayer.Interfaces;

namespace Api.BusinessLogicLayer.Services
{
    /// <summary>
    /// This class i made solely for the purpose of testing classes that use an HttpClient.
    /// </summary>
    /// <remarks>
    /// The class HttpClient does not implement an interface. For testing purposes an<br/>
    /// interface with the relevant methods has been created (IHttpClient).<br/>
    /// This makes it possible to unit test our custom client, hence the name<br/>
    /// "TestableHttpClient".
    /// </remarks>
    public class TestableHttpClient : HttpClient, IHttpClient
    {

    }
}