using Api.BusinessLogicLayer.Responses;
using CustomExceptions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Filters;
using Newtonsoft.Json;

namespace Api
{
    /// <summary>
    /// Source: https://docs.microsoft.com/en-us/aspnet/core/mvc/controllers/filters?view=aspnetcore-2.2
    /// Source: https://www.talkingdotnet.com/global-exception-handling-in-aspnet-core-webapi/
    /// </summary>
    public class CustomExceptionFilter : IExceptionFilter
    {
        public void OnException(ExceptionContext context)
        {
            var response = context.HttpContext.Response;
            var exceptionType = context.Exception.GetType();
            var message = context.Exception.Message;
            var error = new ErrorResponse(message);

            if (exceptionType == typeof(IdentityException))
            {
                response.ContentType = "application/json";
                var json = JsonConvert.SerializeObject(error);
                response.WriteAsync(json);
            }
        }
    }
}