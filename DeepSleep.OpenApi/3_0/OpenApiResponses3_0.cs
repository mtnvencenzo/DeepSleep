namespace DeepSleep.OpenApi.v3_0
{
    using System.Collections.Generic;
    using System.Text.Json.Serialization;

    /// <summary>
    /// 
    /// </summary>
    public class OpenApiResponses3_0
    {
        /// <summary>Gets or sets the responses.</summary>
        /// <value>The responses.</value>
        [JsonExtensionData] // OpenApiResponse3_0
        public Dictionary<string, object> responses { get; set; } = new Dictionary<string, object>();

        /// <summary>The extensions</summary>
        [JsonExtensionData]
        public Dictionary<string, object> extensions { get; set; }
    }
}
