namespace DeepSleep.OpenApi.v3_0
{
    using System.Collections.Generic;
    using System.Text.Json.Serialization;

    /// <summary>
    /// 
    /// </summary>
    public class OpenApiTag3_0
    {
        /// <summary>Gets or sets the name.</summary>
        /// <value>The name.</value>
        public string name { get; set; }

        /// <summary>Gets or sets the description.</summary>
        /// <value>The description.</value>
        public string description { get; set; }

        /// <summary>Gets or sets the external docs.</summary>
        /// <value>The external docs.</value>
        public OpenApiExternalDocumentation3_0 externalDocs { get; set; }

        /// <summary>The extensions</summary>
        [JsonExtensionData]
        public Dictionary<string, object> extensions = new Dictionary<string, object>();
    }
}
