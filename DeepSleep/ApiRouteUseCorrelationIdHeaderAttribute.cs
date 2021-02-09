namespace DeepSleep
{
    using System;

    /// <summary>
    /// 
    /// </summary>
    /// <seealso cref="System.Attribute" />
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = false, Inherited = false)]
    public class ApiRouteUseCorrelationIdHeaderAttribute : Attribute
    {
        /// <summary>Initializes a new instance of the <see cref="ApiRouteUseCorrelationIdHeaderAttribute"/> class.</summary>
        public ApiRouteUseCorrelationIdHeaderAttribute()
        {
            this.UseCorrelationIdHeader = true;
        }

        /// <summary>Initializes a new instance of the <see cref="ApiRouteUseCorrelationIdHeaderAttribute"/> class.</summary>
        /// <param name="useCorrelationIdHeader">if set to <c>true</c> [use correlation identifier header].</param>
        public ApiRouteUseCorrelationIdHeaderAttribute(bool useCorrelationIdHeader)
        {
            this.UseCorrelationIdHeader = useCorrelationIdHeader;
        }

        /// <summary>Gets the use correlation identifier header.</summary>
        /// <value>The use correlation identifier header.</value>
        public bool? UseCorrelationIdHeader { get; private set; }
    }
}
