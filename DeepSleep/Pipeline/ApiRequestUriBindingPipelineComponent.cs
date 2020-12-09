namespace DeepSleep.Pipeline
{
    using DeepSleep.Resources;
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.Linq;
    using System.Text.Json;
    using System.Threading.Tasks;
    using System.Web;

    /// <summary>
    /// 
    /// </summary>
    public class ApiRequestUriBindingPipelineComponent : PipelineComponentBase
    {
        private readonly ApiRequestDelegate apinext;

        /// <summary>
        /// Initializes a new instance of the <see cref="ApiRequestUriBindingPipelineComponent"/> class.
        /// </summary>
        /// <param name="next">The next.</param>
        public ApiRequestUriBindingPipelineComponent(ApiRequestDelegate next)
        {
            apinext = next;
        }

        /// <summary>Invokes the specified context resolver.</summary>
        /// <param name="contextResolver">The context resolver.</param>
        /// <param name="formUrlEncodedObjectSerializer">The form url serializer.</param>
        /// <returns></returns>
        public async Task Invoke(IApiRequestContextResolver contextResolver, IFormUrlEncodedObjectSerializer formUrlEncodedObjectSerializer)
        {
            var context = contextResolver.GetContext();

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

                if (context.RequestInfo.InvocationContext?.UriModelType != null || (context.RequestInfo.InvocationContext?.SimpleParameters.Count ?? 0) > 0)
                {
                    var nameValues = new Dictionary<string, string>();

                    if ((context.RouteInfo?.RoutingItem?.RouteVariables?.Count ?? 0) > 0)
                    {
                        foreach (var routeVar in context.RouteInfo.RoutingItem.RouteVariables.Keys)
                        {
                            if (nameValues.ContainsKey(routeVar))
                            {
                                addedBindingError = true;
                                context.ErrorMessages.Add(string.Format(ValidationErrors.UriRouteBindingError, routeVar));
                            }
                            else
                            {
                                nameValues.Add(routeVar, context.RouteInfo.RoutingItem.RouteVariables[routeVar]);
                            }
                        }
                    }

                    if ((context.RequestInfo.QueryVariables?.Count ?? 0) > 0)
                    {
                        foreach (var qvar in context.RequestInfo.QueryVariables.Keys)
                        {
                            if (!nameValues.ContainsKey(qvar))
                            {
                                nameValues.Add(qvar, context.RequestInfo.QueryVariables[qvar]);
                            }
                        }
                    }

                    if (!addedBindingError)
                    {
                        // ----------------------------------------
                        // Bind the UrlModel if UrlModelType exists
                        // ----------------------------------------
                        if (context.RequestInfo.InvocationContext?.UriModelType != null)
                        {
                            var bindingValues = nameValues
                                .Select(kv => $"{kv.Key}={kv.Value}");

                            var formUrlEncoded = string.Join("&", bindingValues);

                            try
                            {
                                var uriModel = await formUrlEncodedObjectSerializer.Deserialize(
                                    formUrlEncoded,
                                    context.RequestInfo.InvocationContext.UriModelType,
                                    true).ConfigureAwait(false);

                                context.RequestInfo.InvocationContext.UriModel = uriModel;
                            }
                            catch (JsonException ex)
                            {
                                addedBindingError = true;
                                context.ErrorMessages.Add(string.Format(ValidationErrors.UriBindingError, ex.Path.TrimStart('$', '.')));
                            }
                        }

                        // ----------------------------------------
                        // Bind the Simple Parameters if exists
                        // ----------------------------------------
                        if ((context.RequestInfo.InvocationContext?.SimpleParameters.Count ?? 0) > 0)
                        {
                            foreach (var nameValue in nameValues)
                            {
                                var simpleParameter = context.RequestInfo.InvocationContext.SimpleParameters
                                    .Where(s => s.Value == null)
                                    .FirstOrDefault(p => p.Key.Name == nameValue.Key).Key;

                                // case insensitive check
                                if (simpleParameter == null)
                                {
                                    simpleParameter = context.RequestInfo.InvocationContext.SimpleParameters
                                        .Where(s => s.Value == null)
                                        .FirstOrDefault(p => string.Compare(p.Key.Name, nameValue.Key, true) == 0).Key;
                                }

                                if (simpleParameter != null && context.RequestInfo.InvocationContext.SimpleParameters.ContainsKey(simpleParameter))
                                {
                                    try
                                    {
                                        context.RequestInfo.InvocationContext.SimpleParameters[simpleParameter] = ConvertValue(nameValue.Value, simpleParameter.ParameterType);
                                    }
                                    catch
                                    {
                                        addedBindingError = true;
                                        context.ErrorMessages.Add(
                                            string.Format(ValidationErrors.UriBindingValueError, simpleParameter.Name, nameValue.Value, simpleParameter.ParameterType.Name));
                                    }
                                }
                            }
                        }
                    }
                }

                if (addedBindingError)
                {
                    context.ResponseInfo.StatusCode = 400;
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
