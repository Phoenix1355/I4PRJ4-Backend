using System;

namespace CustomExceptions
{
    /// <summary>
    /// This exceptions is thrown whenever a JSON Web Token contains an invalid "UserId" claim.
    /// </summary>
    public class UnexpectedStatusException : Exception
    {
        public UnexpectedStatusException(string message) : base(message)
        {

        }
    }
}
