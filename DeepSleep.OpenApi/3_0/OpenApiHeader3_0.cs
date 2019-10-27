namespace DeepSleep.OpenApi.v3_0
{
    using Newtonsoft.Json;
    using System.Collections.Generic;

    /// <summary>
    /// 
    /// </summary>
    public class OpenApiHeader3_0
    {
        /// <summary>Gets or sets the description.</summary>
        /// <value>The description.</value>
        public string description { get; set; }

        /// <summary>Gets or sets a value indicating whether this <see cref="OpenApiHeader3_0"/> is required.</summary>
        /// <value><c>true</c> if required; otherwise, <c>false</c>.</value>
        public bool required { get; set; }

        /// <summary>Gets or sets a value indicating whether this <see cref="OpenApiHeader3_0"/> is deprecated.</summary>
        /// <value><c>true</c> if deprecated; otherwise, <c>false</c>.</value>
        public bool deprecated { get; set; }

        /// <summary>Gets or sets a value indicating whether [allow empty value].</summary>
        /// <value><c>true</c> if [allow empty value]; otherwise, <c>false</c>.</value>
        public bool allowEmptyValue { get; set; }

        /// <summary>Gets or sets the style.</summary>
        /// <value>The style.</value>
        public OpenApiStyle3_0 style { get; set; }

        /// <summary>Gets or sets a value indicating whether this <see cref="OpenApiHeader3_0"/> is explode.</summary>
        /// <value><c>true</c> if explode; otherwise, <c>false</c>.</value>
        public bool explode { get; set; }

        /// <summary>Gets or sets a value indicating whether [allow reserved].</summary>
        /// <value><c>true</c> if [allow reserved]; otherwise, <c>false</c>.</value>
        public bool allowReserved { get; set; }

        /// <summary>Gets or sets the schema.</summary>
        /// <value>The schema.</value>
        public OpenApiSchema3_0 schema { get; set; }

        /// <summary>Gets or sets the example.</summary>
        /// <value>The example.</value>
        public object example { get; set; }

        /// <summary>Gets or sets the examples.</summary>
        /// <value>The examples.</value>
        public List<object> examples { get; set; } = new List<object>();

        /// <summary>Gets or sets the content.</summary>
        /// <value>The content.</value>
        public List<object> content { get; set; } = new List<object>();

        /// <summary>The extensions</summary>
        [JsonExtensionData]
        public Dictionary<string, object> extensions = new Dictionary<string, object>();
    }
}
