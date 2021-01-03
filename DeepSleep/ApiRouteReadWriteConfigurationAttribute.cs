namespace DeepSleep
{
    using System;

    /// <summary>
    /// 
    /// </summary>
    /// <seealso cref="System.Attribute" />
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = false, Inherited = false)]
    public class ApiRouteReadWriteConfigurationAttribute : Attribute
    {
        /// <summary>Initializes a new instance of the <see cref="ApiRouteReadWriteConfigurationAttribute"/> class.</summary>
        /// <param name="acceptHeaderOverride">The accept header override.</param>
        /// <param name="readableMediaTypes">The readable media types.</param>
        /// <param name="writeableMediaTypes">The writeable media types.</param>
        public ApiRouteReadWriteConfigurationAttribute(string acceptHeaderOverride = null, string[] readableMediaTypes = null, string[] writeableMediaTypes = null)
        {
            this.ReadableMediaTypes = readableMediaTypes;
            this.WriteableMediaTypes = writeableMediaTypes;
            this.AcceptHeaderOverride = acceptHeaderOverride;
        }

        /// <summary>Gets the readable media types.</summary>
        /// <value>The readable media types.</value>
        public string[] ReadableMediaTypes { get; private set; }

        /// <summary>Gets the writeable media types.</summary>
        /// <value>The writeable media types.</value>
        public string[] WriteableMediaTypes { get; private set; }

        /// <summary>Gets the accept header override.</summary>
        /// <value>The accept header override.</value>
        public string AcceptHeaderOverride { get; private set; }
    }
}
