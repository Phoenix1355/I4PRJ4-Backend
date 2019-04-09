using System;

namespace CustomExceptions
{
    public class MultipleOrderException : Exception
    {
        public MultipleOrderException(string message) : base(message)
        {

        }
    }
}