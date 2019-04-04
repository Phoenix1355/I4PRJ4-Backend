using System;

namespace CustomExceptions
{
    public class InsufficientFundsException : Exception
    {
        public InsufficientFundsException()
        {

        }

        public InsufficientFundsException(string message) : base(message)
        {

        }

        public InsufficientFundsException(string message, Exception inner) : base(message, inner)
        {

        }
    }
}