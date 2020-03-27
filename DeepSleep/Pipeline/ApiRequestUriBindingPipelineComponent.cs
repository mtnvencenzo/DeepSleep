namespace DeepSleep.Pipeline
{
    using DeepSleep.Resources;
    using Microsoft.Extensions.Logging;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;
    using System.Threading.Tasks;

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
        /// <param name="serviceProvider">The service provider.</param>
        /// <param name="responseMessageConverter">The response message converter.</param>
        /// <param name="logger">The logger.</param>
        /// <returns></returns>
        public async Task Invoke(IApiRequestContextResolver contextResolver, IServiceProvider serviceProvider, IApiResponseMessageConverter responseMessageConverter, ILogger<ApiRequestUriBindingPipelineComponent> logger)
        {
            var context = contextResolver.GetContext();

            if (await context.ProcessHttpRequestUriBinding(serviceProvider, responseMessageConverter, logger).ConfigureAwait(false))
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

        /// <summary>Converts the value.</summary>
        /// <param name="value">The value.</param>
        /// <param name="convertTo">The convert to.</param>
        /// <returns></returns>
        private static object ConvertValue(string value, Type convertTo)
        {
            var type = Nullable.GetUnderlyingType(convertTo) ?? convertTo;

            if (value == null)
            {
                return type.GetDefaultValue();
            }

            if (type == typeof(string))
            {
                return value;
            }

            if (type == typeof(DateTime))
            {
                return DateTimeOffset.Parse(value).UtcDateTime;
            }

            if (type == typeof(DateTimeOffset))
            {
                return DateTimeOffset.Parse(value);
            }

            if (type == typeof(TimeSpan))
            {
                return TimeSpan.Parse(value);
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

            return Convert.ChangeType(value, type);
        }

        /// <summary>Processes the HTTP request URI binding.</summary>
        /// <param name="context">The context.</param>
        /// <param name="serviceProvider">The service provider.</param>
        /// <param name="responseMessageConverter">The response message converter.</param>
        /// <param name="logger">The logger.</param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public static Task<bool> ProcessHttpRequestUriBinding(this ApiRequestContext context, IServiceProvider serviceProvider, IApiResponseMessageConverter responseMessageConverter, ILogger logger)
        {
            logger?.LogInformation("Invoked");

            if (!context.RequestAborted.IsCancellationRequested)
            {
                var addedBindingError = false;
                object uriModel = null;

                if (context.RequestInfo.InvocationContext?.UriModelType != null)
                {

                    try
                    {
                        uriModel = serviceProvider?.GetService(context.RequestInfo.InvocationContext.UriModelType);
                    }
                    catch (Exception) { }


                    if (uriModel == null)
                    {
                        try
                        {
                            uriModel = Activator.CreateInstance(context.RequestInfo.InvocationContext.UriModelType);
                        }
                        catch (Exception ex)
                        {
                            throw new Exception(string.Format("Unable to instantiate UriModel '{0}'", context.RequestInfo.InvocationContext.UriModelType.FullName), ex);
                        }
                    }


                    var propertiesSet = new List<string>();
                    Action<PropertyInfo, string> propertySetter = (p, v) =>
                    {
                        if (p != null && !propertiesSet.Contains(p.Name))
                        {
                            try
                            {
                                var convertedValue = ConvertValue(v, p.PropertyType);

                                p.SetValue(uriModel, convertedValue);
                                propertiesSet.Add(p.Name);
                            }
                            catch (Exception)
                            {
                                addedBindingError = true;
                                context.ProcessingInfo.ExtendedMessages.Add(responseMessageConverter.Convert(string.Format(ValidationErrors.UriRouteBindingError,
                                    v,
                                    p.Name)));
                            }
                        }
                    };

                    var properties = context.RequestInfo.InvocationContext.UriModelType.GetProperties()
                        .Where(p => p.CanWrite)
                        .ToArray();

                    if ((context.RouteInfo?.RoutingItem?.RouteVariables?.Count ?? 0) > 0)
                    {
                        foreach (var routeVar in context.RouteInfo.RoutingItem.RouteVariables.Keys)
                        {
                            var prop = properties.FirstOrDefault(p => string.Compare(p.Name, routeVar, true) == 0);
                            propertySetter(prop, context.RouteInfo.RoutingItem.RouteVariables[routeVar]);
                        }
                    }

                    if ((context.RequestInfo.QueryVariables?.Count ?? 0) > 0)
                    {
                        foreach (var qvar in context.RequestInfo.QueryVariables.Keys)
                        {
                            var prop = properties.Where(p => !propertiesSet.Contains(p.Name)).FirstOrDefault(p => string.Compare(p.Name, qvar, true) == 0);
                            propertySetter(prop, context.RequestInfo.QueryVariables[qvar]);
                        }
                    }
                }

                if (addedBindingError)
                {
                    context.ResponseInfo.ResponseObject = new ApiResponse
                    {
                        StatusCode = 400
                    };
                    return Task.FromResult(false);
                }

                if (context.RequestInfo.InvocationContext != null)
                {
                    context.RequestInfo.InvocationContext.UriModel = uriModel;
                }

                return Task.FromResult(true);
            }

            return Task.FromResult(false);
        }
    }
}
