namespace DeepSleep
{
    using System;

    /// <summary>
    /// 
    /// </summary>
    /// <seealso cref="System.Attribute" />
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = false, Inherited = false)]
    public class ApiRouteIncludeRequestIdHeaderAttribute : Attribute
    {
        /// <summary>Initializes a new instance of the <see cref="ApiRouteIncludeRequestIdHeaderAttribute"/> class.</summary>
        /// <param name="includeRequestIdHeaderInResponse">if set to <c>true</c> [include request identifier header in response].</param>
        public ApiRouteIncludeRequestIdHeaderAttribute(bool includeRequestIdHeaderInResponse = true)
        {
            this.IncludeRequestIdHeaderInResponse = includeRequestIdHeaderInResponse;
        }

        /// <summary>Gets the include request identifier header in response.</summary>
        /// <value>The include request identifier header in response.</value>
        public bool? IncludeRequestIdHeaderInResponse { get; private set; }
    }
}
