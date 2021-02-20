namespace DeepSleep.OpenApi
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Linq.Expressions;
    using System.Reflection;

    /// <summary>
    /// 
    /// </summary>
    internal static class TypeExtensions
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
            typeof(Single)
        };

        internal static HashSet<Type> IntegerTypes = new HashSet<Type>
        {
            typeof(int),
            typeof(uint),
            typeof(short),
            typeof(ushort),
            typeof(long),
            typeof(ulong),
            typeof(byte),
            typeof(sbyte)
        };

        internal static HashSet<Type> DateTypes = new HashSet<Type>
        {
            typeof(DateTime),
            typeof(DateTimeOffset),
            typeof(TimeSpan)
        };

        #endregion

        /// <summary>Determines whether [is numeric type] [the specified type].</summary>
        /// <param name="type">The type.</param>
        /// <returns><c>true</c> if [is numeric type] [the specified type]; otherwise, <c>false</c>.</returns>
        internal static bool IsNumericType(Type type)
        {
            return NumericTypes.Contains(type) ||
                   NumericTypes.Contains(Nullable.GetUnderlyingType(type));
        }

        /// <summary>Determines whether [is integer type] [the specified type].</summary>
        /// <param name="type">The type.</param>
        /// <returns><c>true</c> if [is integer type] [the specified type]; otherwise, <c>false</c>.</returns>
        internal static bool IsIntegerType(Type type)
        {
            return IntegerTypes.Contains(type) ||
                   IntegerTypes.Contains(Nullable.GetUnderlyingType(type));
        }

        /// <summary>Determines whether [is string type] [the specified type].</summary>
        /// <param name="type">The type.</param>
        /// <returns><c>true</c> if [is string type] [the specified type]; otherwise, <c>false</c>.</returns>
        internal static bool IsStringType(Type type)
        {
            if (type == typeof(object))
            {
                return false;
            }

            if (type == typeof(string) || type == typeof(byte[]) || type == typeof(char) || type == typeof(Uri) || type.IsSubclassOf(typeof(string)) || type.IsAssignableFrom(typeof(string)))
            {
                return true;
            }

            return false;
        }

        /// <summary>Determines whether [is enum type] [the specified type].</summary>
        /// <param name="type">The type.</param>
        /// <returns><c>true</c> if [is enum type] [the specified type]; otherwise, <c>false</c>.</returns>
        internal static bool IsEnumType(Type type)
        {
            return type.IsEnum || (Nullable.GetUnderlyingType(type)?.IsEnum ?? false);
        }

        /// <summary>Determines whether [is date type] [the specified type].</summary>
        /// <param name="type">The type.</param>
        /// <returns><c>true</c> if [is date type] [the specified type]; otherwise, <c>false</c>.</returns>
        internal static bool IsDateType(Type type)
        {
            return DateTypes.Contains(type) ||
                 DateTypes.Contains(Nullable.GetUnderlyingType(type));
        }

        /// <summary>Determines whether [is array type] [the specified type].</summary>
        /// <param name="type">The type.</param>
        /// <returns><c>true</c> if [is array type] [the specified type]; otherwise, <c>false</c>.</returns>
        internal static bool IsArrayType(Type type)
        {
            if (typeof(string).IsAssignableFrom(type))
            {
                return false;
            }

            return typeof(System.Collections.IEnumerable).IsAssignableFrom(type);
        }

        /// <summary>Determines whether [is dictionary type] [the specified type].</summary>
        /// <param name="type">The type.</param>
        /// <returns><c>true</c> if [is dictionary type] [the specified type]; otherwise, <c>false</c>.</returns>
        internal static bool IsDictionaryType(Type type)
        {
            if (typeof(System.Collections.IDictionary).IsAssignableFrom(type))
            {
                return true;
            }

            if (type.IsGenericType)
            {
                if (typeof(System.Collections.Generic.IDictionary<,>).IsAssignableFrom(type.GetGenericTypeDefinition()))
                {
                    return true;
                }
            }

            return false;
        }

        /// <summary>Gets the type of the dictionary value.</summary>
        /// <param name="type">The type.</param>
        /// <returns></returns>
        internal static Type GetDictionaryValueType(Type type)
        {
            if (type.IsGenericType)
            {
                return type.GetGenericArguments()[1];
            }

            if (typeof(System.Collections.IDictionary).IsAssignableFrom(type))
            {
                return typeof(object);
            }

            return typeof(object);
        }

        /// <summary>Gets the type of the array.</summary>
        /// <param name="type">The type.</param>
        /// <returns></returns>
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

        /// <summary>Gets the type of the root.</summary>
        /// <param name="type">The type.</param>
        /// <returns></returns>
        internal static Type GetRootType(Type type)
        {
            var underType = Nullable.GetUnderlyingType(type);

            return underType != null
                ? underType
                : type;
        }

        /// <summary>Determines whether [is nullable type] [the specified type].</summary>
        /// <param name="type">The type.</param>
        /// <returns><c>true</c> if [is nullable type] [the specified type]; otherwise, <c>false</c>.</returns>
        internal static bool IsNullableType(Type type)
        {
            if (type.IsNullable())
                return true;

            if (Nullable.GetUnderlyingType(type) != null)
                return true;

            //if (!type.IsValueType)
            //    return true;

            return false;
        }

        /// <summary>Determines whether [is complex type] [the specified type].</summary>
        /// <param name="type">The type.</param>
        /// <returns><c>true</c> if [is complex type] [the specified type]; otherwise, <c>false</c>.</returns>
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

        /// <summary>Determines whether the specified type is nullable.</summary>
        /// <param name="type">The type.</param>
        /// <returns></returns>
        internal static bool IsNullable(this Type type)
        {
            return type.GetTypeInfo().IsGenericType && type.GetGenericTypeDefinition() == typeof(Nullable<>);
        }

        /// <summary>Gets the default value.</summary>
        /// <param name="type">The type. <see cref="System.Type"/></param>
        /// <returns>The <see cref="object"/>.</returns>
        internal static object GetDefaultValue(this Type type)
        {
            // We want an Func<object> which returns the default.
            // Create that expression here.
            Expression<Func<object>> e = Expression.Lambda<Func<object>>(
                // Have to convert to object.
                Expression.Convert(
                    // The default value, always get what the *code* tells us.
                    Expression.Default(type),
                    typeof(object)));

            // Compile and return the value.
            return e.Compile()();
        }

        /// <summary>Gets the name of the non generic type.</summary>
        /// <param name="name">The name.</param>
        /// <returns></returns>
        internal static string GetNonGenericTypeName(string name)
        {
            if (name.Contains("`"))
            {
                return name.Substring(0, name.IndexOf("`"));
            }

            return name;
        }
    }
}
