using System;

namespace CustomExceptions
{
    public class GoogleMapsApiException : Exception
    {
        public GoogleMapsApiException()
        {

        }

        public GoogleMapsApiException(string message) : base(message)
        {

        }

        public GoogleMapsApiException(string message, Exception inner) : base(message, inner)
        {

        }
    }
}