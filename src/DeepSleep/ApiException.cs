namespace DeepSleep
{
    using System;

    /// <summary>
    /// 
    /// </summary>
    /// <seealso cref="System.Exception" />
    public abstract class ApiException : Exception
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ApiNotImplementedException"/> class.
        /// </summary>
        public ApiException() : base() { }

        /// <summary>
        /// Initializes a new instance of the <see cref="ApiNotImplementedException"/> class.
        /// </summary>
        /// <param name="message">The message that describes the error.</param>
        public ApiException(string message) : base(message) { }

        /// <summary>
        /// Initializes a new instance of the <see cref="ApiNotImplementedException"/> class.
        /// </summary>
        /// <param name="message">The error message that explains the reason for the exception.</param>
        /// <param name="innerException">The exception that is the cause of the current exception, or a null reference (Nothing in Visual Basic) if no inner exception is specified.</param>
        public ApiException(string message, Exception innerException) : base(message, innerException) { }

        /// <summary>Gets the HTTP status.</summary>
        /// <value>The HTTP status.</value>
        public abstract int HttpStatus { get; }
    }
}
