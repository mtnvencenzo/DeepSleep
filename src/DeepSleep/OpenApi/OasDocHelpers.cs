namespace DeepSleep.OpenApi
{
    using DeepSleep.OpenApi.Decorators;
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Reflection;
    using System.Xml.Linq;

    internal static class OasDocHelpers
    {
        internal static string GetDocumenationSummary(MethodInfo methodInfo, IList<XDocument> commentDocs)
        {
            var attributeSummary = methodInfo
                .GetCustomAttributes(typeof(OasSummaryAttribute), false)
                .Cast<OasSummaryAttribute>()
                .FirstOrDefault()
                ?.Summary;

            if (!string.IsNullOrWhiteSpace(attributeSummary))
            {
                return ProcessDocumenation(attributeSummary);
            }


            var methodDocDescriptor = GenerateDocMethodDescriptor(methodInfo);

            foreach (var commentDoc in commentDocs)
            {
                var summary = commentDoc
                    .Root
                    ?.Element("members")
                    ?.Elements("member")
                    ?.Where(e => e.Attribute("name") != null)
                    ?.Where(e => e.Attribute("name").Value == methodDocDescriptor)
                    ?.FirstOrDefault()
                    ?.Element("summary")
                    ?.Nodes()
                    ?.Aggregate(string.Empty, (element, node) => element += node.ToString());

                if (summary != null)
                {
                    return ProcessDocumenation(summary);
                }
            }

            return null;
        }

        internal static string GetDocumenationSummary(Type type, IList<XDocument> commentDocs)
        {
            var attributeSummary = type
                .GetCustomAttributes(typeof(OasSummaryAttribute), false)
                .Cast<OasSummaryAttribute>()
                .FirstOrDefault()
                ?.Summary;

            if (!string.IsNullOrWhiteSpace(attributeSummary))
            {
                return ProcessDocumenation(attributeSummary);
            }

            var typeDocDescriptor = GenerateDocTypeDescriptor(type);

            foreach (var commentDoc in commentDocs)
            {
                var summary = commentDoc
                    .Root
                    ?.Element("members")
                    ?.Elements("member")
                    ?.Where(e => e.Attribute("name") != null)
                    ?.Where(e => e.Attribute("name").Value == typeDocDescriptor)
                    ?.FirstOrDefault()
                    ?.Element("summary")
                    ?.Nodes()
                    ?.Aggregate(string.Empty, (element, node) => element += node.ToString());

                if (summary != null)
                {
                    return ProcessDocumenation(summary);
                }
            }

            return null;
        }

        internal static string GetDocumenationSummary(PropertyInfo property, IList<XDocument> commentDocs)
        {
            if (property == null)
            {
                return null;
            }

            var description = property
                .GetCustomAttributes(typeof(OasDescriptionAttribute), false)
                .Cast<OasDescriptionAttribute>()
                .FirstOrDefault()
                ?.Description;

            if (!string.IsNullOrWhiteSpace(description))
            {
                return ProcessDocumenation(description);
            }

            var propertyDocDescriptor = GenerateDocPropertyDescriptor(property);

            foreach (var commentDoc in commentDocs)
            {
                var summary = commentDoc
                    .Root
                    ?.Element("members")
                    ?.Elements("member")
                    ?.Where(e => e.Attribute("name") != null)
                    ?.Where(e => e.Attribute("name").Value == propertyDocDescriptor)
                    ?.FirstOrDefault()
                    ?.Element("summary")
                    ?.Nodes()
                    ?.Aggregate(string.Empty, (element, node) => element += node.ToString());

                if (summary != null)
                {
                    return ProcessDocumenation(summary);
                }
            }

            return null;
        }

        internal static string GetDocumentationSummary(Type type, IList<XDocument> commentDocs)
        {
            var description = type
                .GetCustomAttributes(typeof(OasDescriptionAttribute), false)
                .Cast<OasDescriptionAttribute>()
                .FirstOrDefault()
                ?.Description;

            if (!string.IsNullOrWhiteSpace(description))
            {
                return ProcessDocumenation(description);
            }


            var typeDocDescriptor = GenerateDocTypeDescriptor(type);

            foreach (var commentDoc in commentDocs)
            {
                var summary = commentDoc
                    .Root
                    ?.Element("members")
                    ?.Elements("member")
                    ?.Where(e => e.Attribute("name") != null)
                    ?.Where(e => e.Attribute("name").Value == typeDocDescriptor)
                    ?.FirstOrDefault()
                    ?.Element("summary")
                    ?.Nodes()
                    ?.Aggregate(string.Empty, (element, node) => element += node.ToString());

                if (summary != null)
                {
                    return ProcessDocumenation(summary);
                }
            }

            return null;
        }

        internal static string GetFieldDocumentationSummary(Type type, string member, IList<XDocument> commentDocs)
        {
            var attrSummary = type
                .GetCustomAttributes(typeof(OasSummaryAttribute), false)
                .Cast<OasSummaryAttribute>()
                .FirstOrDefault()
                ?.Summary;

            if (!string.IsNullOrWhiteSpace(attrSummary))
            {
                return ProcessDocumenation(attrSummary);
            }


            var enumDocDescriptor = GenerateDocFieldDescriptor(type, member);

            foreach (var commentDoc in commentDocs)
            {
                var summary = commentDoc
                    .Root
                    ?.Element("members")
                    ?.Elements("member")
                    ?.Where(e => e.Attribute("name") != null)
                    ?.Where(e => e.Attribute("name").Value == enumDocDescriptor)
                    ?.FirstOrDefault()
                    ?.Element("summary")
                    ?.Nodes()
                    ?.Aggregate(string.Empty, (element, node) => element += node.ToString());

                if (summary != null)
                {
                    return ProcessDocumenation(summary);
                }
            }

            return null;
        }

        internal static string GetReturnTypeDocumentationSummary(MethodInfo methodInfo, IList<XDocument> commentDocs)
        {
            var attributeSummary = methodInfo
                .ReturnTypeCustomAttributes
                .GetCustomAttributes(typeof(OasSummaryAttribute), false)
                .Cast<OasSummaryAttribute>()
                .FirstOrDefault()
                ?.Summary;

            if (!string.IsNullOrWhiteSpace(attributeSummary))
            {
                return ProcessDocumenation(attributeSummary);
            }

            var methodDocDescriptor = GenerateDocMethodDescriptor(methodInfo);

            foreach (var commentDoc in commentDocs)
            {
                var returns = commentDoc
                    .Root
                    ?.Element("members")
                    ?.Elements("member")
                    ?.Where(e => e.Attribute("name") != null)
                    ?.Where(e => e.Attribute("name").Value == methodDocDescriptor)
                    ?.FirstOrDefault()
                    ?.Element("returns")
                    ?.Nodes()
                    ?.Aggregate(string.Empty, (element, node) => element += node.ToString());

                if (returns != null)
                {
                    return ProcessDocumenation(returns);
                }
            }


            return null;
        }

        internal static string GetDocumentationDescription(MethodInfo methodInfo, IList<XDocument> commentDocs)
        {
            var description = methodInfo
                .GetCustomAttributes(typeof(OasDescriptionAttribute), false)
                .Cast<OasDescriptionAttribute>()
                .FirstOrDefault()
                ?.Description;

            if (!string.IsNullOrWhiteSpace(description))
            {
                return ProcessDocumenation(description);
            }

            var methodDocDescriptor = GenerateDocMethodDescriptor(methodInfo);

            foreach (var commentDoc in commentDocs)
            {
                var remarks = commentDoc
                    .Root
                    ?.Element("members")
                    ?.Elements("member")
                    ?.Where(e => e.Attribute("name") != null)
                    ?.Where(e => e.Attribute("name").Value == methodDocDescriptor)
                    ?.FirstOrDefault()
                    ?.Element("remarks")
                    ?.Nodes()
                    ?.Aggregate(string.Empty, (element, node) => element += node.ToString());

                if (remarks != null)
                {
                    return ProcessDocumenation(remarks);
                }
            }

            return string.Empty;
        }

        internal static string GetParameterDescription(MethodInfo methodInfo, ParameterInfo parameter, IList<XDocument> commentDocs)
        {
            if (methodInfo == null || parameter == null)
            {
                return null;
            }

            var description = parameter
                .GetCustomAttributes(typeof(OasDescriptionAttribute), false)
                .Cast<OasDescriptionAttribute>()
                .FirstOrDefault()
                ?.Description;

            if (!string.IsNullOrWhiteSpace(description))
            {
                return ProcessDocumenation(description);
            }

            var methodDocDescriptor = GenerateDocMethodDescriptor(methodInfo);

            foreach (var commentDoc in commentDocs)
            {
                var summary = commentDoc
                    .Root
                    ?.Element("members")
                    ?.Elements("member")
                    ?.Where(e => e.Attribute("name") != null)
                    ?.Where(e => e.Attribute("name").Value == methodDocDescriptor)
                    ?.FirstOrDefault()
                    ?.Elements("param")
                    ?.Where(e => e.Attribute("name") != null)
                    ?.Where(e => e.Attribute("name").Value == parameter.Name)
                    ?.FirstOrDefault()
                    ?.Nodes()
                    ?.Aggregate(string.Empty, (element, node) => element += node.ToString());

                if (summary != null)
                {
                    return ProcessDocumenation(summary);
                }
            }

            return null;
        }

        internal static string GenerateDocMethodDescriptor(MethodInfo methodInfo)
        {
            var name = $"M:{GenerateTypeDescriptor(methodInfo.DeclaringType)}.{methodInfo.Name}(";

            var parameters = methodInfo.GetParameters();

            foreach (var p in parameters)
            {
                name += $"{GenerateTypeDescriptor(p.ParameterType)},";
            }
            name = name.Substring(0, name.Length - 1);
            name += ")";

            return name;
        }

        internal static string GenerateDocPropertyDescriptor(PropertyInfo property)
        {
            var name = $"P:{GenerateTypeDescriptor(property.DeclaringType)}.{property.Name}";

            return name;
        }

        internal static string GenerateDocFieldDescriptor(Type type, string member)
        {
            var name = $"F:{GenerateTypeDescriptor(type)}.{member}";

            return name;
        }

        internal static string GenerateDocTypeDescriptor(Type type)
        {
            var name = $"T:{GenerateTypeDescriptor(type)}";

            return name;
        }

        internal static string GenerateTypeDescriptor(Type type)
        {
            if (type.IsGenericType)
            {
                var typeName = type.Name.Contains("`")
                    ? type.Name.Substring(0, type.Name.IndexOf("`"))
                    : type.Name;

                var name = $"{type.Namespace}.{typeName}{{";

                var parameters = type.GetGenericArguments();
                parameters.ToList().ForEach(p => name += $"{GenerateTypeDescriptor(p)},");

                name = name.Substring(0, name.Length - 1);
                name += "}";
                return name;
            }
            else
            {
                return type.FullName;
            }
        }

        internal static string ProcessDocumenation(string doc)
        {
            var input = doc ?? string.Empty;

            input = input
                .Trim()
                .Replace("<see href", "<a target=\"_blank\" href")
                .TrimStart()
                .TrimEnd()
                .Trim();

            string output = string.Empty;
            string line;

            using (var reader = new StringReader(input))
            {
                while (true)
                {
                    line = reader.ReadLine();

                    if (line != null)
                    {
                        output += line
                            .TrimStart()
                            .TrimEnd();

                        output += Environment.NewLine;
                    }
                    else
                    {
                        break;
                    }
                }
            }

            return output
                .TrimStart()
                .TrimEnd();
        }
    }
}
