namespace DeepSleep.OpenApi.v3_0
{
    using Newtonsoft.Json;
    using System.Collections.Generic;

    /// <summary>
    /// 
    /// </summary>
    public class OpenApiSchema3_0
    {
        /// <summary>Gets or sets the reference.</summary>
        /// <value>The reference.</value>
        [JsonProperty("$ref")]
        public string @ref { get; set; }

        /// <summary>Gets or sets the title.</summary>
        /// <value>The title.</value>
        public string title { get; set; }

        /// <summary>Gets or sets the description.</summary>
        /// <value>The description.</value>
        public string description { get; set; }

        /// <summary>Gets or sets the multiple of.</summary>
        /// <value>The multiple of.</value>
        public int? multipleOf { get; set; }

        /// <summary>Gets or sets the maximum.</summary>
        /// <value>The maximum.</value>
        public uint? maximum { get; set; }

        /// <summary>Gets or sets the exclusive maximum.</summary>
        /// <value>The exclusive maximum.</value>
        public bool? exclusiveMaximum { get; set; }

        /// <summary>Gets or sets the minimum.</summary>
        /// <value>The minimum.</value>
        public uint? minimum { get; set; }

        /// <summary>Gets or sets the exclusive minimum.</summary>
        /// <value>The exclusive minimum.</value>
        public bool? exclusiveMinimum { get; set; }

        /// <summary>Gets or sets the maximum length.</summary>
        /// <value>The maximum length.</value>
        public uint? maxLength { get; set; }

        /// <summary>Gets or sets the minimum length.</summary>
        /// <value>The minimum length.</value>
        public uint? minLength { get; set; }

        /// <summary>Gets or sets the pattern.</summary>
        /// <value>The pattern.</value>
        public string pattern { get; set; }

        /// <summary>Gets or sets the maximum items.</summary>
        /// <value>The maximum items.</value>
        public uint? maxItems { get; set; }

        /// <summary>Gets or sets the minimum items.</summary>
        /// <value>The minimum items.</value>
        public uint? minItems { get; set; }

        /// <summary>Gets or sets the unique items.</summary>
        /// <value>The unique items.</value>
        public bool? uniqueItems { get; set; }

        /// <summary>Gets or sets the maximum properties.</summary>
        /// <value>The maximum properties.</value>
        public uint? maxProperties { get; set; }

        /// <summary>Gets or sets the minimum properties.</summary>
        /// <value>The minimum properties.</value>
        public uint? minProperties { get; set; }

        /// <summary>Gets or sets the required.</summary>
        /// <value>The required.</value>
        public List<string> required { get; set; }

        /// <summary>Gets or sets the enum.</summary>
        /// <value>The enum.</value>
        [JsonProperty("enum")]
        public List<object> @enum { get; set; }

        /// <summary>Gets or sets the type.</summary>
        /// <value>The type.</value>
        public string type { get; set; }

        /// <summary>Gets or sets the properties.</summary>
        /// <value>The properties.</value>
        public Dictionary<string, OpenApiSchema3_0> properties { get; set; }

        /// <summary>Gets or sets all of.</summary>
        /// <value>All of.</value>
        public OpenApiSchema3_0 allOf { get; set; }

        /// <summary>Gets or sets the one of.</summary>
        /// <value>The one of.</value>
        public OpenApiSchema3_0 oneOf { get; set; }

        /// <summary>Gets or sets any of.</summary>
        /// <value>Any of.</value>
        public OpenApiSchema3_0 anyOf { get; set; }

        /// <summary>Gets or sets the not.</summary>
        /// <value>The not.</value>
        public OpenApiSchema3_0 not { get; set; }

        /// <summary>Gets or sets the items.</summary>
        /// <value>The items.</value>
        public OpenApiSchema3_0 items { get; set; }

        /// <summary>Gets or sets the additional properties.</summary>
        /// <value>The additional properties.</value>
        public OpenApiSchema3_0 additionalProperties { get; set; }

        /// <summary>Gets or sets the format.</summary>
        /// <value>The format.</value>
        public string format { get; set; }

        /// <summary>Gets or sets the default.</summary>
        /// <value>The default.</value>
        public object @default { get; set; }

        /// <summary>Gets or sets the nullable.</summary>
        /// <value>The nullable.</value>
        public bool? nullable { get; set; }

        /// <summary>Gets or sets the discriminator.</summary>
        /// <value>The discriminator.</value>
        public OpenApiDiscriminator3_0 discriminator { get; set; }

        /// <summary>Gets or sets the read only.</summary>
        /// <value>The read only.</value>
        public bool? readOnly { get; set; }

        /// <summary>Gets or sets the writeonly.</summary>
        /// <value>The writeonly.</value>
        public bool? writeonly { get; set; }

        /// <summary>Gets or sets the XML.</summary>
        /// <value>The XML.</value>
        public object xml { get; set; }

        /// <summary>Gets or sets the external docs.</summary>
        /// <value>The external docs.</value>
        public OpenApiExternalDocumentation3_0 externalDocs { get; set; }

        /// <summary>Gets or sets the example.</summary>
        /// <value>The example.</value>
        public object example { get; set; }

        /// <summary>Gets or sets the deprecated.</summary>
        /// <value>The deprecated.</value>
        public bool? deprecated { get; set; }

        /// <summary>Gets or sets the extensions.</summary>
        /// <value>The extensions.</value>
        [JsonExtensionData]
        public Dictionary<string, object> extensions { get; set; }
    }
}
