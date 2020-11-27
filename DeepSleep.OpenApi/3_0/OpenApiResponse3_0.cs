namespace DeepSleep.OpenApi.v3_0
{
    using System.Collections.Generic;
    using System.Text.Json.Serialization;

    /// <summary>
    /// 
    /// </summary>
    public class OpenApiResponse3_0
    {
        /// <summary>Gets or sets the reference.</summary>
        /// <value>The reference.</value>
        [JsonPropertyName("$ref")]
        public string @ref { get; set; }

        /// <summary>Gets or sets the description.</summary>
        /// <value>The description.</value>
        [JsonPropertyName("description")]
        public string description { get; set; }

        /// <summary>Gets or sets the headers.</summary>
        /// <value>The headers.</value>
        public Dictionary<string, OpenApiInlineOrReferenceHeader3_0> headers { get; set; }

        /// <summary>Gets or sets the content.</summary>
        /// <value>The content.</value>
        public Dictionary<string, OpenApiMediaType3_0> content { get; set; }

        /// <summary>Gets or sets the links.</summary>
        /// <value>The links.</value>
        public Dictionary<string, OpenApiInlineOrReferenceLink3_0> links { get; set; }

        /// <summary>Gets or sets the extensions.</summary>
        /// <value>The extensions.</value>
        [JsonExtensionData]
        public Dictionary<string, object> extensions { get; set; }
    }
}
