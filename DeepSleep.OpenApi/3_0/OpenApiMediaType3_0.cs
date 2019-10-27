namespace DeepSleep.OpenApi.v3_0
{
    using Newtonsoft.Json;
    using System.Collections.Generic;

    /// <summary>
    /// 
    /// </summary>
    public class OpenApiMediaType3_0
    {
        /// <summary>Gets or sets the schema.</summary>
        /// <value>The schema.</value>
        [JsonProperty("schema")]
        public OpenApiSchema3_0 schema { get; set; }

        /// <summary>Gets or sets the example.</summary>
        /// <value>The example.</value>
        [JsonProperty("example")]
        public object example { get; set; }

        /// <summary>Gets or sets the examples.</summary>
        /// <value>The examples.</value>
        public List<object> examples { get; set; }

        /// <summary>Gets or sets the encoding.</summary>
        /// <value>The encoding.</value>
        public Dictionary<string, OpenApiEncoding3_0> encoding { get; set; }
    }
}
