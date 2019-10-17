namespace DeepSleep.JsonPatch
{
    using System;

    /// <summary>
    /// 
    /// </summary>
    /// <seealso cref="System.Exception" />
    public class JsonPatchOperationException : Exception
    {
        #region Constructors & Initialization

        /// <summary>Initializes a new instance of the <see cref="JsonPatchOperationException"/> class.</summary>
        public JsonPatchOperationException(JsonPatchOperationError error) : base("An eror has occured, review he Error property for more details")
        {
            Error = error;
        }

        /// <summary>Initializes a new instance of the <see cref="JsonPatchOperationException"/> class.</summary>
        /// <param name="error">The error.</param>
        /// <param name="message">The message.</param>
        public JsonPatchOperationException(JsonPatchOperationError error, string message) : base(message)
        {
            Error = error;
        }

        /// <summary>Initializes a new instance of the <see cref="JsonPatchOperationException" /> class.</summary>
        /// <param name="error">The error.</param>
        /// <param name="message">The message.</param>
        /// <param name="innerException">The inner exception.</param>
        public JsonPatchOperationException(JsonPatchOperationError error, string message, Exception innerException) : base(message, innerException)
        {
            Error = error;
        }

        #endregion

        /// <summary>Gets the error.</summary>
        /// <value>The error.</value>
        public JsonPatchOperationError Error { get; private set; }
    }
}
