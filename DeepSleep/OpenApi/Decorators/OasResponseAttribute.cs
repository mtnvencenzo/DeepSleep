namespace DeepSleep.OpenApi.Decorators
{
    using System;

    /// <summary>
    /// 
    /// </summary>
    /// <seealso cref="System.Attribute" />
    [AttributeUsage(validOn: AttributeTargets.Method, AllowMultiple = true)]
    public class OasResponseAttribute : Attribute
    {
        /// <summary>Initializes a new instance of the <see cref="OasResponseAttribute"/> class.</summary>
        /// <param name="responseType">Type of the response.</param>
        public OasResponseAttribute(Type responseType)
        {
            this.StatusCode = "200";
            this.ResponseType = responseType;
        }

        /// <summary>Initializes a new instance of the <see cref="OasResponseAttribute"/> class.</summary>
        /// <param name="statusCode">The status code.</param>
        /// <param name="responseType">Type of the response.</param>
        public OasResponseAttribute(string statusCode, Type responseType)
        {
            this.StatusCode = statusCode;
            this.ResponseType = responseType;
        }

        /// <summary>Gets the status code.</summary>
        /// <value>The status code.</value>
        public string StatusCode { get; }

        /// <summary>Gets the type of the response.</summary>
        /// <value>The type of the response.</value>
        public Type ResponseType { get; }
    }
}
