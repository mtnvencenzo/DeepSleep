namespace DeepSleep.OpenApi.v3_0
{
    using Newtonsoft.Json;
    using System.Collections.Generic;

    /// <summary>
    /// 
    /// </summary>
    public class OpenApiSecurityScheme3_0
    {
        /// <summary>Gets or sets the reference.</summary>
        /// <value>The reference.</value>
        [JsonProperty("$ref")]
        public string @ref { get; set; }

        /// <summary>Gets or sets the type.</summary>
        /// <value>The type.</value>
        public OpenApiSecuritySchemeType3_0 type { get; set; }

        /// <summary>Gets or sets the description.</summary>
        /// <value>The description.</value>
        public string description { get; set; }

        /// <summary>Gets or sets the name.</summary>
        /// <value>The name.</value>
        public string name { get; set; }

        /// <summary>Gets or sets the in.</summary>
        /// <value>The in.</value>
        public OpenApiSecuritySchemeIn3_0 @in { get; set; }

        /// <summary>Gets or sets the scheme.</summary>
        /// <value>The scheme.</value>
        public string scheme { get; set; }

        /// <summary>Gets or sets the bearer format.</summary>
        /// <value>The bearer format.</value>
        public string bearerFormat { get; set; }

        /// <summary>Gets or sets the flows.</summary>
        /// <value>The flows.</value>
        public object flows { get; set; }

        /// <summary>Gets or sets the open identifier connect URL.</summary>
        /// <value>The open identifier connect URL.</value>
        public string openIdConnectUrl { get; set; }

        /// <summary>Gets or sets the extensions.</summary>
        /// <value>The extensions.</value>
        [JsonExtensionData]
        public Dictionary<string, object> extensions { get; set; }
    }
}
