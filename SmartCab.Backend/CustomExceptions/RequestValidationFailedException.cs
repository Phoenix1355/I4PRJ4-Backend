using System;

namespace CustomExceptions
{
    /// <summary>
    /// This exceptions is thrown whenever a request failed validation.
    /// </summary>
    public class RequestValidationFailedException : Exception
    {
        public RequestValidationFailedException()
        {

        }

        public RequestValidationFailedException(string message) : base(message)
        {

        }

        public RequestValidationFailedException(string message, Exception inner) : base(message, inner)
        {

        }
    }
}