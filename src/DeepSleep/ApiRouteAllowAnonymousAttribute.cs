namespace DeepSleep
{
    using System;

    /// <summary>
    /// 
    /// </summary>
    /// <seealso cref="System.Attribute" />
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = false, Inherited = false)]
    public class ApiRouteAllowAnonymousAttribute : Attribute
    {
        /// <summary>Initializes a new instance of the <see cref="ApiRouteAllowAnonymousAttribute"/> class.</summary>
        public ApiRouteAllowAnonymousAttribute()
        {
            this.AllowAnonymous = true;
        }

        /// <summary>Initializes a new instance of the <see cref="ApiRouteAllowAnonymousAttribute"/> class.</summary>
        /// <param name="allowAnonymous">if set to <c>true</c> [allow anonymous].</param>
        public ApiRouteAllowAnonymousAttribute(bool allowAnonymous)
        {
            this.AllowAnonymous = allowAnonymous;
        }

        /// <summary>Gets a value indicating whether [allow anonymous].</summary>
        /// <value><c>true</c> if [allow anonymous]; otherwise, <c>false</c>.</value>
        public bool? AllowAnonymous { get; private set; }
    }
}
