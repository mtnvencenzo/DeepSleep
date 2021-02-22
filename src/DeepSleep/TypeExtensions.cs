namespace DeepSleep
{
    using System;
    using System.Linq.Expressions;
    using System.Reflection;

    /// <summary>
    /// 
    /// </summary>
    internal static class TypeExtensions
    {
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

        /// <summary>Determines whether [is nullable type] [the specified type].</summary>
        /// <param name="type">The type.</param>
        /// <returns><c>true</c> if [is nullable type] [the specified type]; otherwise, <c>false</c>.</returns>
        internal static bool IsNullableType(Type type)
        {
            if (type.IsNullable())
                return true;

            if (Nullable.GetUnderlyingType(type) != null)
                return true;

            return false;
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
    }
}
