namespace DeepSleep.OpenApi.v3_0
{
    using Newtonsoft.Json;

    /// <summary>
    /// 
    /// </summary>
    public class OpenApiInlineOrReferenceLink3_0
    {
        /// <summary>Initializes a new instance of the <see cref="OpenApiInlineOrReferenceLink3_0"/> class.</summary>
        /// <param name="ref">The reference.</param>
        public OpenApiInlineOrReferenceLink3_0(string @ref)
        {
            this.@ref = @ref;
        }

        /// <summary>Initializes a new instance of the <see cref="OpenApiInlineOrReferenceLink3_0"/> class.</summary>
        /// <param name="link">The link.</param>
        public OpenApiInlineOrReferenceLink3_0(OpenApiLink3_0 link)
        {
            this.link = link;
        }

        /// <summary>Gets or sets the reference.</summary>
        /// <value>The reference.</value>
        [JsonProperty("$ref")]
        public string @ref { get; set; }

        /// <summary>Gets or sets the link.</summary>
        /// <value>The link.</value>
        public OpenApiLink3_0 link { get; set; }
    }
}
