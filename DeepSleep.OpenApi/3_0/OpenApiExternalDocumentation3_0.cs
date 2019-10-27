namespace DeepSleep.OpenApi.v3_0
{
    using Newtonsoft.Json;
    using System.Collections.Generic;

    /// <summary>
    /// 
    /// </summary>
    public class OpenApiExternalDocumentation3_0
    {
        /// <summary>Gets or sets the description.</summary>
        /// <value>The description.</value>
        public string description { get; set; }

        /// <summary>Gets or sets the URL.</summary>
        /// <value>The URL.</value>
        public string url { get; set; }

        /// <summary>The extensions</summary>
        [JsonExtensionData]
        public Dictionary<string, object> extensions = new Dictionary<string, object>();
    }
}
