namespace DeepSleep.Pipeline
{
    using DeepSleep.Formatting;
    using DeepSleep.Auth;
    using DeepSleep.Resources;
    using DeepSleep.Validation;
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.IO;
    using System.Linq;
    using System.Reflection;
    using System.Threading.Tasks;
    using Microsoft.Extensions.Logging;

    /// <summary>
    /// 
    /// </summary>
    public static class ApiRequestContextExtensionMethods
    {
        #region Helper Methods and Fields

        /// <summary>Gets the accepted supported language.</summary>
        /// <param name="supportedLanguages">The supported languages.</param>
        /// <param name="acceptLanguages">The accept languages.</param>
        /// <returns></returns>
        private static string GetAcceptedSupportedLanguage(IEnumerable<string> supportedLanguages, IEnumerable<LanguageValueWithQuality> acceptLanguages)
        {
            if (supportedLanguages == null)
                return string.Empty;
            if (supportedLanguages.Count() == 0)
                return string.Empty;
            if (acceptLanguages == null)
                return string.Empty;
            if (acceptLanguages.Count() == 0)
                return string.Empty;

            var supported = new Dictionary<string, string>();
            foreach (var s in supportedLanguages)
            {
                var prepared = PrepareCultureCodeAsNeutralCulture(s);
                if (!supported.Keys.Any(k => string.Compare(prepared, k, true) == 0))
                {
                    supported.Add(prepared, s);
                }
            }
            
            var accepted = acceptLanguages
                .OrderByDescending(s => s.Quality)
                .Select(s => PrepareCultureCodeAsNeutralCulture(s.Code));

            foreach (var accept in accepted)
            {
                var key = supported.Keys.FirstOrDefault(s => string.Compare(s, accept, true) == 0);
                if (key != null)
                {
                    return supported[key];
                }
            }

            return string.Empty;
        }

        /// <summary>Prepares the culture code as neutral culture.</summary>
        /// <param name="code">The code.</param>
        /// <returns></returns>
        private static string PrepareCultureCodeAsNeutralCulture(string code)
        {
            var cd = code?.Trim();

            if (string.IsNullOrWhiteSpace(cd))
                return cd;

            if (cd.Length == 5)
                return cd;

            if(cd.Length == 2)
                return $"{cd}-{cd.ToUpper()}";

            return cd;
        }

        /// <summary>Gets the route information.</summary>
        /// <param name="context">The context.</param>
        /// <param name="resolver">The resolver.</param>
        /// <param name="routes">The routes.</param>
        /// <returns></returns>
        private static async Task<ApiRoutingItem> GetRouteInfo(this ApiRequestContext context, IUriRouteResolver resolver, IApiRoutingTable routes)
        {
            // -----------------------------------------------------------------
            // We want to trick the routing engine to treat HEAD requests as GET
            // http://tools.ietf.org/html/rfc7231#section-4.3.2
            // -----------------------------------------------------------------

            if (routes != null && resolver != null)
            {
                var routeMatch = new Func<string, Task<ApiRoutingItem>>(async (method) =>
                {
                   var potentialRoutes = routes.GetRoutes()
                       .Where(r => r.EndpointLocation != null)
                       .Where(r => string.Compare(r.HttpMethod, method, true) == 0);

                   RouteMatch result;
                   foreach (var route in potentialRoutes)
                   {
                       result = await resolver.ResolveRoute(route.Template, context.RequestInfo.Path).ConfigureAwait(false);

                       if (result?.IsMatch ?? false)
                       {
                           var newRoute = CloneRoutingItem(route);
                           newRoute.RouteVariables = result.RouteVariables;
                           return newRoute;
                       }
                   }

                   return null;
                });

                if (context.RequestInfo.Method.In(StringComparison.InvariantCultureIgnoreCase, "head"))
                {
                    var routeInfo = await routeMatch("HEAD").ConfigureAwait(false);

                    if (routeInfo == null)
                        routeInfo = await routeMatch("GET").ConfigureAwait(false);

                    return routeInfo;
                }
                else
                {
                    return await routeMatch(context.RequestInfo.Method).ConfigureAwait(false);
                }
            }

            return null;
        }

        /// <summary>Gets the template information.</summary>
        /// <param name="context">The context.</param>
        /// <param name="resolver">The resolver.</param>
        /// <param name="routes">The routes.</param>
        /// <returns></returns>
        private static async Task<ApiRoutingTemplate> GetTemplateInfo(this ApiRequestContext context, IUriRouteResolver resolver, IApiRoutingTable routes)
        {
            RouteMatch result;
            ApiRoutingTemplate template = null;

            if (routes != null && resolver != null)
            {
                foreach (var route in routes.GetRoutes())
                {
                    result = await resolver.ResolveRoute(route.Template, context.RequestInfo.Path).ConfigureAwait(false);

                    if (result?.IsMatch ?? false)
                    {
                        if (template == null)
                        {
                            template = new ApiRoutingTemplate
                            {
                                EndpointLocations = new List<ApiEndpointLocation>(),
                                Template = route.Template,
                            };

                            template.VariablesList.AddRange(route.VariablesList);
                        }

                        template.EndpointLocations.Add(new ApiEndpointLocation
                        {
                            Controller = route.EndpointLocation.Controller,
                            Endpoint = route.EndpointLocation.Endpoint,
                            HttpMethod = route.HttpMethod,
                        });
                    }
                }
            }

            return template;
        }

        /// <summary>Clones the routing item.</summary>
        /// <param name="item">The item.</param>
        /// <returns></returns>
        private static ApiRoutingItem CloneRoutingItem(ApiRoutingItem item)
        {
            var newitem = new ApiRoutingItem
            {
                EndpointLocation = new ApiEndpointLocation
                {
                    Controller = item.EndpointLocation.Controller,
                    Endpoint = item.EndpointLocation.Endpoint,
                    HttpMethod = item.EndpointLocation?.HttpMethod
                },
                Name = item.Name,
                Template = item.Template,
                Config = item.Config,
                HttpMethod = item.HttpMethod
            };

            newitem.RouteVariables = new Dictionary<string, string>();
            newitem.VariablesList.AddRange(item.VariablesList);
            return newitem;
        }

        /// <summary>Handles the exception.</summary>
        /// <param name="context">The context.</param>
        /// <param name="exception">The exception.</param>
        /// <param name="responseMessageConverter">The response message converter.</param>
        /// <returns></returns>
        private static int HandleException(this ApiRequestContext context, Exception exception, IApiResponseMessageConverter responseMessageConverter)
        {
            int code;

            if (exception is ApiNotImplementedException)
            {
                code = 501;

                if (context != null)
                {
                    context.ProcessingInfo.ExtendedMessages.Add(responseMessageConverter.Convert(UnhandledExceptionErrors.NotImplemented));
                    context.AddException(exception);
                }
            }
            else
            {
                code = 500;

                if (context != null)
                {
                    context.ProcessingInfo.ExtendedMessages.Add(responseMessageConverter.Convert(UnhandledExceptionErrors.UnhandledException));
                    context.AddException(exception);
                }
            }

            return code;
        }

        /// <summary>Gets the acceptable formatter.</summary>
        /// <param name="factory">The factory.</param>
        /// <param name="mediaHeader">The media header.</param>
        /// <param name="formatterType">Type of the formatter.</param>
        /// <returns></returns>
        private static Task<IFormatStreamReaderWriter> GetAcceptableFormatter(this IFormatStreamReaderWriterFactory factory, MediaHeaderValueWithQualityString mediaHeader, out string formatterType)
        {
            formatterType = string.Empty;
            IFormatStreamReaderWriter formatter = null;

            foreach (var mediaValue in mediaHeader.Values.Where(m => m.Quality > 0))
            {
                formatter = factory.Get($"{mediaValue.Type}/{mediaValue.SubType}", mediaValue.ParameterString(), out formatterType);
                if (formatter != null)
                    break;
            }

            if (formatter == null)
            {
                formatter = factory.Default(out formatterType);
            }

            var source = new TaskCompletionSource<IFormatStreamReaderWriter>();
            source.SetResult(formatter);
            return source.Task; ;
        }

        /// <summary>Gets the media type formatter.</summary>
        /// <param name="factory">The factory.</param>
        /// <param name="mediaHeader">The media header.</param>
        /// <param name="formatterType">Type of the formatter.</param>
        /// <returns></returns>
        private static Task<IFormatStreamReaderWriter> GetMediaTypeFormatter(this IFormatStreamReaderWriterFactory factory, MediaHeaderValueWithParameters mediaHeader, out string formatterType)
        {
            formatterType = string.Empty;
            IFormatStreamReaderWriter formatter = null;
            var mediaValue = mediaHeader.MediaValue;

            formatter = factory.Get($"{mediaValue.Type}/{mediaValue.SubType}", mediaValue.ParameterString(), out formatterType);

            var source = new TaskCompletionSource<IFormatStreamReaderWriter>();
            source.SetResult(formatter);
            return source.Task; ;
        }

        /// <summary>Converts the value.</summary>
        /// <param name="value">The value.</param>
        /// <param name="convertTo">The convert to.</param>
        /// <returns></returns>
        private static object ConvertValue(string value, Type convertTo)
        {
            if (convertTo == typeof(string))
                return value;

            if (convertTo.IsNullable())
            {
                var nullableType = Nullable.GetUnderlyingType(convertTo);
                return (value == null) 
                    ? null 
                    : Convert.ChangeType(value, nullableType);
            }

            if (value == null)
                return convertTo.GetDefaultValue();

            return Convert.ChangeType(value, convertTo);
        }

        #endregion

        /// <summary>Processes the HTTP conformance.</summary>
        /// <param name="context">The context.</param>
        /// <returns></returns>
        public static Task<bool> ProcessHttpConformance(this ApiRequestContext context)
        {
            if (!context.RequestAborted.IsCancellationRequested)
            {
                var validHttpVersions = new[] { "http/1.1", "http/1.2", "http/2", "http/2.0", "http/2.1" };

                // Only supportting http 1.1 and http 2.0
                if (!validHttpVersions.Contains(context?.RequestInfo?.Protocol?.ToLowerInvariant()) )
                {
                    context.ResponseInfo.ResponseObject = new ApiResponse
                    {
                        StatusCode = 505
                    };

                    return Task.FromResult(false);
                }

                return Task.FromResult(true);
            }

            return Task.FromResult(false);
        }

        /// <summary>Processes the HTTP request accept.</summary>
        /// <param name="context">The context.</param>
        /// <param name="formatterFactory">The formatter factory.</param>
        /// <returns></returns>
        public static async Task<bool> ProcessHttpRequestAccept(this ApiRequestContext context, IFormatStreamReaderWriterFactory formatterFactory)
        {
            if (!context.RequestAborted.IsCancellationRequested)
            {
                if (context.RequestInfo != null)
                {
                    var accept = !string.IsNullOrWhiteSpace(context.RequestInfo.Accept)
                        ? context.RequestInfo.Accept
                        : new MediaHeaderValueWithQualityString("*/*");

                    IFormatStreamReaderWriter formatter = null;

                    if (formatterFactory != null)
                    {
                        formatter = await formatterFactory.GetAcceptableFormatter(accept, out var _).ConfigureAwait(false);
                    }

                    if (formatter == null)
                    {
                        string acceptable = (formatterFactory != null)
                            ? string.Join(", ", formatterFactory.GetTypes())
                            : string.Empty;

                        context.ResponseInfo.AddHeader("X-Allow-Accept", acceptable);
                        context.ResponseInfo.ResponseObject = new ApiResponse
                        {
                            StatusCode = 406
                        };
                        return false;
                    }
                }

                return true;
            }

            return false;
        }

        /// <summary>Processes the HTTP request authentication.</summary>
        /// <param name="context">The context.</param>
        /// <param name="authFactory">The authentication factory.</param>
        /// <param name="responseMessageConverter">The response message converter.</param>
        /// <returns></returns>
        /// <exception cref="Exception">No auth factory established for authenticated route
        /// or
        /// No auth providers established for authenticated route</exception>
        public static async Task<bool> ProcessHttpRequestAuthentication(this ApiRequestContext context, IAuthenticationFactory authFactory, IApiResponseMessageConverter responseMessageConverter)
        {
            if (!context.RequestAborted.IsCancellationRequested)
            {
                if (!(context.RouteInfo?.RoutingItem?.Config?.AllowAnonymous ?? false))
                {
                    if (authFactory == null)
                    {
                        throw new Exception("No auth factory established for authenticated route");
                    }

                    var authProvider = authFactory?.GetProvider(context.RequestInfo?.ClientAuthenticationInfo?.AuthScheme);
                    var result = (authProvider != null)
                        ? await authProvider.Authenticate(context, responseMessageConverter).ConfigureAwait(false)
                        : null;

                    if (result == null || !result.IsAuthenticated)
                    {
                        if (authFactory.FirstOrDefault() == null)
                        {
                            throw new Exception("No auth providers established for authenticated route");
                        }

                        var challenges = new List<string>();
                        foreach (var p in authFactory.GetProviders())
                        {
                            if (!challenges.Contains($"{p.AuthScheme} realm=\"{p.Realm}\""))
                            {
                                challenges.Add($"{p.AuthScheme} realm=\"{p.Realm}\"");
                            }
                        };


                        challenges.ForEach(c => context.ResponseInfo.AddHeader("WWW-Authenticate", c));
                        context.ResponseInfo.ResponseObject = new ApiResponse
                        {
                            StatusCode = 401
                        };
                        return false;
                    }
                }

                return true;
            }

            return false;
        }

        /// <summary>Processes the HTTP request authorization.</summary>
        /// <param name="context">The context.</param>
        /// <returns></returns>
        public static Task<bool> ProcessHttpRequestAuthorization(this ApiRequestContext context)
        {
            TaskCompletionSource<bool> source = new TaskCompletionSource<bool>();

            if (!context.RequestAborted.IsCancellationRequested)
            {
                source.SetResult(true);
                return source.Task;
            }

            source.SetResult(false);
            return source.Task;
        }

        /// <summary>Processes the HTTP request body binding.</summary>
        /// <param name="context">The context.</param>
        /// <param name="formatterFactory">The formatter factory.</param>
        /// <param name="serviceProvider">The service provider.</param>
        /// <param name="responseMessageConverter">The response message converter.</param>
        /// <returns></returns>
        public static async Task<bool> ProcessHttpRequestBodyBinding(this ApiRequestContext context, IFormatStreamReaderWriterFactory formatterFactory, IServiceProvider serviceProvider, IApiResponseMessageConverter responseMessageConverter)
        {
            if (!context.RequestAborted.IsCancellationRequested)
            {
                if (context?.RequestInfo?.Method?.In(StringComparison.InvariantCultureIgnoreCase, "post", "patch", "put") ?? false)
                {
                    if (!context.RequestInfo.ContentLength.HasValue)
                    {
                        context.ResponseInfo.ResponseObject = new ApiResponse
                        {
                            StatusCode = 411
                        };
                        return false;
                    }

                    if (context.RequestInfo.ContentLength > 0 && string.IsNullOrWhiteSpace(context.RequestInfo.ContentType))
                    {
                        context.ResponseInfo.ResponseObject = new ApiResponse
                        {
                            StatusCode = 415
                        };
                        return false;
                    }

                    if (context.RequestInfo.ContentLength > 0 && context.RequestInfo.InvocationContext?.BodyModelType == null)
                    {
                        context.ResponseInfo.ResponseObject = new ApiResponse
                        {
                            StatusCode = 413
                        };
                        return false;
                    }

                    if (context.RequestInfo.ContentLength > 0 && !string.IsNullOrWhiteSpace(context.RequestInfo.ContentType))
                    {
                        IFormatStreamReaderWriter formatter = (formatterFactory != null)
                            ? await formatterFactory.GetMediaTypeFormatter(context.RequestInfo.ContentType, out var formatterType).ConfigureAwait(false)
                            : null;

                        if (formatter == null)
                        {
                            context.ResponseInfo.ResponseObject = new ApiResponse
                            {
                                StatusCode = 415
                            };
                            return false;
                        }


                        try
                        {
                            context.RequestInfo.InvocationContext.BodyModel = await formatter.ReadType(context.RequestInfo.Body, context.RequestInfo.InvocationContext.BodyModelType)
                                .ConfigureAwait(false);
                        }
                        catch (System.Exception)
                        {
                            context.ProcessingInfo.ExtendedMessages.Add(responseMessageConverter.Convert(ValidationErrors.RequestBodyDeserializationError));
                            context.ResponseInfo.ResponseObject = new ApiResponse
                            {
                                StatusCode = 400
                            };
                            return false;
                        }
                    }
                }

                return true;
            }

            return false;
        }

        /// <summary>Processes the HTTP request canceled.</summary>
        /// <param name="context">The context.</param>
        /// <returns></returns>
        public static Task<bool> ProcessHttpRequestCanceled(this ApiRequestContext context)
        {
            TaskCompletionSource<bool> source = new TaskCompletionSource<bool>();

            if (context.RequestAborted.IsCancellationRequested)
            {
                context.ResponseInfo.ResponseObject = new ApiResponse
                {
                    StatusCode = 408
                };
                source.SetResult(false);
                return source.Task;
            }

            source.SetResult(true);
            return source.Task;
        }

        /// <summary>
        /// Processes the HTTP request cross origin resource sharing preflight.
        /// </summary>
        /// <param name="context">The context.</param>
        /// <returns></returns>
        public static Task<bool> ProcessHttpRequestCrossOriginResourceSharingPreflight(this ApiRequestContext context)
        {
            TaskCompletionSource<bool> source = new TaskCompletionSource<bool>();

            if (!context.RequestAborted.IsCancellationRequested)
            {
                if (context?.RequestInfo?.IsCorsPreflightRequest() ?? false)
                {
                    var methods = context.RouteInfo.TemplateInfo.EndpointLocations
                        .Select((r) => r.HttpMethod.ToUpper())
                        .Distinct()
                        .ToArray();

                    context.ResponseInfo.ResponseObject = new ApiResponse
                    {
                        StatusCode = 200
                    };

                    context.ResponseInfo.AddHeader("Access-Control-Allow-Methods", string.Join(", ", methods).Trim());

                    if (!string.IsNullOrWhiteSpace(context.RequestInfo?.CrossOriginRequest?.AccessControlRequestHeaders))
                    {
                        context.ResponseInfo.AddHeader("Access-Control-Allow-Headers", context.RequestInfo.CrossOriginRequest.AccessControlRequestHeaders);
                    }

                    source.SetResult(false);
                    return source.Task;
                }

                source.SetResult(true);
                return source.Task;
            }

            source.SetResult(false);
            return source.Task;
        }

        /// <summary>Processes the HTTP endpoint invocation.</summary>
        /// <param name="context">The context.</param>
        /// <returns></returns>
        public static async Task<bool> ProcessHttpEndpointInvocation(this ApiRequestContext context)
        {
            if (!context.RequestAborted.IsCancellationRequested)
            {
                if (context.RequestInfo?.InvocationContext?.ControllerMethod != null)
                {
                    var parameters = new List<object>();
                    bool addedUriParam = false;
                    bool addedBodyParam = false;

                    // -----------------------------------------------------------
                    // Build the parameters list to invoke the controller method
                    // This includes the UriModel and BodyModel if they exist.
                    // If any other parameters exists on the controller method
                    // they'll be passed a null value.  A possible enhancement
                    // would be to pull the extra parameters from the DI container
                    // -----------------------------------------------------------
                    foreach (var param in context.RequestInfo.InvocationContext.ControllerMethod.GetParameters())
                    {
                        if (!addedUriParam && context.RequestInfo.InvocationContext.UriModel != null && (param.GetCustomAttribute<UriBoundAttribute>() != null || param.Name == "uri"))
                        {
                            parameters.Add(context.RequestInfo.InvocationContext.UriModel);
                            addedUriParam = true;
                        }
                        else if (!addedBodyParam && context.RequestInfo.InvocationContext.BodyModel != null && (param.GetCustomAttribute<BodyBoundAttribute>() != null || param.Name == "body"))
                        {
                            parameters.Add(context.RequestInfo.InvocationContext.BodyModel);
                            addedBodyParam = true;
                        }
                        else
                        {
                            parameters.Add(null);
                        }
                    }

                    // -----------------------------------------------------
                    // Invoke the controller method with the parameters list
                    // -----------------------------------------------------
                    var endpointResponse = context.RequestInfo.InvocationContext.ControllerMethod.Invoke(
                        context.RequestInfo.InvocationContext.Controller,
                        parameters.ToArray());

                    // -----------------------------------------------------
                    // If the response is awaitable then handle
                    // the await on the result
                    // -----------------------------------------------------
                    if (endpointResponse as Task != null)
                    {
                        await ((Task)endpointResponse).ConfigureAwait(false);
                        var resultProperty = endpointResponse.GetType().GetProperty("Result");
                        var response = resultProperty.GetValue(endpointResponse);

                        if (response != null && response.GetType().FullName != "System.Threading.Tasks.VoidTaskResult")
                        {
                            endpointResponse = response;
                        }
                        else
                        {
                            endpointResponse = null;
                        }
                    }

                    // ---------------------------------------------------------------------------
                    // The api context framework uses a custom response object to handle wrting 
                    // to the response stream as well as containing any overrides to aspects of 
                    // the response. If the response is not the custom type then build the custom
                    // type and assign the original response object to the custom response
                    // ---------------------------------------------------------------------------
                    if (endpointResponse as ApiResponse == null)
                    {
                        endpointResponse = new ApiResponse
                        {
                            Body = endpointResponse
                        };
                    }
                    else
                    {
                        var rs = endpointResponse as ApiResponse;
                        if (rs.Headers != null)
                        {
                            rs.Headers.ForEach(h =>
                            {
                                context.ResponseInfo.AddHeader(h.Name, h.Value);
                            });
                        }
                    }

                    

                    context.ResponseInfo.ResponseObject = endpointResponse as ApiResponse;
                }

                return true;
            }

            return false;
        }

        /// <summary>Processes the HTTP endpoint validation.</summary>
        /// <param name="context">The context.</param>
        /// <param name="validationProvider">The validation provider.</param>
        /// <param name="serviceProvider">The service provider.</param>
        /// <param name="responseMessageConverter">The response message converter.</param>
        /// <returns></returns>
        public static async Task<bool> ProcessHttpEndpointValidation(this ApiRequestContext context, IApiValidationProvider validationProvider, IServiceProvider serviceProvider, IApiResponseMessageConverter responseMessageConverter)
        {
            if (!context.RequestAborted.IsCancellationRequested)
            {
                context.ProcessingInfo.Validation.State = ApiValidationState.Validating;


                if (validationProvider != null)
                {
                    var invokers = validationProvider.GetInvokers();


                    foreach (var validationInvoker in invokers)
                    {
                        if (context.RequestInfo.InvocationContext.UriModel != null)
                        {
                            var objectUriValidationResult = await validationInvoker.InvokeObjectValidation(context.RequestInfo.InvocationContext.UriModel, context, serviceProvider, responseMessageConverter).ConfigureAwait(false);
                            if (!objectUriValidationResult)
                            {
                                context.ProcessingInfo.Validation.State = ApiValidationState.Failed;
                            }
                        }
                    }


                    foreach (var validationInvoker in invokers)
                    {
                        if (context.RequestInfo.InvocationContext.BodyModel != null)
                        {
                            var objectBodyValidationResult = await validationInvoker.InvokeObjectValidation(context.RequestInfo.InvocationContext.BodyModel, context, serviceProvider, responseMessageConverter).ConfigureAwait(false);
                            if (!objectBodyValidationResult)
                            {
                                context.ProcessingInfo.Validation.State = ApiValidationState.Failed;
                            }
                        }
                    }


                    foreach (var validationInvoker in invokers)
                    {
                        if (context.RequestInfo?.InvocationContext?.ControllerMethod != null)
                        {
                            var methodValidationResult = await validationInvoker.InvokeMethodValidation(context.RequestInfo.InvocationContext.ControllerMethod, context, serviceProvider, responseMessageConverter).ConfigureAwait(false);
                            if (!methodValidationResult)
                            {
                                context.ProcessingInfo.Validation.State = ApiValidationState.Failed;
                            }
                        }
                    }
                }


                if (context.ProcessingInfo.Validation.State == ApiValidationState.Failed)
                {
                    bool has400 = context.ProcessingInfo.ExtendedMessages.Exists(m => m != null && m.Code.StartsWith("400"));
                    bool has404 = context.ProcessingInfo.ExtendedMessages.Exists(m => m != null && m.Code.StartsWith("404"));

                    var statusCode = (has404 && !has400)
                        ? 404
                        : 400;

                    context.ResponseInfo.ResponseObject = new ApiResponse
                    {
                        StatusCode = statusCode
                    };
                    return false;
                }

                context.ProcessingInfo.Validation.State = ApiValidationState.Succeeded;
                return true;
            }

            
            return false;
        }

        /// <summary>Processes the HTTP request header validation.</summary>
        /// <param name="context">The context.</param>
        /// <param name="responseMessageConverter">The response message converter.</param>
        /// <returns></returns>
        public static Task<bool> ProcessHttpRequestHeaderValidation(this ApiRequestContext context, IApiResponseMessageConverter responseMessageConverter)
        {
            TaskCompletionSource<bool> source = new TaskCompletionSource<bool>();

            if (!context.RequestAborted.IsCancellationRequested)
            {
                var addedHeaderError = false;

                if (context.RequestInfo.Headers != null)
                {
                    foreach (var header in context.RequestInfo.Headers)
                    {
                        int max = int.MaxValue;
                        if (header.Value.Length > max)
                        {
                            addedHeaderError = true;
                            context.ProcessingInfo.ExtendedMessages.Add(responseMessageConverter.Convert(string.Format(ValidationErrors.HeaderLengthExceeded,
                                header.Name,
                                max.ToString())));
                        }
                    }
                }

                if (addedHeaderError)
                {
                    context.ResponseInfo.ResponseObject = new ApiResponse
                    {
                        StatusCode = 431
                    };
                    source.SetResult(false);
                    return source.Task;
                }

                source.SetResult(true);
                return source.Task;
            }

            source.SetResult(false);
            return source.Task;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="context"></param>
        /// <param name="serviceProvider"></param>
        /// <param name="logger"></param>
        /// <returns></returns>
        public static Task<bool> ProcessHttpEndpointInitialization(this ApiRequestContext context, IServiceProvider serviceProvider, ILogger logger)
        {
            if (!context.RequestAborted.IsCancellationRequested)
            {
                if (context?.RouteInfo?.RoutingItem?.EndpointLocation?.Controller == null)
                {
                    throw new Exception("Routing item's controller type is null");
                }

                if (string.IsNullOrWhiteSpace(context.RouteInfo.RoutingItem.EndpointLocation.Endpoint))
                {
                    throw new Exception("Routing item's endpoint name is null");
                }


                MethodInfo method = context.RouteInfo.RoutingItem.EndpointLocation.GetEndpointMethod();

                object endpointController = null;

                try
                {
                    if (serviceProvider != null)
                    {
                        endpointController = serviceProvider.GetService(context.RouteInfo.RoutingItem.EndpointLocation.Controller);
                    }
                }
                catch (System.Exception ex)
                {
                    if (logger != null)
                    {
                        logger.LogInformation($"Controller {context.RouteInfo.RoutingItem.EndpointLocation.Controller} cound not be activated.");
                        logger.LogInformation($"{ex.ToString()}");
                    }
                }

                try
                {
                    if (endpointController == null)
                    {
                        var constructors = context?.RouteInfo?.RoutingItem?.EndpointLocation?.Controller.GetConstructors();

                        if (constructors.Length == 0 || constructors.FirstOrDefault(c => c.GetParameters().Length == 0) != null)
                        {
                            endpointController = Activator.CreateInstance(context?.RouteInfo?.RoutingItem?.EndpointLocation?.Controller);
                        }

                        var firstConstructor = constructors.First();
                        var constructorParameters = new List<object>();

                        firstConstructor.GetParameters().ToList().ForEach(p => constructorParameters.Add(null));
                        endpointController = Activator.CreateInstance(context?.RouteInfo?.RoutingItem?.EndpointLocation?.Controller, constructorParameters.ToArray());
                    }
                }
                catch (System.Exception)
                {
                    throw new Exception(string.Format("Endpoint controller could not be instantiated.  Ensure that the controller has a parameterless contructor or is registered in the DI container"));
                }


                ParameterInfo uriParameter = context.RouteInfo.RoutingItem.EndpointLocation.GetUriParameter(context.RouteInfo?.RoutingItem?.RouteVariables?.Count ?? 0);
                ParameterInfo bodyParameter = context.RouteInfo.RoutingItem.EndpointLocation.GetBodyParameter(context.RouteInfo?.RoutingItem?.RouteVariables?.Count ?? 0);

                context.RequestInfo.InvocationContext = new ApiInvocationContext
                {
                    Controller = endpointController,
                    ControllerMethod = method,
                    UriModelType = uriParameter?.ParameterType,
                    BodyModelType = bodyParameter?.ParameterType
                };

                return Task.FromResult(true);
            }

            return Task.FromResult(false);
        }

        /// <summary>Processes the HTTP request localization.</summary>
        /// <param name="context">The context.</param>
        /// <returns></returns>
        public static Task<bool> ProcessHttpRequestLocalization(this ApiRequestContext context)
        {
            TaskCompletionSource<bool> source = new TaskCompletionSource<bool>();

            if (!context.RequestAborted.IsCancellationRequested)
            {
                var fallBackLanguage = !string.IsNullOrWhiteSpace(context.RouteInfo?.RoutingItem?.Config?.FallBackLanguage)
                    ? context.RouteInfo.RoutingItem.Config.FallBackLanguage
                    : CultureInfo.CurrentUICulture.Name;

                var supportedLanguages = context.RouteInfo?.RoutingItem?.Config?.SupportedLanguages != null
                    ? context.RouteInfo.RoutingItem.Config.SupportedLanguages
                    : new string[] { };

                var acceptedLanguage = GetAcceptedSupportedLanguage(supportedLanguages, context.RequestInfo?.AcceptLanguage?.Values);

                if (string.IsNullOrWhiteSpace(acceptedLanguage))
                {
                    acceptedLanguage = GetAcceptedSupportedLanguage(supportedLanguages, new LanguageValueWithQuality[] {
                        new LanguageValueWithQuality
                        {
                            Code = fallBackLanguage,
                            Parameters = new List<string>(),
                            Quality = 1.0f
                        }
                    });
                }


                if (string.IsNullOrWhiteSpace(acceptedLanguage) && context.RequestInfo?.AcceptLanguage?.Values != null)
                {
                    var neutralLangs = context.RequestInfo.AcceptLanguage.Values
                        .Where(s => (s.Code?.Trim()?.Length ?? 0) > 1)
                        .Select(s => new LanguageValueWithQuality
                        {
                            Code = s.Code.Trim().Substring(0,2),
                            Parameters = s.Parameters,
                            Quality = s.Quality
                        });

                    acceptedLanguage = GetAcceptedSupportedLanguage(supportedLanguages, neutralLangs);
                }

                if (string.IsNullOrWhiteSpace(acceptedLanguage))
                {
                    acceptedLanguage = fallBackLanguage;
                }

                if (string.IsNullOrWhiteSpace(acceptedLanguage))
                {
                    context.RequestInfo.AcceptCulture = CultureInfo.CurrentUICulture;
                }
                else
                {
                    var culture = new CultureInfo(acceptedLanguage);
                    context.RequestInfo.AcceptCulture = culture;
                }

                source.SetResult(true);
                return source.Task;
            }

            source.SetResult(false);
            return source.Task;
        }

        /// <summary>Processes the HTTP request method.</summary>
        /// <param name="context">The context.</param>
        /// <returns></returns>
        public static Task<bool> ProcessHttpRequestMethod(this ApiRequestContext context)
        {
            TaskCompletionSource<bool> source = new TaskCompletionSource<bool>();

            if (!context.RequestAborted.IsCancellationRequested)
            {
                // Templates exist for thies route
                if ((context.RouteInfo.TemplateInfo?.EndpointLocations?.Count ?? 0) > 0)
                {
                    // A route was not found for the template 
                    if (context.RouteInfo.RoutingItem == null)
                    {
                        var methods = context.RouteInfo.TemplateInfo.EndpointLocations
                            .Select(e => e.HttpMethod.ToUpper())
                            .Distinct()
                            .ToList();

                        if (methods.Contains("GET") && !methods.Contains("HEAD"))
                        {
                            methods.Add("HEAD");
                        }

                        context.ResponseInfo.AddHeader("Allow", string.Join(", ", methods));
                        context.ResponseInfo.ResponseObject = new ApiResponse
                        {
                            StatusCode = 405
                        };
                        source.SetResult(false);
                        return source.Task;
                    }
                }

                source.SetResult(true);
                return source.Task;
            }

            source.SetResult(false);
            return source.Task;
        }

        /// <summary>Processes the HTTP request not found.</summary>
        /// <param name="context">The context.</param>
        /// <returns></returns>
        public static Task<bool> ProcessHttpRequestNotFound(this ApiRequestContext context)
        {
            TaskCompletionSource<bool> source = new TaskCompletionSource<bool>();

            if (!context.RequestAborted.IsCancellationRequested)
            {
                if ((context.RouteInfo?.TemplateInfo?.EndpointLocations?.Count ?? 0) == 0)
                {
                    context.ResponseInfo.ResponseObject = new ApiResponse
                    {
                        StatusCode = 404
                    };
                    source.SetResult(false);
                    return source.Task;
                }

                source.SetResult(true);
                return source.Task;
            }

            source.SetResult(false);
            return source.Task;
        }

        /// <summary>Processes the HTTP request routing.</summary>
        /// <param name="context">The context.</param>
        /// <param name="routes">The routes.</param>
        /// <param name="resolver">The resolver.</param>
        /// <returns></returns>
        public static async Task<bool> ProcessHttpRequestRouting(this ApiRequestContext context, IApiRoutingTable routes, IUriRouteResolver resolver)
        {
            if (!context.RequestAborted.IsCancellationRequested)
            {
                context.RouteInfo.RoutingItem = await context.GetRouteInfo(resolver, routes).ConfigureAwait(false);
                context.RouteInfo.TemplateInfo = await context.GetTemplateInfo(resolver, routes).ConfigureAwait(false);
                return true;
            }

            return false;
        }

        /// <summary>Processes the HTTP request URI binding.</summary>
        /// <param name="context">The context.</param>
        /// <param name="serviceProvider">The service provider.</param>
        /// <param name="responseMessageConverter">The response message converter.</param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public static Task<bool> ProcessHttpRequestUriBinding(this ApiRequestContext context, IServiceProvider serviceProvider, IApiResponseMessageConverter responseMessageConverter)
        {
            TaskCompletionSource<bool> source = new TaskCompletionSource<bool>();

            if (!context.RequestAborted.IsCancellationRequested)
            {
                var addedBindingError = false;

                if (context.RequestInfo.InvocationContext?.UriModelType != null)
                {
                    try
                    {
                        context.RequestInfo.InvocationContext.UriModel = (serviceProvider != null)
                            ? serviceProvider.GetService(context.RequestInfo.InvocationContext.UriModelType)
                            : null;
                    }
                    catch (System.Exception) { }



                    if (context.RequestInfo.InvocationContext.UriModel == null)
                    {
                        try
                        {
                            context.RequestInfo.InvocationContext.UriModel = Activator.CreateInstance(context.RequestInfo.InvocationContext.UriModelType);
                        }
                        catch (System.Exception ex)
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

                                p.SetValue(context.RequestInfo.InvocationContext.UriModel, convertedValue);
                                propertiesSet.Add(p.Name);
                            }
                            catch (System.Exception)
                            {
                                addedBindingError = true;
                                context.ProcessingInfo.ExtendedMessages.Add(responseMessageConverter.Convert(string.Format(ValidationErrors.UriRouteBindingError,
                                    context.RouteInfo?.RoutingItem?.RouteVariables[v],
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
                    source.SetResult(false);
                    return source.Task;
                }

                source.SetResult(true);
                return source.Task;
            }

            source.SetResult(false);
            return source.Task;
        }

        /// <summary>Processes the HTTP request URI validation.</summary>
        /// <param name="context">The context.</param>
        /// <param name="responseMessageConverter">The response message converter.</param>
        /// <returns></returns>
        public static Task<bool> ProcessHttpRequestUriValidation(this ApiRequestContext context, IApiResponseMessageConverter responseMessageConverter)
        {
            var source = new TaskCompletionSource<bool>();

            if (!context.RequestAborted.IsCancellationRequested)
            {
                int max = 2083;

                if (!string.IsNullOrWhiteSpace(context.RequestInfo?.RequestUri))
                {
                    if (context.RequestInfo.RequestUri.Length > max)
                    {
                        context.ProcessingInfo.ExtendedMessages.Add(responseMessageConverter.Convert(string.Format(ValidationErrors.RequestUriLengthExceeded, max.ToString())));

                        context.ResponseInfo.ResponseObject = new ApiResponse
                        {
                            StatusCode = 414
                        };
                        source.SetResult(false);
                        return source.Task;
                    }
                }

                source.SetResult(true);
                return source.Task;
            }

            source.SetResult(false);
            return source.Task;
        }

        /// <summary>Processes the HTTP response body writing.</summary>
        /// <param name="context">The context.</param>
        /// <param name="formatterFactory">The formatter factory.</param>
        /// <returns></returns>
        public static async Task<bool> ProcessHttpResponseBodyWriting(this ApiRequestContext context, IFormatStreamReaderWriterFactory formatterFactory)
        {
            if (!context.RequestAborted.IsCancellationRequested)
            {
                if (context.ResponseInfo.ResponseObject == null)
                    context.ResponseInfo.ResponseObject = new ApiResponse();

                if (context.ResponseInfo.ResponseObject?.Body != null && formatterFactory != null)
                {
                    var accept = !string.IsNullOrWhiteSpace(context.RequestInfo.Accept)
                        ? context.RequestInfo.Accept
                        : new MediaHeaderValueWithQualityString("*/*");

                    var formatterType = string.Empty;
                    var formatter = await formatterFactory.GetAcceptableFormatter(accept, out formatterType).ConfigureAwait(false);

                    if (formatter != null)
                    {
                        if (formatter.SupportsPrettyPrint && context.RequestInfo.PrettyPrint)
                        {
                            context.ResponseInfo.AddHeader("X-PrettyPrint", true.ToString().ToLower());
                        }

                        context.ResponseInfo.ContentType = formatterType;

                        using (var m = new MemoryStream())
                        {
                            var formatterOptions = (context.ProcessingInfo.OverridingFormatOptions != null)
                                ? context.ProcessingInfo.OverridingFormatOptions
                                : new FormatterOptions { PrettyPrint = context.RequestInfo.PrettyPrint };

                            await formatter.WriteType(m, context.ResponseInfo.ResponseObject.Body, formatterOptions).ConfigureAwait(false);

                            context.ResponseInfo.ContentLength = m.Length;
                            context.ResponseInfo.RawResponseObject = m.ToArray();
                        }
                    }

                }
                
                if(!context.RequestInfo.IsCorsPreflightRequest() && context.ResponseInfo.ContentLength == 0 && context.ResponseInfo.RawResponseObject == null) 
                {
                    if (context.ResponseInfo.HasSuccessStatus() && (context.ResponseInfo.ResponseObject?.StatusCode ?? 200) != 202)
                    {
                        context.ResponseInfo.ResponseObject.StatusCode = 204;
                    }
                }
            }

            return true;
        }

        /// <summary>Processes the HTTP response correlation.</summary>
        /// <param name="context">The context.</param>
        /// <returns></returns>
        public static Task<bool> ProcessHttpResponseCorrelation(this ApiRequestContext context)
        {
            TaskCompletionSource<bool> source = new TaskCompletionSource<bool>();

            if (!context.RequestAborted.IsCancellationRequested)
            {
                if (context.RequestInfo.CorrelationId != null)
                {
                    context.ResponseInfo.AddHeader("X-CorrelationId", context.RequestInfo.CorrelationId);
                }
            }

            source.SetResult(true);
            return source.Task;
        }

        /// <summary>Processes the HTTP response deprecated.</summary>
        /// <param name="context">The context.</param>
        /// <returns></returns>
        public static Task<bool> ProcessHttpResponseDeprecated(this ApiRequestContext context)
        {
            TaskCompletionSource<bool> source = new TaskCompletionSource<bool>();

            if (!context.RequestAborted.IsCancellationRequested)
            {
                if (context.RouteInfo?.RoutingItem?.Config?.Deprecated ?? false)
                {
                    context.ResponseInfo.AddHeader("X-Deprecated", InfoMessages.ResourceEndpointDeprecated);
                }
            }

            source.SetResult(true);
            return source.Task;
        }

        /// <summary>Processes the HTTP response cross origin resource sharing.</summary>
        /// <param name="context">The context.</param>
        /// <param name="configResolver">The configuration resolver.</param>
        /// <returns></returns>
        public async static Task<bool> ProcessHttpResponseCrossOriginResourceSharing(this ApiRequestContext context, ICrossOriginConfigResolver configResolver)
        {
            if (!context.RequestAborted.IsCancellationRequested)
            {
                if (!string.IsNullOrWhiteSpace(context.RequestInfo?.CrossOriginRequest?.Origin))
                {
                    context.ResponseInfo.AddHeader("Access-Control-Allow-Origin", context.RequestInfo.CrossOriginRequest.Origin);
                    context.ResponseInfo.AddHeader("Access-Control-Allow-Credentials", "true");

                    if (configResolver != null)
                    {
                        var corsConfig = await configResolver.ResolveConfig().ConfigureAwait(false);

                        if (corsConfig?.ExposeHeaders != null && corsConfig.ExposeHeaders.Count() > 0)
                        {
                            var headers = from h in corsConfig.ExposeHeaders
                                          where h.Trim() != string.Empty
                                          select h.Trim();
                                          
                            context.ResponseInfo.AddHeader("Access-Control-Expose-Headers", string.Join(", ", headers.Distinct()).Trim());
                        }
                    }
                }
            }


            return true;
        }

        /// <summary>Processes the HTTP response caching.</summary>
        /// <param name="context">The context.</param>
        /// <returns></returns>
        public static Task<bool> ProcessHttpResponseCaching(this ApiRequestContext context)
        {
            TaskCompletionSource<bool> source = new TaskCompletionSource<bool>();

            if (!context.RequestAborted.IsCancellationRequested)
            {
                var statusCode = context.ResponseInfo.ResponseObject?.StatusCode;

                if (statusCode >= 200 && statusCode <= 299 && statusCode != 204)
                {
                    if (context?.RequestInfo?.IsCorsPreflightRequest() ?? false)
                    {
                        context.ResponseInfo.AddHeader("Vary", "Origin");
                        context.ResponseInfo.AddHeader("Access-Control-Max-Age", "600");

                        source.SetResult(true);
                        return source.Task;
                    }

                    var method = context.RequestInfo?.Method?.ToLower();

                    if (method.In(StringComparison.InvariantCultureIgnoreCase, "get", "put", "options", "head"))
                    {
                        var directive = context.RouteInfo?.RoutingItem?.Config?.CacheDirective;

                        if (directive != null && directive.Cacheability == HttpCacheType.Cacheable && directive.ExpirationSeconds > 0)
                        {
                            context.ResponseInfo.AddHeader("Cache-Control", $"{directive.CacheLocation.ToString().ToLower()}, max-age={directive.ExpirationSeconds}");
                            context.ResponseInfo.AddHeader("Expires", DateTime.UtcNow.AddSeconds(directive.ExpirationSeconds).ToString("r"));

                            // ADDING VARY HEADERS TO SPECIFY WHAT THE RESPONSE REPRESENTATION WAS GENERATED AGAINST.
                            context.ResponseInfo.AddHeader("Vary", "Accept, Accept-Encoding, Accept-Language");

                            source.SetResult(true);
                            return source.Task;
                        }
                    }
                }

                context.ResponseInfo.AddHeader("Cache-Control", "no-cache, no-store, must-revalidate, max-age=0");
                context.ResponseInfo.AddHeader("Expires", DateTime.UtcNow.AddYears(-1).ToString("r"));
            }

            source.SetResult(true);
            return source.Task;
        }

        /// <summary>Processes the HTTP response messages.</summary>
        /// <param name="context">The context.</param>
        /// <param name="responseMessageProcessorProvider">The response message processor provider.</param>
        /// <returns></returns>
        public static async Task<bool> ProcessHttpResponseMessages(this ApiRequestContext context, IApiResponseMessageProcessorProvider responseMessageProcessorProvider)
        {
            if (!context.RequestAborted.IsCancellationRequested)
            {
                if (context?.ProcessingInfo?.ExtendedMessages != null)
                {
                    context.ProcessingInfo.ExtendedMessages.ClearDuplicates();
                    context.ProcessingInfo.ExtendedMessages.SortMessagesByCode();

                    if (responseMessageProcessorProvider != null)
                    {
                        foreach (var processor in responseMessageProcessorProvider.GetProcessors())
                        {
                            await processor.Process(context).ConfigureAwait(false);
                        }
                    }
                }
            }

            return true;
        }

        /// <summary>Processes the HTTP response unhandled exception.</summary>
        /// <param name="context">The context.</param>
        /// <param name="exception">The exception.</param>
        /// <param name="config">The configuration.</param>
        /// <param name="responseMessageConverter">The response message converter.</param>
        /// <returns></returns>
        public static async Task<bool> ProcessHttpResponseUnhandledException(this ApiRequestContext context, Exception exception, IApiServiceConfiguration config, IApiResponseMessageConverter responseMessageConverter)
        {
            if (exception != null)
            {
                var code = context.HandleException(exception, responseMessageConverter);

                if (config?.ExceptionHandler != null && exception as ApiNotImplementedException == null)
                {
                    try
                    {
                        await config.ExceptionHandler(context, exception).ConfigureAwait(false);
                    }
                    catch (System.Exception) { }
                }
                
                context.ResponseInfo.ResponseObject = new ApiResponse
                {
                    StatusCode = code
                };
            }

            return true;
        }
    }
}
