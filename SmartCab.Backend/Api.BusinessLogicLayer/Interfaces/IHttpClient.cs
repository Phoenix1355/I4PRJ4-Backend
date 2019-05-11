using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace Api.BusinessLogicLayer.Interfaces
{
    /// <summary>
    /// Httpclient for testability. 
    /// </summary>
    public interface IHttpClient
    {
        Task<HttpResponseMessage> GetAsync(Uri requestUri);
    }
}