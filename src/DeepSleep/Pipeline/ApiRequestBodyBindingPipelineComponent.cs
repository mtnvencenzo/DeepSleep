namespace DeepSleep.Pipeline
{
    using DeepSleep.Media;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    /// <summary>
    /// 
    /// </summary>
    public class ApiRequestBodyBindingPipelineComponent : PipelineComponentBase
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ApiRequestBodyBindingPipelineComponent"/> class.
        /// </summary>
        /// <param name="next">The next.</param>
        public ApiRequestBodyBindingPipelineComponent(ApiRequestDelegate next)
            : base(next) { }

        /// <summary>Invokes the specified context resolver.</summary>
        /// <param name="contextResolver">The context resolver.</param>
        /// <returns></returns>
        public override async Task Invoke(IApiRequestContextResolver contextResolver)
        {
            var context = contextResolver
                 .GetContext()
                 .SetThreadCulure();

            var formatterFactory = context?.RequestServices?.GetService<IDeepSleepMediaSerializerFactory>();

            if (await context.ProcessHttpRequestBodyBinding(contextResolver, formatterFactory).ConfigureAwait(false))
            {
                await apinext.Invoke(contextResolver).ConfigureAwait(false);
            }
        }
    }

    /// <summary>
    /// 
    /// </summary>
    public static class ApiRequestBodyBindingPipelineComponentExtensionMethods
    {
        /// <summary>Uses the API request body binding.</summary>
        /// <param name="pipeline">The pipeline.</param>
        /// <returns></returns>
        public static IApiRequestPipeline UseApiRequestBodyBinding(this IApiRequestPipeline pipeline)
        {
            return pipeline.UsePipelineComponent<ApiRequestBodyBindingPipelineComponent>();
        }

        /// <summary>Processes the HTTP request body binding.</summary>
        /// <param name="context">The context.</param>
        /// <param name="contextResolver">The API request context resolver.</param>
        /// <param name="formatterFactory">The formatter factory.</param>
        /// <returns></returns>
        internal static async Task<bool> ProcessHttpRequestBodyBinding(this ApiRequestContext context, IApiRequestContextResolver contextResolver, IDeepSleepMediaSerializerFactory formatterFactory)
        {
            if (!context.RequestAborted.IsCancellationRequested)
            {
                if (context.Request.Method?.In(StringComparison.InvariantCultureIgnoreCase, "post", "patch", "put") ?? false)
                {
                    if (!context.Request.ContentLength.HasValue)
                    {
                        if (context.Configuration?.RequestValidation?.RequireContentLengthOnRequestBodyRequests ?? true)
                        {
                            context.Response.StatusCode = 411;
                            return false;
                        }
                    }

                    if (context.Request.ContentLength > 0 && string.IsNullOrWhiteSpace(context.Request.ContentType))
                    {
                        context.Response.StatusCode = 415;
                        return false;
                    }

                    if (context.Configuration?.RequestValidation?.MaxRequestLength > 0 && context.Request.ContentLength > 0)
                    {
                        if (context.Request.ContentLength > context.Configuration.RequestValidation.MaxRequestLength)
                        {
                            context.Response.StatusCode = 413;
                            return false;
                        }
                    }

                    if (context.Request.ContentLength > 0 && context.Routing?.Route?.Location?.BodyParameterType == null)
                    {
                        if (!(context.Configuration?.RequestValidation?.AllowRequestBodyWhenNoModelDefined ?? false))
                        {
                            context.Response.StatusCode = 413;
                            return false;
                        }
                    }

                    if (context.Routing.Route.Location.BodyParameterType != null && context.Request.ContentLength > 0 && !string.IsNullOrWhiteSpace(context.Request.ContentType))
                    {
                        IDeepSleepMediaSerializer formatter = null;

                        var formatterTypes = context.Configuration?.ReadWriteConfiguration?.ReadableMediaTypes 
                            ?? formatterFactory?.GetReadableTypes(objType: context.Routing.Route.Location.BodyParameterType, overridingFormatters: null) 
                            ?? new List<string>();

                        if (formatterFactory != null)
                        {
                            formatter = await formatterFactory.GetContentTypeFormatter(
                                contentTypeHeader: context.Request.ContentType,
                                objType: context.Routing.Route.Location.BodyParameterType,
                                formatterType: out var _,
                                readableMediaTypes: context.Configuration?.ReadWriteConfiguration?.ReadableMediaTypes).ConfigureAwait(false);
                        }

                        if (context.Configuration.ReadWriteConfiguration?.ReaderResolver != null)
                        {
                            var overrides = await context.Configuration.ReadWriteConfiguration.ReaderResolver(context?.RequestServices).ConfigureAwait(false);

                            if (overrides?.Formatters != null)
                            {
                                formatter = await formatterFactory.GetContentTypeFormatter(
                                    contentTypeHeader: context.Request.ContentType,
                                    objType: context.Routing.Route.Location.BodyParameterType,
                                    formatterType: out var _,
                                    readableFormatters: overrides.Formatters,
                                    readableMediaTypes: context.Configuration?.ReadWriteConfiguration?.ReadableMediaTypes).ConfigureAwait(false);

                                formatterTypes = overrides.Formatters
                                    .Where(f => f != null)
                                    .Where(f => f.SupportsRead)
                                    .Where(f => f.ReadableMediaTypes != null)
                                    .Where(f => f.CanHandleType(context.Routing.Route.Location.BodyParameterType))
                                    .SelectMany(f => f.ReadableMediaTypes)
                                    .Distinct()
                                    .ToList();

                                formatterTypes = context.Configuration?.ReadWriteConfiguration?.ReadableMediaTypes ?? formatterTypes ?? new List<string>();
                            }
                            else
                            {
                                formatterTypes = context.Configuration?.ReadWriteConfiguration?.ReadableMediaTypes ?? new List<string>();
                            }
                        }



                        if (formatter == null)
                        {
                            context.Response.StatusCode = 415;

                            context.Response.AddHeader(
                                name: "X-Allow-Content-Types",
                                value: string.Join(", ", formatterTypes),
                                append: false,
                                allowMultiple: false);

                            return false;
                        }


                        Encoding contextTypeEncoding = null;

                        if (context.Request.ContentType != null as string)
                        {
                            if (!string.IsNullOrWhiteSpace(context.Request.ContentType.Charset))
                            {
                                try
                                {
                                    contextTypeEncoding = Encoding.GetEncoding(context.Request.ContentType.Charset);
                                }
                                catch { }
                            }
                        }


                        try
                        {
                            context.Request.InvocationContext.BodyModel = await formatter.ReadType(
                                stream: context.Request.Body,
                                objType: context.Routing.Route.Location.BodyParameterType,
                                options: new MediaSerializerOptions
                                {
                                    Culture = context.Request.AcceptCulture,
                                    Encoding = contextTypeEncoding ?? Encoding.UTF8
                                }).ConfigureAwait(false);
                        }
                        catch (Exception ex)
                        {
                            context.AddInternalException(ex);

                            if (ex.GetType().Name.Contains("BadHttpRequestException"))
                            {
                                context.Response.StatusCode = 413;
                            }
                            else
                            {
                                context.AddValidationError(context.Configuration?.ValidationErrorConfiguration?.RequestDeserializationError);
                                context.Response.StatusCode = context.Configuration?.ValidationErrorConfiguration?.BodyDeserializationErrorStatusCode ?? 400;
                            }

                            return false;
                        }
                    }
                }

                return true;
            }

            return false;
        }
    }
}
