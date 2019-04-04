using System;

namespace CustomExceptions
{
    public class MultipleOrderException : Exception
    {
        public MultipleOrderException()
        {

        }

        public MultipleOrderException(string message) : base(message)
        {

        }

        public MultipleOrderException(string message, Exception inner) : base(message, inner)
        {

        }
    }
}