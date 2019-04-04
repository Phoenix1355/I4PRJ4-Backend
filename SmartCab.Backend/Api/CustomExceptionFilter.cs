using System.Diagnostics;
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
            var exceptionMessage = context.Exception.Message;

            //Contains a generic description --> no sensitive information
            var error = new ErrorResponse();

            //If the exception is a custom exception, then set the errorResponse
            //to the exception message (it will be stripped of sensitive information.
            if (exceptionType == typeof(IdentityException) ||
                exceptionType == typeof(InsufficientFundsException) ||
                exceptionType == typeof(MultipleOrderException) ||
                exceptionType == typeof(UserIdInvalidException))
            {
                error = new ErrorResponse(exceptionMessage);
            }

            //Write error to response
            response.ContentType = "application/json";
            var json = JsonConvert.SerializeObject(error);
            response.WriteAsync(json);

            //Finally write to the debugger
            Debug.WriteLine(exceptionMessage);
        }
    }
}