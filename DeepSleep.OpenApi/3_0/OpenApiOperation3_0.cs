namespace DeepSleep.OpenApi.v3_0
{
    using Newtonsoft.Json;
    using System.Collections.Generic;

    /// <summary>
    /// 
    /// </summary>
    public class OpenApiOperation3_0
    {
        /// <summary>Gets or sets the tags.</summary>
        /// <value>The tags.</value>
        public List<string> tags { get; set; }

        /// <summary>Gets or sets the summary.</summary>
        /// <value>The summary.</value>
        public string summary { get; set; }

        /// <summary>Gets or sets the description.</summary>
        /// <value>The description.</value>
        public string description { get; set; }

        /// <summary>Gets or sets the external docs.</summary>
        /// <value>The external docs.</value>
        public OpenApiExternalDocumentation3_0 externalDocs { get; set; }

        /// <summary>Gets or sets the operation identifier.</summary>
        /// <value>The operation identifier.</value>
        public string operationId { get; set; }

        /// <summary>Gets or sets the parameters.</summary>
        /// <value>The parameters.</value>
        public List<OpenApiInlineOrReferenceParameter3_0> parameters { get; set; }

        /// <summary>Gets or sets the request body.</summary>
        /// <value>The request body.</value>
        public OpenApiInlineOrReferenceRequestBody3_0 requestBody { get; set; }

        /// <summary>Gets or sets the responses.</summary>
        /// <value>The responses.</value>
        public Dictionary<string, OpenApiResponse3_0> responses { get; set; }

        /// <summary>Gets or sets the callbacks.</summary>
        /// <value>The callbacks.</value>
        public Dictionary<string, OpenApiInlineOrReferenceCallback3_0> callbacks { get; set; }

        /// <summary>Gets or sets the deprecated.</summary>
        /// <value>The deprecated.</value>
        public bool? deprecated { get; set; }

        /// <summary>Gets or sets the security.</summary>
        /// <value>The security.</value>
        public List<OpenApiSecurityRequirementContainer3_0> security { get; set; }

        /// <summary>Gets or sets the servers.</summary>
        /// <value>The servers.</value>
        public List<OpenApiServer3_0> servers { get; set; }

        /// <summary>Gets or sets the extensions.</summary>
        /// <value>The extensions.</value>
        [JsonExtensionData]
        public Dictionary<string, object> extensions { get; set; }
    }
}
