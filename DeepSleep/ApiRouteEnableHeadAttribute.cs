namespace DeepSleep
{
    using System;

    /// <summary>
    /// 
    /// </summary>
    /// <seealso cref="System.Attribute" />
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = false, Inherited = false)]
    public class ApiRouteEnableHeadAttribute : Attribute
    {
        /// <summary>Initializes a new instance of the <see cref="ApiRouteEnableHeadAttribute"/> class.</summary>
        /// <param name="enableHeadForGetRequests">The enable head for get requests.</param>
        public ApiRouteEnableHeadAttribute(bool enableHeadForGetRequests = true)
        {
            this.EnableHeadForGetRequests = enableHeadForGetRequests;
        }

        /// <summary>Gets the enable head for get requests.</summary>
        /// <value>The enable head for get requests.</value>
        public bool? EnableHeadForGetRequests { get; private set; }
    }
}
