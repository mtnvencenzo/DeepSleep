namespace DeepSleep.OpenApi.v3_0
{
    using System.Collections.Generic;
    using System.Text.Json.Serialization;

    /// <summary>
    /// 
    /// </summary>
    public class OpenApiServerVariable3_0
    {
        /// <summary>Gets or sets the enum.</summary>
        /// <value>The enum.</value>
        [JsonPropertyName("enum")]
        public List<string> @enum { get; set; } = new List<string>();

        /// <summary>Gets or sets the default.</summary>
        /// <value>The default.</value>
        [JsonPropertyName("default")]
        public string @default { get; set; }

        /// <summary>Gets or sets the description.</summary>
        /// <value>The description.</value>
        public string description { get; set; }

        /// <summary>Gets or sets the extensions.</summary>
        /// <value>The extensions.</value>
        [JsonExtensionData]
        public Dictionary<string, object> extensions { get; set; }
    }
}
