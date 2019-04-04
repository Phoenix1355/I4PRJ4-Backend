using System.Collections.Generic;

namespace Api.BusinessLogicLayer.Responses
{
    /// <summary>
    /// This class is used to return custom error in a uniform way to the clients.
    /// </summary>
    /// <remarks>
    /// The structure results in a json object like so:
    ///     {
    ///         "errors": {
    ///             "error": [
    ///                 "Some error description."
    ///             ]
    ///         }
    ///     }
    /// </remarks>
    public class ErrorResponse
    {
        public Dictionary<string, List<string>> Errors { get; set; }

        /// <summary>
        /// Constructor used to create errors with a custom messages.
        /// </summary>
        /// <param name="errorMessage"></param>
        public ErrorResponse(string errorMessage)
        {
            Errors = new Dictionary<string, List<string>>();
            var list = new List<string>
            {
                errorMessage
            };
            Errors.Add("error", list);
        }

        /// <summary>
        /// Constructor used to create errors with a generic message.
        /// </summary>
        public ErrorResponse()
        {
            Errors = new Dictionary<string, List<string>>();
            var list = new List<string>
            {
                "An unknown error occured."
            };
            Errors.Add("error", list);
        }
    }
}