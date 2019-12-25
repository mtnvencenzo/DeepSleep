namespace DeepSleep.OpenApi.v3_0
{
    using Newtonsoft.Json;
    using System.Collections.Generic;

    /// <summary>
    /// 
    /// </summary>
    public class OpenApiPathItem3_0
    {
        /// <summary>Gets or sets the reference.</summary>
        /// <value>The reference.</value>
        [JsonProperty("$ref")]
        public string @ref { get; set; }

        /// <summary>Gets or sets the summary.</summary>
        /// <value>The summary.</value>
        [JsonProperty("summary")]
        public string summary { get; set; }

        /// <summary>Gets or sets the description.</summary>
        /// <value>The description.</value>
        [JsonProperty("description")]
        public string description { get; set; }

        /// <summary>Gets or sets the get.</summary>
        /// <value>The get.</value>
        public OpenApiOperation3_0 get { get; set; }

        /// <summary>Gets or sets the put.</summary>
        /// <value>The put.</value>
        public OpenApiOperation3_0 put { get; set; }

        /// <summary>Gets or sets the post.</summary>
        /// <value>The post.</value>
        public OpenApiOperation3_0 post { get; set; }

        /// <summary>Gets or sets the delete.</summary>
        /// <value>The delete.</value>
        public OpenApiOperation3_0 delete { get; set; }

        /// <summary>Gets or sets the options.</summary>
        /// <value>The options.</value>
        public OpenApiOperation3_0 options { get; set; }

        /// <summary>Gets or sets the head.</summary>
        /// <value>The head.</value>
        public OpenApiOperation3_0 head { get; set; }

        /// <summary>Gets or sets the patch.</summary>
        /// <value>The patch.</value>
        public OpenApiOperation3_0 patch { get; set; }

        /// <summary>Gets or sets the trace.</summary>
        /// <value>The trace.</value>
        public OpenApiOperation3_0 trace { get; set; }

        /// <summary>Gets or sets the servers.</summary>
        /// <value>The servers.</value>
        public List<OpenApiServer3_0> servers { get; set; }

        /// <summary>Gets or sets the parameters.</summary>
        /// <value>The parameters.</value>
        public List<OpenApiParameter3_0> parameters { get; set; }

        /// <summary>Gets or sets the extensions.</summary>
        /// <value>The extensions.</value>
        [JsonExtensionData]
        public Dictionary<string, object> extensions { get; set; }
    }
}
