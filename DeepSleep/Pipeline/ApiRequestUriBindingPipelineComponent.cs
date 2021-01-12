namespace DeepSleep.Pipeline
{
    using DeepSleep.Configuration;
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.Linq;
    using System.Text.Json;
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
        /// <summary>Uses the API request URI binding.</summary>
        /// <param name="pipeline">The pipeline.</param>
        /// <returns></returns>
        public static IApiRequestPipeline UseApiRequestUriBinding(this IApiRequestPipeline pipeline)
        {
            return pipeline.UsePipelineComponent<ApiRequestUriBindingPipelineComponent>();
        }

        /// <summary>Processes the HTTP request URI binding.</summary>
        /// <param name="context">The context.</param>
        /// <param name="formUrlEncodedObjectSerializer">The form url serializer.</param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        internal static async Task<bool> ProcessHttpRequestUriBinding(this ApiRequestContext context, IFormUrlEncodedObjectSerializer formUrlEncodedObjectSerializer)
        {
            if (!context.RequestAborted.IsCancellationRequested)
            {
                var addedBindingError = false;

                if (context.Routing?.Route?.Location?.UriParameterType != null || (context.Request?.InvocationContext?.SimpleParameters.Count ?? 0) > 0)
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

                    if (!addedBindingError)
                    {
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

                        // ----------------------------------------
                        // Bind the Simple Parameters if exists
                        // ----------------------------------------
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
                                        context.Request.InvocationContext.SimpleParameters[simpleParameter] = ConvertValue(nameValue.Value, simpleParameter.ParameterType);
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

        /// <summary>
        /// 
        /// </summary>
        /// <param name="value"></param>
        /// <param name="convertTo"></param>
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
    }
}
