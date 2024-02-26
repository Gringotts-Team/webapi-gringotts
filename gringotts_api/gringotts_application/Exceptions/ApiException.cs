namespace gringotts_application.Exceptions
{
    /// <summary>
    /// Represents an exception that can be thrown in the API layer. 
    /// </summary>
    public class ApiException : Exception
    {
        // Gets the HTTP status code associated with the exceptions.
        public int StatusCode { get; }
        // Initializes a new instance of the <see cref="ApiException"/> class.
        public ApiException()
        {
        }
        /// <summary>
        /// Initializes a new instance of the <see cref="ApiException"/> class with specified error message.
        /// </summary>
        /// <param name="message">The message that describes the error.</param>
        public ApiException(string message) : base(message)
        {
        }
        /// <summary>
        /// Initializes a new instance of the <see cref="ApiException"/> class with specified error message
        /// and HTTP status code.
        /// </summary>
        /// <param name="message">The message that describes the error.</param>
        /// <param name="statusCode">The HTTP status code associated with the exception.</param>
        public ApiException(string message, int statusCode) : base(message)
        {
            StatusCode = statusCode;
        }

    }
}

