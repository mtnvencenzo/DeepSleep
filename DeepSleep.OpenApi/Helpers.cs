namespace DeepSleep.OpenApi
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;

    internal static class Helpers
    {
        #region Static Properties & Fields

        internal static HashSet<Type> NumericTypes = new HashSet<Type>
        {
            typeof(int),
            typeof(uint),
            typeof(double),
            typeof(decimal),
            typeof(short),
            typeof(ushort),
            typeof(long),
            typeof(ulong),
            typeof(float),
            typeof(byte)
        };

        internal static HashSet<Type> IntegerTypes = new HashSet<Type>
        {
            typeof(int),
            typeof(uint),
            typeof(short),
            typeof(ushort),
            typeof(long),
            typeof(ulong)
        };

        internal static HashSet<Type> DateTypes = new HashSet<Type>
        {
            typeof(DateTime),
            typeof(DateTimeOffset)
        };

        #endregion

        internal static bool IsNumericType(Type type)
        {
            return NumericTypes.Contains(type) ||
                   NumericTypes.Contains(Nullable.GetUnderlyingType(type));
        }

        internal static bool IsIntegerType(Type type)
        {
            return IntegerTypes.Contains(type) ||
                   IntegerTypes.Contains(Nullable.GetUnderlyingType(type));
        }

        internal static bool IsStringType(Type type)
        {
            if (type == typeof(string) || type == typeof(byte[]))
            {
                return true;
            }

            return false;
        }

        internal static bool IsEnumType(Type type)
        {
            return type.IsEnum || (Nullable.GetUnderlyingType(type)?.IsEnum ?? false);
        }

        internal static bool IsDateType(Type type)
        {
            return DateTypes.Contains(type) ||
                 DateTypes.Contains(Nullable.GetUnderlyingType(type));
        }

        internal static bool IsArrayType(Type type)
        {
            if (typeof(string).IsAssignableFrom(type))
            {
                return false;
            }

            return typeof(IEnumerable).IsAssignableFrom(type);
        }

        internal static Type GetArrayType(Type type)
        {
            if (type.IsArray)
                return type.GetElementType();

            // type is IEnumerable<T>;
            if (ImplIEnumT(type))
                return type.GetGenericArguments().First();

            // type implements/extends IEnumerable<T>;
            var enumType = type.GetInterfaces().Where(ImplIEnumT).Select(t => t.GetGenericArguments().First()).FirstOrDefault();
            if (enumType != null)
                return enumType;

            // type is IEnumerable
            if (IsIEnum(type) || type.GetInterfaces().Any(IsIEnum))
                return typeof(object);

            return null;

            bool IsIEnum(Type t) => t == typeof(System.Collections.IEnumerable);
            bool ImplIEnumT(Type t) => t.IsGenericType && t.GetGenericTypeDefinition() == typeof(IEnumerable<>);
        }

        internal static Type GetRootType(Type type)
        {
            var underType = Nullable.GetUnderlyingType(type);

            return underType != null
                ? underType
                : type;
        }

        internal static string GetOpenApiSchemaType(Type type)
        {
            var rootType = GetRootType(type);

            if (rootType == typeof(bool) || Nullable.GetUnderlyingType(type) == typeof(bool))
            {
                return "boolean";
            }

            if (Helpers.IsIntegerType(type))
            {
                return "integer";
            }

            if (Helpers.IsNumericType(type))
            {
                return "number";
            }

            if (Helpers.IsStringType(type) || Helpers.IsEnumType(type) || Helpers.IsDateType(type) || rootType == typeof(Guid) || rootType == typeof(Uri))
            {
                return "string";
            }

            if (typeof(IEnumerable).IsAssignableFrom(type))
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
            
            if (openApiType == "integer" && rootType == typeof(long))
            {
                return "int64";
            }

            if (openApiType == "number" && rootType == typeof(float))
            {
                return "float";
            }

            if (openApiType == "number" && rootType == typeof(double))
            {
                return "double";
            }

            if (openApiType == "string" && rootType == typeof(byte[]))
            {
                return "byte";
            }

            if (openApiType == "string" && rootType.IsAssignableFrom(typeof(Stream)))
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

        internal static bool IsComplexType(Type type)
        {
            var rootType = GetRootType(type);

            return !IsNumericType(rootType) &&
                !IsIntegerType(rootType) &&
                !IsStringType(rootType) &&
                !IsArrayType(rootType) &&
                !IsDateType(rootType) &&
                rootType != typeof(bool) &&
                rootType != typeof(object) &&
                rootType != typeof(Guid) &&
                rootType != typeof(Uri);
        }

        internal static string GetDocumentTypeSchemaName(Type type)
        {
            var rootType = GetRootType(type);

            var typeName = (DefaultOpenApiGenerator.PrefixNamesWithNamespace)
                ? rootType.FullName
                : rootType.Name;

            if (IsArrayType(rootType))
            {
                return $"ArrayOf{typeName}";
            }

            return typeName;
        }

        internal static bool IsNullableType(Type type)
        {
            return Nullable.GetUnderlyingType(type) != null;
        }

        internal static string GetOperationId(string httpMethod, ApiRoutingItem route)
        {
            return
                httpMethod.ToUpper().Substring(0, 1) +
                httpMethod.ToLower().Substring(1) +
                route.EndpointLocation.GetEndpointMethod().Name;
        }
    }
}
