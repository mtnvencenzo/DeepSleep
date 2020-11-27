namespace DeepSleep.OpenApi.v3_0
{
    using System.Collections.Generic;
    using System.Text.Json.Serialization;

    /// <summary>
    /// 
    /// </summary>
    public class OpenApiRequestBody3_0
    {
        /// <summary>Gets or sets the description.</summary>
        /// <value>The description.</value>
        public string description { get; set; }

        /// <summary>Gets or sets the content.</summary>
        /// <value>The content.</value>
        public Dictionary<string, OpenApiMediaType3_0> content { get; set; } = new Dictionary<string, OpenApiMediaType3_0>();

        /// <summary>Gets or sets a value indicating whether this <see cref="OpenApiRequestBody3_0" /> is required.</summary>
        /// <value><c>true</c> if required; otherwise, <c>false</c>.</value>
        public bool required { get; set; }

        /// <summary>The extensions</summary>
        [JsonExtensionData]
        public Dictionary<string, object> extensions = new Dictionary<string, object>();
    }
}
