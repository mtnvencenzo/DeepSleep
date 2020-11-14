﻿namespace DeepSleep
{
    using System;

    /// <summary>
    /// 
    /// </summary>
    /// <seealso cref="System.Exception" />
    public class ApiServiceUnavailableException : Exception
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ApiServiceUnavailableException"/> class.
        /// </summary>
        public ApiServiceUnavailableException() : base() { }

        /// <summary>
        /// Initializes a new instance of the <see cref="ApiServiceUnavailableException"/> class.
        /// </summary>
        /// <param name="message">The message that describes the error.</param>
        public ApiServiceUnavailableException(string message) : base(message) { }

        /// <summary>
        /// Initializes a new instance of the <see cref="ApiServiceUnavailableException"/> class.
        /// </summary>
        /// <param name="message">The error message that explains the reason for the exception.</param>
        /// <param name="innerException">The exception that is the cause of the current exception, or a null reference (Nothing in Visual Basic) if no inner exception is specified.</param>
        public ApiServiceUnavailableException(string message, Exception innerException) : base(message, innerException) { }
    }
}
