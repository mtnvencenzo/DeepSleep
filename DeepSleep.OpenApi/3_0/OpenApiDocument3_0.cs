namespace DeepSleep.OpenApi.v3_0
{
    using Newtonsoft.Json;
    using System.Collections.Generic;

    /// <summary>
    /// 
    /// </summary>
    public class OpenApiDocument3_0
    {
        /// <summary>Gets the openapi.</summary>
        /// <value>The openapi.</value>
        public string openapi => "3.0.2";

        /// <summary>Gets or sets the information.</summary>
        /// <value>The information.</value>
        public OpenApiInfo3_0 info { get; set; }

        /// <summary>Gets or sets the servers.</summary>
        /// <value>The servers.</value>
        public List<OpenApiServer3_0> servers { get; set; } = new List<OpenApiServer3_0>();

        /// <summary>The paths</summary>
        public Dictionary<string, OpenApiPathItem3_0> paths = new Dictionary<string, OpenApiPathItem3_0>();

        /// <summary>Gets or sets the components.</summary>
        /// <value>The components.</value>
        public OpenApiComponents3_0 components { get; set; }

        /// <summary>Gets or sets the security.</summary>
        /// <value>The security.</value>
        public List<OpenApiSecurityRequirementContainer3_0> security { get; set; } = new List<OpenApiSecurityRequirementContainer3_0>();

        /// <summary>Gets or sets the external docs.</summary>
        /// <value>The external docs.</value>
        public OpenApiExternalDocumentation3_0 externalDocs { get; set; }

        /// <summary>Gets or sets the tags.</summary>
        /// <value>The tags.</value>
        public List<OpenApiTag3_0> tags { get; set; } = new List<OpenApiTag3_0>();

        /// <summary>Gets or sets the extensions.</summary>
        /// <value>The extensions.</value>
        [JsonExtensionData]
        public Dictionary<string, object> extensions { get; set; } = new Dictionary<string, object>();
    }
}
