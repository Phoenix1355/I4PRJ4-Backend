using System;

namespace CustomExceptions
{
    /// <summary>
    /// This exceptions is thrown whenever a JSON Web Token contains an invalid "UserId" claim.
    /// </summary>
    public class UserIdInTokenInvalid : Exception
    {
        public UserIdInTokenInvalid()
        {
            
        }

        public UserIdInTokenInvalid(string message) : base(message)
        {

        }

        public UserIdInTokenInvalid(string message, Exception inner) : base(message, inner)
        {

        }
    }
}
