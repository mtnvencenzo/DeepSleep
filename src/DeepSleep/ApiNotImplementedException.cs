namespace DeepSleep
{
    using System;

    /// <summary>
    /// 
    /// </summary>
    /// <seealso cref="System.Exception" />
    public class ApiNotImplementedException : ApiException
    {
        /// <summary>Initializes a new instance of the <see cref="ApiNotImplementedException"/> class.</summary>
        public ApiNotImplementedException() : base() { }

        /// <summary>Initializes a new instance of the <see cref="ApiNotImplementedException"/> class.</summary>
        /// <param name="message">The message that describes the error.</param>
        public ApiNotImplementedException(string message) : base(message) { }

        /// <summary>Initializes a new instance of the <see cref="ApiNotImplementedException"/> class.</summary>
        /// <param name="message">The error message that explains the reason for the exception.</param>
        /// <param name="innerException">The exception that is the cause of the current exception, or a null reference (Nothing in Visual Basic) if no inner exception is specified.</param>
        public ApiNotImplementedException(string message, Exception innerException) : base(message, innerException) { }

        /// <summary>Gets the HTTP status.</summary>
        /// <value>The HTTP status.</value>
        public override int HttpStatus => 501;
    }
}
