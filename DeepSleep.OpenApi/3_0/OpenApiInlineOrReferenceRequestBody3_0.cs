namespace DeepSleep.OpenApi.v3_0
{
    using Newtonsoft.Json;

    /// <summary>
    /// 
    /// </summary>
    public class OpenApiInlineOrReferenceRequestBody3_0
    {
        /// <summary>Initializes a new instance of the <see cref="OpenApiInlineOrReferenceRequestBody3_0"/> class.</summary>
        /// <param name="ref">The reference.</param>
        public OpenApiInlineOrReferenceRequestBody3_0(string @ref)
        {
            this.@ref = @ref;
        }

        /// <summary>Initializes a new instance of the <see cref="OpenApiInlineOrReferenceRequestBody3_0"/> class.</summary>
        /// <param name="requestBody">The request body.</param>
        public OpenApiInlineOrReferenceRequestBody3_0(OpenApiRequestBody3_0 requestBody)
        {
            this.requestBody = requestBody;
        }

        /// <summary>Gets or sets the reference.</summary>
        /// <value>The reference.</value>
        [JsonProperty("$ref")]
        public string @ref { get; set; }

        /// <summary>Gets or sets the request body.</summary>
        /// <value>The request body.</value>
        public OpenApiRequestBody3_0 requestBody { get; set; }
    }
}
