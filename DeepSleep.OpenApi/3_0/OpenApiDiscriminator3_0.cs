namespace DeepSleep.OpenApi.v3_0
{
    using System.Collections.Generic;

    /// <summary>
    /// 
    /// </summary>
    public class OpenApiDiscriminator3_0
    {
        /// <summary>Gets or sets the name of the property.</summary>
        /// <value>The name of the property.</value>
        public string propertyName { get; set; }

        /// <summary>Gets or sets the mapping.</summary>
        /// <value>The mapping.</value>
        public Dictionary<string, string> mapping { get; set; } = new Dictionary<string, string>();
    }
}
