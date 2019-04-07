using System;

namespace CustomExceptions
{
    /// <summary>
    /// This exception is thrown whenever an error related to the identity framework occurs.
    /// <remarks>
    /// Errors could happen is a user is tried created with an email that is already in use etc.
    /// </remarks>
    /// </summary>
    public class IdentityException : Exception
    {
        public IdentityException(string message) : base(message)
        {

        }
    }
}