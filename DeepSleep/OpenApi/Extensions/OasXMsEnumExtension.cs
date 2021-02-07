namespace DeepSleep.OpenApi.Extensions
{
    using Microsoft.OpenApi;
    using Microsoft.OpenApi.Interfaces;
    using Microsoft.OpenApi.Writers;
    using System;
    using System.Collections.Generic;
    using System.Text.Json;
    using System.Xml.Linq;

    /// <summary>
    /// 
    /// </summary>
    /// <seealso cref="Microsoft.OpenApi.Interfaces.IOpenApiExtension" />
    public class OasXMsEnumExtension : IOpenApiExtension
    {
        private readonly Type enumType;
        private readonly IList<XDocument> commentDocs;
        private readonly OasEnumModeling enumModeling;
        private readonly JsonNamingPolicy namingPolicy;

        /// <summary>Initializes a new instance of the <see cref="OasXMsEnumExtension" /> class.</summary>
        /// <param name="enumType">Type of the enum.</param>
        /// <param name="commentDocs">The comment docs.</param>
        /// <param name="namingPolicy">The naming policy.</param>
        /// <param name="enumModeling">The enum modeling.</param>
        public OasXMsEnumExtension(
            Type enumType, 
            IList<XDocument> commentDocs, 
            JsonNamingPolicy namingPolicy,
            OasEnumModeling enumModeling = OasEnumModeling.AsString)
        {
            this.enumType = enumType;
            this.commentDocs = commentDocs;
            this.enumModeling = enumModeling;
            this.namingPolicy = namingPolicy;
        }

        /// <summary>Writes the specified writer.</summary>
        /// <param name="writer">The writer.</param>
        /// <param name="specVersion">The spec version.</param>
        public void Write(IOpenApiWriter writer, OpenApiSpecVersion specVersion)
        {
            writer.WriteStartObject();

            writer.WriteProperty("name", this.enumType.Name);
            writer.WriteProperty("modelAsString", this.enumModeling == OasEnumModeling.AsString);
            writer.WritePropertyName("values");

            var items = Enum.GetValues(enumType);

            writer.WriteStartArray();

            foreach (var item in items)
            {
                var summary = OasDocHelpers.GetFieldDocumentationSummary(enumType, item.ToString(), this.commentDocs);

                writer.WriteStartObject();

                writer.WriteProperty("value", this.namingPolicy.ConvertName(item.ToString()));
                writer.WriteProperty("name", this.namingPolicy.ConvertName(item.ToString()));

                if (!string.IsNullOrWhiteSpace(summary))
                {
                    writer.WriteProperty("description", summary);
                }

                writer.WriteEndObject();
            }

            writer.WriteEndArray();
            writer.WriteEndObject();
        }
    }
}
