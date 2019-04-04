using System;

namespace CustomExceptions
{
    /// <summary>
    /// This exceptions is thrown whenever a JSON Web Token contains an invalid "UserId" claim.
    /// </summary>
    public class UserIdInvalidException : Exception
    {
        public UserIdInvalidException()
        {
            
        }

        public UserIdInvalidException(string message) : base(message)
        {

        }

        public UserIdInvalidException(string message, Exception inner) : base(message, inner)
        {

        }
    }
}
