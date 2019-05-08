using System.ComponentModel.DataAnnotations;
using System.Diagnostics;
using CustomExceptions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Filters;
using Newtonsoft.Json;

namespace Api.ErrorHandling
{
    /// <summary>
    /// This class is used as a exception filter in the middleware. Used to return custom messages when exceptions are thrown.
    /// </summary>
    /// <remarks>
    /// Source: https://docs.microsoft.com/en-us/aspnet/core/mvc/controllers/filters?view=aspnetcore-2.2 <br/>
    /// Source: https://www.talkingdotnet.com/global-exception-handling-in-aspnet-core-webapi/
    /// </remarks>
    public class CustomExceptionFilter : IExceptionFilter
    {
        /// <summary>
        /// This method handles exceptions that are thrown during the processing of a request.
        /// </summary>
        /// <param name="context">The exception context that contains information about the thrown exception.</param>
        public void OnException(ExceptionContext context)
        {
            var response = context.HttpContext.Response;
            var exceptionType = context.Exception.GetType();
            var exceptionMessage = context.Exception.Message;

            //If it is not a custom exception use the default construtor that contains a
            //generic error message. This is to hide sensitive information.
            var error = new ErrorMessage();
            var status = StatusCodes.Status500InternalServerError;

            //AllAsync custom exceptions that should return status 400
            if (exceptionType == typeof(GoogleMapsApiException) ||
                exceptionType == typeof(IdentityException) ||
                exceptionType == typeof(InsufficientFundsException) ||
                exceptionType == typeof(ValidationException) ||
                exceptionType == typeof(MultipleOrderException) ||
                exceptionType == typeof(NegativeDepositException) ||
                exceptionType == typeof(UnexpectedStatusException) ||
                exceptionType == typeof(TooManyPassengersException))
            {
                status = StatusCodes.Status400BadRequest;
                error = new ErrorMessage(exceptionMessage);
            }

            //AllAsync custom exceptions that should return status 401
            if (exceptionType == typeof(UserIdInvalidException))
            {
                status = StatusCodes.Status401Unauthorized;
                error = new ErrorMessage(exceptionMessage);
            }
            
            context.ExceptionHandled = true;

            //Write error to response
            response.StatusCode = status;
            response.ContentType = "application/json";
            var json = JsonConvert.SerializeObject(error);
            response.WriteAsync(json);

            //Finally write to the debugger
            Debug.WriteLine(exceptionMessage);
        }
    }
}