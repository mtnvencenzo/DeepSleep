namespace DeepSleep.OpenApi.Decorators
{
    using System;

    /// <summary>
    /// 
    /// </summary>
    /// <seealso cref="System.Attribute" />
    [AttributeUsage(validOn: AttributeTargets.Method | AttributeTargets.Class | AttributeTargets.Enum | AttributeTargets.Parameter | AttributeTargets.Property | AttributeTargets.ReturnValue, AllowMultiple = true)]
    public class OasDescriptionAttribute : Attribute
    {
        /// <summary>Initializes a new instance of the <see cref="OasDescriptionAttribute"/> class.</summary>
        /// <param name="description">The description.</param>
        public OasDescriptionAttribute(string description)
        {
            this.Description = description ?? string.Empty;
        }

        /// <summary>Gets the description.</summary>
        /// <value>The description.</value>
        public string Description { get; }
    }
}
