namespace DeepSleep.OpenApi.Decorators
{
    using System;

    /// <summary>
    /// 
    /// </summary>
    /// <seealso cref="System.Attribute" />
    [AttributeUsage(validOn: AttributeTargets.Method, AllowMultiple = true)]
    public class OpenApiResponseAttribute : Attribute
    {
        /// <summary>Initializes a new instance of the <see cref="OpenApiResponseAttribute"/> class.</summary>
        /// <param name="responseType">Type of the response.</param>
        public OpenApiResponseAttribute(Type responseType)
        {
            this.StatusCode = "200";
            this.ResponseType = responseType;
        }

        /// <summary>Initializes a new instance of the <see cref="OpenApiResponseAttribute"/> class.</summary>
        /// <param name="statusCode">The status code.</param>
        /// <param name="responseType">Type of the response.</param>
        public OpenApiResponseAttribute(string statusCode, Type responseType)
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
