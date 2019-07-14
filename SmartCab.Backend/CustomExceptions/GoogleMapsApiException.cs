using System;

namespace CustomExceptions
{
    public class GoogleMapsApiException : Exception
    {
        public GoogleMapsApiException(string message) : base(message)
        {

        }
    }
}