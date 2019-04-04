using System;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics;
using System.Net;
using Api.BusinessLogicLayer.Responses;
using CustomExceptions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Filters;
using Newtonsoft.Json;

namespace Api.Errorhandling
{
    /// <summary>
    /// This class is used as a filter in the middleware. Used to return custom messages when exceptions are thrown.
    /// </summary>
    /// <remarks>
    /// Source: https://docs.microsoft.com/en-us/aspnet/core/mvc/controllers/filters?view=aspnetcore-2.2 <br/>
    /// Source: https://www.talkingdotnet.com/global-exception-handling-in-aspnet-core-webapi/
    /// </remarks>
    public class CustomExceptionFilter : IExceptionFilter
    {
        public void OnException(ExceptionContext context)
        {
            var response = context.HttpContext.Response;
            var exceptionType = context.Exception.GetType();
            var exceptionMessage = context.Exception.Message;

            //Contains a generic description --> no sensitive information
            var error = new ErrorMessage();
            var status = StatusCodes.Status500InternalServerError;

            //If the exception is a custom exception, then set the errorResponse
            //to the exception message (it will be stripped of sensitive information.
            if (exceptionType == typeof(IdentityException))
            {
                status = StatusCodes.Status400BadRequest;
                error = new ErrorMessage(exceptionMessage);
            }

            if (exceptionType == typeof(InsufficientFundsException))
            {
                status = StatusCodes.Status400BadRequest;
                error = new ErrorMessage(exceptionMessage);
            }

            if (exceptionType == typeof(MultipleOrderException))
            {
                status = StatusCodes.Status400BadRequest;
                error = new ErrorMessage(exceptionMessage);
            }

            if (exceptionType == typeof(UserIdInvalidException))
            {
                status = StatusCodes.Status401Unauthorized;
                error = new ErrorMessage(exceptionMessage);
            }

            if (exceptionType == typeof(ValidationException))
            {
                status = StatusCodes.Status400BadRequest;
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