namespace DeepSleep.OpenApi
{
    using DeepSleep.Configuration;
    using DeepSleep.Media;
    using DeepSleep.OpenApi.Decorators;
    using Microsoft.OpenApi;
    using Microsoft.OpenApi.Any;
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Reflection;
    using System.Text.Json;
    using System.Text.Json.Serialization;
    using System.Threading.Tasks;

    internal static class OasHelpers
    {
        internal static IList<IOpenApiAny> GetEnumDefinition(Type type)
        {
            var rootType = TypeExtensions.GetRootType(type);

            if (rootType.IsEnum == false)
            {
                return null;
            }

            var enumItems = Enum.GetNames(rootType);

            return enumItems
                .Select(e => new OpenApiString(e))
                .Cast<IOpenApiAny>()
                .ToList();
        }

        internal static string GetOpenApiSchemaType(Type type, OasEnumModeling enumModeling = OasEnumModeling.AsString)
        {
            var rootType = TypeExtensions.GetRootType(type);

            if(TypeExtensions.IsDictionaryType(rootType))
            {
                return "object";
            }

            if (rootType == typeof(bool))
            {
                return "boolean";
            }

            if (TypeExtensions.IsIntegerType(rootType))
            {
                return "integer";
            }

            if (TypeExtensions.IsNumericType(rootType))
            {
                return "number";
            }

            if (TypeExtensions.IsEnumType(rootType) && enumModeling != OasEnumModeling.AsIntegerEnum)
            {
                return "string";
            }

            if (TypeExtensions.IsEnumType(rootType) && enumModeling == OasEnumModeling.AsIntegerEnum)
            {
                return "integer";
            }

            if (TypeExtensions.IsStringType(rootType) || TypeExtensions.IsDateType(rootType) || rootType == typeof(Guid) || rootType == typeof(Uri))
            {
                return "string";
            }

            if (typeof(IEnumerable).IsAssignableFrom(rootType))
            {
                return "array";
            }

            return "object";
        }

        internal static string GetOpenApiSchemaFormat(string openApiType, Type rootType)
        {
            if (rootType.IsEnum)
            {
                return null;
            }

            if (openApiType == "integer" && rootType == typeof(int))
            {
                return "int32";
            }

            if (openApiType == "integer" && rootType == typeof(uint))
            {
                return "int32";
            }

            if (openApiType == "integer" && rootType == typeof(short))
            {
                return "int32";
            }

            if (openApiType == "integer" && rootType == typeof(ushort))
            {
                return "int32";
            }

            if (openApiType == "integer" && rootType == typeof(byte))
            {
                return "int32";
            }

            if (openApiType == "integer" && rootType == typeof(sbyte))
            {
                return "int32";
            }

            if (openApiType == "integer" && rootType == typeof(long))
            {
                return "int64";
            }

            if (openApiType == "integer" && rootType == typeof(ulong))
            {
                return "int64";
            }

            if (openApiType == "number" && (rootType == typeof(float) || rootType == typeof(Single)))
            {
                return "float";
            }

            if (openApiType == "number" && (rootType == typeof(double) || rootType == typeof(decimal)))
            {
                return "double";
            }

            if (openApiType == "string" && rootType == typeof(byte[]))
            {
                return "byte";
            }

            if (openApiType == "string" && rootType.IsSubclassOf(typeof(Stream)))
            {
                return "binary";
            }

            if (openApiType == "string" && rootType == typeof(DateTime))
            {
                return "date-time";
            }

            if (openApiType == "string" && rootType == typeof(DateTimeOffset))
            {
                return "date-time";
            }

            if (openApiType == "string" && rootType == typeof(Guid))
            {
                return "uuid";
            }

            if (openApiType == "string" && rootType == typeof(Uri))
            {
                return "uri";
            }

            return null;
        }

        internal static string GetDocumentTypeSchemaName(Type type, bool prefixNamesWithNamespace)
        {
            var rootType = TypeExtensions.GetRootType(type);
            string typeName;


            if (TypeExtensions.IsArrayType(rootType))
            {
                var arrayType = TypeExtensions.GetArrayType(rootType);
                typeName = prefixNamesWithNamespace
                    ? arrayType.FullName
                    : arrayType.Name;

                typeName = TypeExtensions.GetNonGenericTypeName(typeName);

                return $"ArrayOf{typeName}";
            }
            else if (rootType.IsGenericType)
            {
                var genericParameters = rootType.GetGenericArguments();
                typeName = prefixNamesWithNamespace
                    ? rootType.FullName
                    : rootType.Name;

                typeName = TypeExtensions.GetNonGenericTypeName(typeName);

                var genericNames = string.Join("", genericParameters
                    .Select(p => TypeExtensions.GetNonGenericTypeName(p.Name))
                    .ToList());

                return $"{genericNames}{typeName}";
            }
            else
            {
                typeName = prefixNamesWithNamespace
                    ? rootType.FullName
                    : rootType.Name;

                typeName = TypeExtensions.GetNonGenericTypeName(typeName);

                return typeName;
            }
        }

        internal static string GetOperationId(string httpMethod, ApiRoutingItem route, bool isAutoHeadOperation)
        {
            var operationId = route.Location.MethodInfo
                .GetCustomAttributes(typeof(OasOperationAttribute), false)
                .Cast<OasOperationAttribute>()
                .FirstOrDefault()
                ?.OperationId;

            if (string.IsNullOrWhiteSpace(operationId))
            {
                var routeParts = route.Template
                    .Split(new string[] { "/" }, StringSplitOptions.RemoveEmptyEntries)
                    .Where(i => !string.IsNullOrWhiteSpace(i))
                    .Select(i => i.Trim())
                    .Where(i => !i.StartsWith("{") && !i.EndsWith("}"))
                    .Where(i => !string.IsNullOrWhiteSpace(i))
                    .Select(i => i.Trim())
                    .Select(i =>
                    {
                        return i.ToUpperInvariant().Substring(0, 1) + (i.Length == 1 ? "" : i.Substring(1));
                    })
                    .ToList();

                var name = string.Join(string.Empty, routeParts);

                operationId =
                    httpMethod.ToUpper().Substring(0, 1) +
                    httpMethod.ToLower().Substring(1) +
                    name;
            }


            if (isAutoHeadOperation)
            {
                if (operationId.StartsWith("get", StringComparison.OrdinalIgnoreCase))
                {
                    operationId = $"Head{operationId.Substring(3)}";
                }
                else if (!operationId.StartsWith("head", StringComparison.OrdinalIgnoreCase))
                {
                    operationId = $"Head{operationId}";
                }
            }

            return operationId.Trim();
        }

        internal static async Task<IList<string>> GetRequestBodyContentTypes(
            Type requestBodyType,
            IServiceProvider serviceProvider, 
            IDeepSleepRequestConfiguration routeConfiguration, 
            IDeepSleepMediaSerializerFactory formatterFactory,
            OpenApiSpecVersion specVersion = OpenApiSpecVersion.OpenApi3_0)
        {
            var formatterTypes = routeConfiguration?.ReadWriteConfiguration?.ReadableMediaTypes
                ?? formatterFactory?.GetReadableTypes(objType: requestBodyType, overridingFormatters: null)
                ?? new List<string>();


            if (routeConfiguration?.ReadWriteConfiguration?.ReaderResolver != null)
            {
                var overrides = await routeConfiguration.ReadWriteConfiguration.ReaderResolver(serviceProvider).ConfigureAwait(false);

                if (overrides?.Formatters != null)
                {
                    formatterTypes = overrides.Formatters
                        .Where(f => f != null)
                        .Where(f => f.CanHandleType(requestBodyType))
                        .Where(f => f.SupportsRead)
                        .Where(f => f.ReadableMediaTypes != null)
                        .SelectMany(f => f.ReadableMediaTypes)
                        .Distinct()
                        .ToList();

                    formatterTypes = routeConfiguration?.ReadWriteConfiguration?.ReadableMediaTypes ?? formatterTypes ?? new List<string>();
                }
                else
                {
                    formatterTypes = routeConfiguration?.ReadWriteConfiguration?.ReadableMediaTypes ?? new List<string>();
                }
            }

            if (specVersion == OpenApiSpecVersion.OpenApi2_0)
            {
                formatterTypes = formatterTypes
                    .Where(type => string.Compare(type, "application/x-www-form-urlencoded", true) != 0)
                    .ToList();
            }

            return formatterTypes.ToList();
        }

        internal static async Task<IList<string>> GetResponseBodyContentTypes(
            Type responseBodyType,
            IServiceProvider serviceProvider,
            IDeepSleepRequestConfiguration routeConfiguration,
            IDeepSleepMediaSerializerFactory formatterFactory)
        {
            var formatterTypes = routeConfiguration?.ReadWriteConfiguration?.WriteableMediaTypes
                ?? formatterFactory?.GetWriteableTypes(objType: responseBodyType, overridingFormatters: null)
                ?? new List<string>();

            if (routeConfiguration?.ReadWriteConfiguration?.WriterResolver != null)
            {
                var overrides = await routeConfiguration.ReadWriteConfiguration.WriterResolver(serviceProvider).ConfigureAwait(false);

                if (overrides?.Formatters != null)
                {
                    formatterTypes = routeConfiguration?.ReadWriteConfiguration?.WriteableMediaTypes
                        ?? formatterFactory?.GetWriteableTypes(objType: responseBodyType, overridingFormatters: overrides.Formatters)
                        ?? new List<string>();
                }
            }

            return formatterTypes?.ToList() ?? new List<string>();
        }

        internal static JsonNamingPolicy GetEnumNamingPolicy(JsonSerializerOptions options)
        {
            if (options?.Converters != null)
            {
                var converter = options.Converters
                    .Where(c => c.GetType().IsAssignableFrom(typeof(JsonStringEnumConverter)) || c.GetType().IsSubclassOf(typeof(JsonStringEnumConverter)))
                    .FirstOrDefault() as JsonStringEnumConverter;

                if (converter != null)
                {
                    var namingPolicy = converter
                        .GetType()
                        .GetField("_namingPolicy", BindingFlags.NonPublic | BindingFlags.Instance)
                        ?.GetValue(converter) as JsonNamingPolicy;

                    return namingPolicy;
                }
            }

            return null;
        }

        internal static string GetDefaultResponseDescription(string statusCode)
        {
            if (string.IsNullOrWhiteSpace(statusCode))
            {
                return string.Empty;
            }

            if (statusCode.StartsWith("1") || statusCode.StartsWith("2") || statusCode.StartsWith("3"))
            {
                return "Success";
            }

            return "Fail";
        }
    }
}
