namespace DeepSleep.OpenApi
{
    using Microsoft.OpenApi.Models;
    using System.Collections.Generic;
    using System.Text.Json;

    /// <summary>
    /// 
    /// </summary>
    /// <seealso cref="DeepSleep.OpenApi.IDeepSleepOasConfigurationProvider" />
    public class DeepSleepOasConfigurationProvider : IDeepSleepOasConfigurationProvider
    {
        /// <summary>Gets or sets the information.</summary>
        /// <value>The information.</value>
        public OpenApiInfo Info { get; set; }

        /// <summary>Gets or sets a value indicating whether [prefix names with namespace].</summary>
        /// <value><c>true</c> if [prefix names with namespace]; otherwise, <c>false</c>.</value>
        public bool PrefixNamesWithNamespace { get; set; }

        /// <summary>Gets or sets the v2 route template.</summary>
        /// <value>The v2 route template.</value>
        public string V2RouteTemplate { get; set; }

        /// <summary>Gets or sets the v3 route template.</summary>
        /// <value>The v3 route template.</value>
        public string V3RouteTemplate { get; set; }

        /// <summary>Gets or sets the XML documentation file names.</summary>
        /// <value>The XML documentation file names.</value>
        public IList<string> XmlDocumentationFileNames { get; set; }

        /// <summary>Gets or sets the enum modeling.</summary>
        /// <value>The enum modeling.</value>
        public OasEnumModeling EnumModeling { get; set; } = OasEnumModeling.AsString;

        /// <summary>Gets or sets the naming policy.</summary>
        /// <value>The naming policy.</value>
        public JsonNamingPolicy NamingPolicy { get; set; }

        /// <summary>Gets or sets the naming policy.</summary>
        /// <value>The naming policy.</value>
        public JsonNamingPolicy EnumNamingPolicy { get; set; }
    }
}
