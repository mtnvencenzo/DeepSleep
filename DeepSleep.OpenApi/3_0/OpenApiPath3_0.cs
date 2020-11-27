namespace DeepSleep.OpenApi.v3_0
{
    using System.Collections.Generic;
    using System.Text.Json.Serialization;

    /// <summary>
    /// 
    /// </summary>
    public class OpenApiPath3_0
    {
        /// <summary>Gets or sets the path.</summary>
        /// <value>The path.</value>
        public string Path { get; set; }

        /// <summary>Gets or sets the path object.</summary>
        /// <value>The path object.</value>
        public OpenApiPathItem3_0 PathObject { get; set; }

        /// <summary>The extensions</summary>
        [JsonExtensionData]
        public Dictionary<string, object> extensions = new Dictionary<string, object>();
    }
}
