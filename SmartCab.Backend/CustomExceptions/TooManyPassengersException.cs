using System;
using System.Collections.Generic;
using System.Text;

namespace CustomExceptions
{
    public class TooManyPassengersException : Exception
    {
        public TooManyPassengersException(string message) : base(message)
        {

        }
    }
}
