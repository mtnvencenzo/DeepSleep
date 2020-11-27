namespace DeepSleep.OpenApi.v3_0
{
    using System.Text.Json.Serialization;

    /// <summary>
    /// 
    /// </summary>
    public class OpenApiInlineOrReferenceCallback3_0
    {
        /// <summary>Initializes a new instance of the <see cref="OpenApiInlineOrReferenceCallback3_0"/> class.</summary>
        /// <param name="ref">The reference.</param>
        public OpenApiInlineOrReferenceCallback3_0(string @ref)
        {
            this.@ref = @ref;
        }

        /// <summary>Initializes a new instance of the <see cref="OpenApiInlineOrReferenceCallback3_0"/> class.</summary>
        /// <param name="callback">The callback.</param>
        public OpenApiInlineOrReferenceCallback3_0(OpenApiCallback3_0 callback)
        {
            this.callback = callback;
        }

        /// <summary>Gets or sets the reference.</summary>
        /// <value>The reference.</value>
        [JsonPropertyName("$ref")]
        public string @ref { get; set; }

        /// <summary>Gets or sets the callback.</summary>
        /// <value>The callback.</value>
        public OpenApiCallback3_0 callback { get; set; }
    }
}
