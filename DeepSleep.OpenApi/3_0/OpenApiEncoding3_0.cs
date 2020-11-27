namespace DeepSleep.OpenApi.v3_0
{
    using System.Collections.Generic;
    using System.Text.Json.Serialization;

    /// <summary>
    /// 
    /// </summary>
    public class OpenApiEncoding3_0
    {
        /// <summary>Gets or sets the type of the content.</summary>
        /// <value>The type of the content.</value>
        public string contentType { get; set; }

        /// <summary>Gets or sets the headers.</summary>
        /// <value>The headers.</value>
        public Dictionary<string, OpenApiInlineOrReferenceHeader3_0> headers { get; set; } = new Dictionary<string, OpenApiInlineOrReferenceHeader3_0>();

        /// <summary>Gets or sets the style.</summary>
        /// <value>The style.</value>
        public OpenApiStyle3_0 style { get; set; }

        /// <summary>Gets or sets a value indicating whether this <see cref="OpenApiEncoding3_0"/> is explode.</summary>
        /// <value><c>true</c> if explode; otherwise, <c>false</c>.</value>
        public bool explode { get; set; }

        /// <summary>Gets or sets a value indicating whether [allow reserved].</summary>
        /// <value><c>true</c> if [allow reserved]; otherwise, <c>false</c>.</value>
        public bool allowReserved { get; set; }

        /// <summary>The extensions</summary>
        [JsonExtensionData]
        public Dictionary<string, object> extensions = new Dictionary<string, object>();
    }
}
