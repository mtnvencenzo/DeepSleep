namespace DeepSleep.OpenApi.v3_0
{
    using System.Text.Json.Serialization;

    /// <summary>
    /// 
    /// </summary>
    public class OpenApiInlineOrReferenceHeader3_0
    {
        /// <summary>Initializes a new instance of the <see cref="OpenApiInlineOrReferenceHeader3_0"/> class.</summary>
        /// <param name="ref">The reference.</param>
        public OpenApiInlineOrReferenceHeader3_0(string @ref)
        {
            this.@ref = @ref;
        }

        /// <summary>Initializes a new instance of the <see cref="OpenApiInlineOrReferenceHeader3_0"/> class.</summary>
        /// <param name="header">The header.</param>
        public OpenApiInlineOrReferenceHeader3_0(OpenApiHeader3_0 header)
        {
            this.header = header;
        }

        /// <summary>Gets or sets the reference.</summary>
        /// <value>The reference.</value>
        [JsonPropertyName("$ref")]
        public string @ref { get; set; }

        /// <summary>Gets the header.</summary>
        /// <value>The header.</value>
        public OpenApiHeader3_0 header { get; private set; }
    }
}
