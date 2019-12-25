namespace DeepSleep.OpenApi.v3_0
{
    using Newtonsoft.Json;
    using System.Collections.Generic;

    /// <summary>
    /// 
    /// </summary>
    public class OpenApiParameter3_0
    {
        /// <summary>Gets or sets the reference.</summary>
        /// <value>The reference.</value>
        [JsonProperty("$ref")]
        public string @ref { get; set; }

        /// <summary>Gets or sets the name.</summary>
        /// <value>The name.</value>
        [JsonProperty("name")]
        public string name { get; set; }

        /// <summary>Gets or sets the in.</summary>
        /// <value>The in.</value>
        [JsonProperty("in")]
        public string @in { get; set; }

        /// <summary>Gets or sets the description.</summary>
        /// <value>The description.</value>
        [JsonProperty("description")]
        public string description { get; set; }

        /// <summary>Gets or sets the required.</summary>
        /// <value>The required.</value>
        [JsonProperty("required")]
        public bool? required { get; set; }

        /// <summary>Gets or sets the deprecated.</summary>
        /// <value>The deprecated.</value>
        [JsonProperty("deprecated")]
        public bool? deprecated { get; set; }

        /// <summary>Gets or sets the allow empty value.</summary>
        /// <value>The allow empty value.</value>
        [JsonProperty("allowEmptyValue")]
        public bool? allowEmptyValue { get; set; }

        /// <summary>Gets or sets the style.</summary>
        /// <value>The style.</value>
        public string style { get; set; }

        /// <summary>Gets or sets the explode.</summary>
        /// <value>The explode.</value>
        [JsonProperty("explode")]
        public bool? explode { get; set; }

        /// <summary>Gets or sets the allow reserved.</summary>
        /// <value>The allow reserved.</value>
        [JsonProperty("allowReserved")]
        public bool? allowReserved { get; set; }

        /// <summary>Gets or sets the schema.</summary>
        /// <value>The schema.</value>
        public OpenApiSchema3_0 schema { get; set; }

        /// <summary>Gets or sets the example.</summary>
        /// <value>The example.</value>
        [JsonProperty("example")]
        public object example { get; set; }

        /// <summary>Gets or sets the examples.</summary>
        /// <value>The examples.</value>
        public List<object> examples { get; set; }

        /// <summary>Gets or sets the content.</summary>
        /// <value>The content.</value>
        public Dictionary<string, OpenApiMediaType3_0> content { get; set; }

        /// <summary>Gets or sets the extensions.</summary>
        /// <value>The extensions.</value>
        [JsonExtensionData]
        public Dictionary<string, object> extensions { get; set; }
    }
}
