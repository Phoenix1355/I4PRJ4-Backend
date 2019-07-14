using System;

namespace CustomExceptions
{
    public class NegativeDepositException : Exception
    {
        public NegativeDepositException(string message) : base(message)
        {

        }
    }
}