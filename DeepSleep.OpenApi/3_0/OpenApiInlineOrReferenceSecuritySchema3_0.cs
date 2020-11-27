namespace DeepSleep.OpenApi.v3_0
{
    using System.Text.Json.Serialization;

    /// <summary>
    /// 
    /// </summary>
    public class OpenApiInlineOrReferenceSecuritySchema3_0
    {
        /// <summary>Initializes a new instance of the <see cref="OpenApiInlineOrReferenceSecuritySchema3_0"/> class.</summary>
        /// <param name="ref">The reference.</param>
        public OpenApiInlineOrReferenceSecuritySchema3_0(string @ref)
        {
            this.@ref = @ref;
        }

        /// <summary>Initializes a new instance of the <see cref="OpenApiInlineOrReferenceSecuritySchema3_0"/> class.</summary>
        /// <param name="securitySchema">The security schema.</param>
        public OpenApiInlineOrReferenceSecuritySchema3_0(OpenApiSecurityScheme3_0 securitySchema)
        {
            this.securitySchema = securitySchema;
        }

        /// <summary>Gets or sets the reference.</summary>
        /// <value>The reference.</value>
        [JsonPropertyName("$ref")]
        public string @ref { get; set; }

        /// <summary>Gets or sets the security schema.</summary>
        /// <value>The security schema.</value>
        public OpenApiSecurityScheme3_0 securitySchema { get; set; }
    }
}
