namespace DeepSleep.OpenApi
{
    using Microsoft.OpenApi.Models;
    using System.Collections.Generic;

    /// <summary>
    /// 
    /// </summary>
    /// <seealso cref="DeepSleep.OpenApi.IOpenApiConfigurationProvider" />
    public class OpenApiConfigurationProvider : IOpenApiConfigurationProvider
    {
        /// <summary>Gets or sets the information.</summary>
        /// <value>The information.</value>
        public OpenApiInfo Info { get; set; }

        /// <summary>Gets or sets a value indicating whether [prefix names with namespace].</summary>
        /// <value><c>true</c> if [prefix names with namespace]; otherwise, <c>false</c>.</value>
        public bool PrefixNamesWithNamespace { get; set; }

        /// <summary>Gets or sets a value indicating whether [include head operations for gets].</summary>
        /// <value><c>true</c> if [include head operations for gets]; otherwise, <c>false</c>.</value>
        public bool IncludeHeadOperationsForGets { get; set; }

        /// <summary>Gets or sets the v2 route template.</summary>
        /// <value>The v2 route template.</value>
        public string V2RouteTemplate { get; set; }

        /// <summary>Gets or sets the v3 route template.</summary>
        /// <value>The v3 route template.</value>
        public string V3RouteTemplate { get; set; }

        /// <summary>Gets or sets the XML documentation file names.</summary>
        /// <value>The XML documentation file names.</value>
        public IList<string> XmlDocumentationFileNames { get; set; }
    }
}
