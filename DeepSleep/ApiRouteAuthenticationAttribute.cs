namespace DeepSleep
{
    using System;

    /// <summary>
    /// 
    /// </summary>
    /// <seealso cref="System.Attribute" />
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = false, Inherited = false)]
    public class ApiRouteAuthenticationAttribute : Attribute
    {
        /// <summary>Initializes a new instance of the <see cref="ApiRouteAuthenticationAttribute"/> class.</summary>
        /// <param name="allowAnonymous">if set to <c>true</c> [allow anonymous].</param>
        public ApiRouteAuthenticationAttribute(bool allowAnonymous)
        {
            this.AllowAnonymous = allowAnonymous;
        }

        /// <summary>Initializes a new instance of the <see cref="ApiRouteAuthenticationAttribute"/> class.</summary>
        /// <param name="supportedAuthenticationSchemes">The supported authentication schemes.</param>
        public ApiRouteAuthenticationAttribute(string[] supportedAuthenticationSchemes)
        {
            this.AllowAnonymous = false;
            this.SupportedAuthenticationSchemes = supportedAuthenticationSchemes;
        }

        /// <summary>Gets a value indicating whether [allow anonymous].</summary>
        /// <value><c>true</c> if [allow anonymous]; otherwise, <c>false</c>.</value>
        public bool? AllowAnonymous { get; private set; }

        /// <summary>Gets the supported authentication schemes.</summary>
        /// <value>The supported authentication schemes.</value>
        public string[] SupportedAuthenticationSchemes { get; private set; }
    }
}
