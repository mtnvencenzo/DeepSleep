namespace DeepSleep.Pipeline
{
    using DeepSleep.Media.Converters;
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.Linq;
    using System.Reflection;
    using System.Text.Json;
    using System.Text.Json.Serialization;
    using System.Threading.Tasks;

    /// <summary>
    /// 
    /// </summary>
    public class ApiRequestUriBindingPipelineComponent : PipelineComponentBase
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ApiRequestUriBindingPipelineComponent"/> class.
        /// </summary>
        /// <param name="next">The next.</param>
        public ApiRequestUriBindingPipelineComponent(ApiRequestDelegate next)
            : base(next) { }

        /// <summary>Invokes the specified context resolver.</summary>
        /// <param name="contextResolver">The context resolver.</param>
        /// <returns></returns>
        public override async Task Invoke(IApiRequestContextResolver contextResolver)
        {
            var context = contextResolver
                 .GetContext()
                 .SetThreadCulure();

            var formUrlEncodedObjectSerializer = context?.RequestServices?.GetService<IFormUrlEncodedObjectSerializer>();

            if (await context.ProcessHttpRequestUriBinding(formUrlEncodedObjectSerializer).ConfigureAwait(false))
            {
                await apinext.Invoke(contextResolver).ConfigureAwait(false);
            }
        }
    }

    /// <summary>
    /// 
    /// </summary>
    public static class ApiRequestUriBindingPipelineComponentExtensionMethods
    {
        private static readonly JsonSerializerOptions arraySerializationOptions;

        static ApiRequestUriBindingPipelineComponentExtensionMethods()
        {
            arraySerializationOptions = new JsonSerializerOptions
            {
                AllowTrailingCommas = false,
                DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingDefault,
                IgnoreReadOnlyFields = true,
                IgnoreReadOnlyProperties = true,
                IncludeFields = false,
                NumberHandling = JsonNumberHandling.AllowReadingFromString,
                PropertyNameCaseInsensitive = true,
                ReadCommentHandling = JsonCommentHandling.Skip,
            };

            arraySerializationOptions.Converters.Add(new NullableBooleanConverter());
            arraySerializationOptions.Converters.Add(new BooleanConverter());
            arraySerializationOptions.Converters.Add(new JsonStringEnumConverter(allowIntegerValues: true));
            arraySerializationOptions.Converters.Add(new NullableTimeSpanConverter());
            arraySerializationOptions.Converters.Add(new TimeSpanConverter());
            arraySerializationOptions.Converters.Add(new NullableDateTimeConverter());
            arraySerializationOptions.Converters.Add(new DateTimeConverter());
            arraySerializationOptions.Converters.Add(new NullableDateTimeOffsetConverter());
            arraySerializationOptions.Converters.Add(new DateTimeOffsetConverter());
            arraySerializationOptions.Converters.Add(new ObjectConverter());
            arraySerializationOptions.Converters.Add(new ContentDispositionConverter());
            arraySerializationOptions.Converters.Add(new ContentTypeConverter());
        }

        /// <summary>Uses the API request URI binding.</summary>
        /// <param name="pipeline">The pipeline.</param>
        /// <returns></returns>
        public static IApiRequestPipeline UseApiRequestUriBinding(this IApiRequestPipeline pipeline)
        {
            return pipeline.UsePipelineComponent<ApiRequestUriBindingPipelineComponent>();
        }

        /// <summary>Processes the HTTP request URI binding.</summary>
        /// <param name="context">The context.</param>
        /// <param name="formUrlEncodedObjectSerializer">The form URL encoded object serializer.</param>
        /// <returns></returns>
        internal static async Task<bool> ProcessHttpRequestUriBinding(this ApiRequestContext context, IFormUrlEncodedObjectSerializer formUrlEncodedObjectSerializer)
        {
            if (!context.RequestAborted.IsCancellationRequested)
            {
                var addedBindingError = false;

                if (context.Routing?.Route?.Location?.UriParameterType != null || (context.Routing?.Route?.Location?.SimpleParameters.Count ?? 0) > 0)
                {
                    var nameValues = new Dictionary<string, string>();

                    if ((context.Routing?.Route?.RouteVariables?.Count ?? 0) > 0)
                    {
                        foreach (var routeVar in context.Routing.Route.RouteVariables.Keys)
                        {
                            if (!nameValues.ContainsKey(routeVar))
                            {
                                nameValues.Add(routeVar, context.Routing.Route.RouteVariables[routeVar]);
                            }
                        }
                    }

                    if ((context.Request.QueryVariables?.Count ?? 0) > 0)
                    {
                        foreach (var qvar in context.Request.QueryVariables.Keys)
                        {
                            if (!nameValues.ContainsKey(qvar))
                            {
                                nameValues.Add(qvar, context.Request.QueryVariables[qvar]);
                            }
                        }
                    }

                    // ----------------------------------------
                    // Bind the Simple Parameters if exists
                    // ----------------------------------------
                    var simpleParameters = new Dictionary<ParameterInfo, object>();

                    if (context.Routing.Route.Location.SimpleParameters != null)
                    {
                        context.Request.InvocationContext.SimpleParameters = context.Routing.Route.Location.SimpleParameters
                            .ToDictionary((k) => k, (k) => null as object);
                    }

                    if ((context.Request.InvocationContext?.SimpleParameters.Count ?? 0) > 0)
                    {
                        foreach (var nameValue in nameValues)
                        {
                            var simpleParameter = context.Request.InvocationContext.SimpleParameters
                                .Where(s => s.Value == null)
                                .FirstOrDefault(p => p.Key.Name == nameValue.Key).Key;

                            // case insensitive check
                            if (simpleParameter == null)
                            {
                                simpleParameter = context.Request.InvocationContext.SimpleParameters
                                    .Where(s => s.Value == null)
                                    .FirstOrDefault(p => string.Compare(p.Key.Name, nameValue.Key, true) == 0).Key;
                            }

                            if (simpleParameter != null && context.Request.InvocationContext.SimpleParameters.ContainsKey(simpleParameter))
                            {
                                try
                                {
                                    context.Request.InvocationContext.SimpleParameters[simpleParameter] = TypeExtensions.IsArrayType(simpleParameter.ParameterType)
                                        ? ConvertArrayValue(nameValue.Value, simpleParameter.ParameterType)
                                        : ConvertValue(nameValue.Value, simpleParameter.ParameterType);
                                }
                                catch
                                {
                                    addedBindingError = true;

                                    var error = context.Configuration?.ValidationErrorConfiguration?.UriBindingValueError ?? string.Empty;

                                    var errorMessage = error
                                        .Replace("{paramName}", simpleParameter.Name)
                                        .Replace("{paramValue}", nameValue.Value ?? string.Empty)
                                        .Replace("{paramType}", simpleParameter.ParameterType.Name);

                                    if (!string.IsNullOrWhiteSpace(errorMessage))
                                    {
                                        context.Validation.Errors.Add(errorMessage);
                                    }
                                }
                            }
                        }
                    }

                    // ----------------------------------------
                    // Bind the UrlModel if UrlModelType exists
                    // ----------------------------------------
                    if (context.Routing.Route.Location.UriParameterType != null)
                    {
                        var bindingValues = nameValues
                            .Select(kv => $"{kv.Key}={kv.Value}");

                        var formUrlEncoded = string.Join("&", bindingValues);

                        try
                        {
                            var uriModel = await formUrlEncodedObjectSerializer.Deserialize(
                                formUrlEncoded,
                                context.Routing.Route.Location.UriParameterType,
                                true).ConfigureAwait(false);

                            context.Request.InvocationContext.UriModel = uriModel;
                        }
                        catch (JsonException ex)
                        {
                            addedBindingError = true;

                            var error = context.Configuration?.ValidationErrorConfiguration?.UriBindingError ?? string.Empty;

                            var errorMessage = error.Replace("{paramName}", ex.Path?.TrimStart('$', '.') ?? string.Empty);

                            if (!string.IsNullOrWhiteSpace(errorMessage))
                            {
                                context.Validation.Errors.Add(errorMessage);
                            }
                        }
                    }
                }

                if (addedBindingError)
                {
                    context.Response.StatusCode = context.Configuration?.ValidationErrorConfiguration?.UriBindingErrorStatusCode ?? 400;

                    return false;
                }

                return true;
            }

            return false;
        }

        /// <summary>Converts the value.</summary>
        /// <param name="value">The value.</param>
        /// <param name="convertTo">The convert to.</param>
        /// <returns></returns>
        private static object ConvertValue(string value, Type convertTo)
        {
            var type = Nullable.GetUnderlyingType(convertTo) ?? convertTo;

            if (value == null)
            {
                return convertTo.IsNullable()
                    ? null
                    : type.GetDefaultValue();
            }

            if (string.IsNullOrWhiteSpace(value) && convertTo.IsNullable())
            {
                return null;
            }
            else if (string.IsNullOrWhiteSpace(value))
            {
                return type.GetDefaultValue();
            }

            if (type == typeof(string))
            {
                return value;
            }

            if (type == typeof(DateTime))
            {
                return DateTimeOffset.Parse(value, CultureInfo.CurrentCulture).UtcDateTime;
            }

            if (type == typeof(DateTimeOffset))
            {
                return DateTimeOffset.Parse(value, CultureInfo.CurrentCulture);
            }

            if (type == typeof(TimeSpan))
            {
                return TimeSpan.Parse(value, CultureInfo.CurrentCulture);
            }

            if (type == typeof(bool) && value == "1")
            {
                return true;
            }

            if (type == typeof(bool) && value == "0")
            {
                return false;
            }

            if (type == typeof(Guid))
            {
                return Guid.Parse(value);
            }

            if (type.IsEnum)
            {
                return Enum.Parse(type, value, true);
            }

            return Convert.ChangeType(value, type, CultureInfo.CurrentCulture);
        }

        /// <summary>Converts the array value.</summary>
        /// <param name="value">The value.</param>
        /// <param name="convertTo">The convert to.</param>
        /// <returns></returns>
        private static object ConvertArrayValue(string value, Type convertTo)
        {
            if (value == null)
            {
                return null;
            }

            var stringValues = new List<string>();

            foreach (var val in value.Split(new string[] { "," }, StringSplitOptions.RemoveEmptyEntries))
            {
                stringValues.Add(val);
            }

            var serialized = JsonSerializer.Serialize(stringValues, stringValues.GetType());

            var obj = JsonSerializer.Deserialize(serialized, convertTo, arraySerializationOptions);
            return obj;
        }
    }
}
