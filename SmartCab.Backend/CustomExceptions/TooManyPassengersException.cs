using System;

namespace CustomExceptions
{
    public class TooManyPassengersException : Exception
    {
        public TooManyPassengersException(string message) : base(message)
        {

        }
    }
}
