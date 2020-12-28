namespace DeepSleep.Pipeline
{
    using DeepSleep.Formatting;
    using System;
    using System.Collections.Generic;
    using System.Linq;
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
            var context = contextResolver.GetContext();

            var formatterFactory = context?.RequestServices?.GetService<IFormatStreamReaderWriterFactory>();

            if (await context.ProcessHttpRequestBodyBinding(formatterFactory).ConfigureAwait(false))
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
        /// <param name="formatterFactory">The formatter factory.</param>
        /// <returns></returns>
        internal static async Task<bool> ProcessHttpRequestBodyBinding(this ApiRequestContext context, IFormatStreamReaderWriterFactory formatterFactory)
        {
            if (!context.RequestAborted.IsCancellationRequested)
            {
                if (context.Request.Method?.In(StringComparison.InvariantCultureIgnoreCase, "post", "patch", "put") ?? false)
                {
                    if (!context.Request.ContentLength.HasValue)
                    {
                        if (context.Configuration?.RequireContentLengthOnRequestBodyRequests ?? true)
                        {
                            context.Response.StatusCode = 411;
                            return false;
                        }
                    }

                    if (context.Request.ContentLength > 0 && string.IsNullOrWhiteSpace(context.Request.ContentType))
                    {
                        context.Response.StatusCode = 450;
                        return false;
                    }

                    if (context.Configuration?.MaxRequestLength > 0 && context.Request.ContentLength > 0)
                    {
                        if (context.Request.ContentLength > context.Configuration.MaxRequestLength)
                        {
                            context.Response.StatusCode = 413;
                            return false;
                        }
                    }

                    if (context.Request.ContentLength > 0 && context.Request.InvocationContext?.BodyModelType == null)
                    {
                        if (!(context.Configuration?.AllowRequestBodyWhenNoModelDefined ?? false))
                        {
                            context.Response.StatusCode = 413;
                            return false;
                        }
                    }

                    if (context.Request.InvocationContext?.BodyModelType != null && context.Request.ContentLength > 0 && !string.IsNullOrWhiteSpace(context.Request.ContentType))
                    {
                        IFormatStreamReaderWriter formatter = null;

                        var formatterTypes = context.Configuration?.ReadWriteConfiguration?.ReadableMediaTypes ?? formatterFactory?.GetReadableTypes(null) ?? new List<string>();

                        if (formatterFactory != null)
                        {
                            formatter = await formatterFactory.GetContentTypeFormatter(
                                contentTypeHeader: context.Request.ContentType,
                                formatterType: out var _,
                                readableMediaTypes: context.Configuration?.ReadWriteConfiguration?.ReadableMediaTypes).ConfigureAwait(false);
                        }

                        if (context.Configuration.ReadWriteConfiguration?.ReaderResolver != null)
                        {
                            var overrides = await context.Configuration.ReadWriteConfiguration.ReaderResolver(new ResolvedFormatterArguments(context)).ConfigureAwait(false);

                            if (overrides?.Formatters != null)
                            {
                                formatter = await formatterFactory.GetContentTypeFormatter(
                                    contentTypeHeader: context.Request.ContentType,
                                    formatterType: out var _,
                                    readableFormatters: overrides.Formatters,
                                    readableMediaTypes: context.Configuration?.ReadWriteConfiguration?.ReadableMediaTypes).ConfigureAwait(false);

                                formatterTypes = overrides.Formatters
                                    .Where(f => f != null)
                                    .Where(f => f.SupportsRead)
                                    .Where(f => f.ReadableMediaTypes != null)
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
                            context.Response.Headers.Add(new ApiHeader("X-Allow-Content-Types", string.Join(", ", formatterTypes)));
                            return false;
                        }

                        try
                        {
                            context.Request.InvocationContext.BodyModel = await formatter.ReadType(
                                stream: context.Request.Body, 
                                objType: context.Request.InvocationContext.BodyModelType,
                                options: null).ConfigureAwait(false);
                        }
                        catch (Exception ex)
                        {
                            context.AddException(ex);

                            if (ex.GetType().Name.Contains("BadHttpRequestException"))
                            {
                                context.Response.StatusCode = 413;
                            }
                            else
                            {
                                context.Response.StatusCode = 450;
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
