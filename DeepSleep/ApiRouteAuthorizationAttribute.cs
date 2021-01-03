namespace DeepSleep
{
    using System;

    /// <summary>
    /// 
    /// </summary>
    /// <seealso cref="System.Attribute" />
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = false, Inherited = false)]
    public class ApiRouteAuthorizationAttribute : Attribute
    {
        /// <summary>Initializes a new instance of the <see cref="ApiRouteAuthorizationAttribute"/> class.</summary>
        /// <param name="policy">The policy.</param>
        public ApiRouteAuthorizationAttribute(string policy)
        {
            this.Policy = policy;
        }

        /// <summary>Gets the policy.</summary>
        /// <value>The policy.</value>
        public string Policy { get; private set; }
    }
}
