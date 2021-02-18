namespace DeepSleep.OpenApi.Decorators
{
    using System;

    /// <summary>
    /// 
    /// </summary>
    /// <seealso cref="System.Attribute" />
    [AttributeUsage(validOn: AttributeTargets.Method | AttributeTargets.Class | AttributeTargets.Enum | AttributeTargets.Parameter | AttributeTargets.Property | AttributeTargets.ReturnValue, AllowMultiple = false)]
    public class OasSummaryAttribute : Attribute
    {
        /// <summary>Initializes a new instance of the <see cref="OasSummaryAttribute"/> class.</summary>
        /// <param name="summary">The summary.</param>
        public OasSummaryAttribute(string summary)
        {
            this.Summary = summary ?? string.Empty;
        }

        /// <summary>Gets the summary.</summary>
        /// <value>The summary.</value>
        public string Summary { get; }
    }
}
