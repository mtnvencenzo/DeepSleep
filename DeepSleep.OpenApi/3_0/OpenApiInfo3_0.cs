namespace DeepSleep.OpenApi.v3_0
{
    using System.Collections.Generic;
    using System.Text.Json.Serialization;

    /// <summary>
    /// 
    /// </summary>
    public class OpenApiInfo3_0
    {
        /// <summary>Gets or sets the title.</summary>
        /// <value>The title.</value>
        public string title { get; set; }

        /// <summary>Gets or sets the description.</summary>
        /// <value>The description.</value>
        public string description { get; set; }

        /// <summary>Gets or sets the terms of service.</summary>
        /// <value>The terms of service.</value>
        public string termsOfService { get; set; }

        /// <summary>Gets or sets the version.</summary>
        /// <value>The version.</value>
        public string version { get; set; }

        /// <summary>Gets or sets the contact.</summary>
        /// <value>The contact.</value>
        public OpenApiContact3_0 contact { get; set; }

        /// <summary>Gets or sets the license.</summary>
        /// <value>The license.</value>
        public OpenApiLicense3_0 license { get; set; }

        /// <summary>The extensions</summary>
        [JsonExtensionData]
        public Dictionary<string, object> extensions = new Dictionary<string, object>();
    }
}
