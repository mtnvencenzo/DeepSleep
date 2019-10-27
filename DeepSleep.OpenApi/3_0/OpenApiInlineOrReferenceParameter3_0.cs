namespace DeepSleep.OpenApi.v3_0
{
    using Newtonsoft.Json;

    /// <summary>
    /// 
    /// </summary>
    public class OpenApiInlineOrReferenceParameter3_0
    {
        /// <summary>Initializes a new instance of the <see cref="OpenApiInlineOrReferenceParameter3_0"/> class.</summary>
        /// <param name="ref">The reference.</param>
        public OpenApiInlineOrReferenceParameter3_0(string @ref)
        {
            this.@ref = @ref;
        }

        /// <summary>Initializes a new instance of the <see cref="OpenApiInlineOrReferenceParameter3_0"/> class.</summary>
        /// <param name="parameter">The parameter.</param>
        public OpenApiInlineOrReferenceParameter3_0(OpenApiParameter3_0 parameter)
        {
            this.parameter = parameter;
        }

        /// <summary>Gets or sets the reference.</summary>
        /// <value>The reference.</value>
        [JsonProperty("$ref")]
        public string @ref { get; set; }

        /// <summary>Gets or sets the parameter.</summary>
        /// <value>The parameter.</value>
        public OpenApiParameter3_0 parameter { get; set; }
    }
}
