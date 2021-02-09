namespace DeepSleep.OpenApi
{
    using DeepSleep.Configuration;
    using DeepSleep.Media;
    using DeepSleep.OpenApi.Decorators;
    using DeepSleep.OpenApi.Extensions;
    using DeepSleep.Pipeline;
    using Microsoft.OpenApi;
    using Microsoft.OpenApi.Any;
    using Microsoft.OpenApi.Interfaces;
    using Microsoft.OpenApi.Models;
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Reflection;
    using System.Text.Json.Serialization;
    using System.Threading.Tasks;
    using System.Xml.Linq;

    /// <summary>
    /// 
    /// </summary>
    public class DeepSleepOasGenerator : IDeepSleepOasGenerator
    {
        private readonly IList<XDocument> commentDocs;
        private readonly IServiceProvider serviceProvider;
        private readonly IDeepSleepOasConfigurationProvider configurationProvider;
        private readonly IDeepSleepRequestConfiguration defaultRequestConfiguration;
        private readonly IDeepSleepMediaSerializerFactory formatStreamReaderWriterFactory;
        private readonly IApiRoutingTable routingTable;
        private readonly OpenApiSpecVersion specVersion;

        /// <summary>Initializes a new instance of the <see cref="DeepSleepOasGenerator" /> class.</summary>
        /// <param name="serviceProvider">The service provider.</param>
        /// <param name="specVersion">The spec version.</param>
        /// <exception cref="System.ArgumentNullException">serviceProvider
        /// or
        /// configurationProvider</exception>
        /// <exception cref="ArgumentNullException">configuration</exception>
        public DeepSleepOasGenerator(
            IServiceProvider serviceProvider, 
            OpenApiSpecVersion specVersion = OpenApiSpecVersion.OpenApi3_0)
        {
            this.serviceProvider = serviceProvider ?? throw new ArgumentNullException(nameof(serviceProvider));
            this.configurationProvider = serviceProvider.GetRequiredService<IDeepSleepOasConfigurationProvider>();
            this.defaultRequestConfiguration = serviceProvider.GetService<IDeepSleepRequestConfiguration>();
            this.routingTable = serviceProvider.GetRequiredService<IApiRoutingTable>();
            this.formatStreamReaderWriterFactory = serviceProvider.GetService<IDeepSleepMediaSerializerFactory>();
            this.specVersion = specVersion;
            this.commentDocs = new List<XDocument>();

            if (this.configurationProvider.XmlDocumentationFileNames != null)
            {
                foreach (var file in this.configurationProvider.XmlDocumentationFileNames)
                {
                    try
                    {
                        var directory = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);

                        if (File.Exists($"{directory}/{file}"))
                        {
                            string contents = File.ReadAllText($"{directory}/{file}");
                            this.commentDocs.Add(XDocument.Parse(contents));
                        }
                    }
                    catch { }
                }
            }

            if (this.configurationProvider.NamingPolicy == null || this.configurationProvider.EnumNamingPolicy == null)
            {
                var jsonConfiguration = serviceProvider.GetService<JsonMediaSerializerConfiguration>();

                this.configurationProvider.NamingPolicy = this.configurationProvider.NamingPolicy 
                    ?? jsonConfiguration?.SerializerOptions?.PropertyNamingPolicy 
                    ?? new OasDefaultNamingPolicy();

                this.configurationProvider.EnumNamingPolicy = this.configurationProvider.EnumNamingPolicy
                    ?? OasHelpers.GetEnumNamingPolicy(jsonConfiguration?.SerializerOptions)
                    ?? new OasDefaultNamingPolicy();
            }
        }

        /// <summary>Generates the specified routing table.</summary>
        /// <returns></returns>
        public async Task<OpenApiDocument> Generate()
        {
            var routes = this.routingTable.GetRoutes();

            var document = new OpenApiDocument
            {
                Info = this.configurationProvider.Info,
                Components = new OpenApiComponents(),
                Extensions = new Dictionary<string, IOpenApiExtension>(),
                Paths = new OpenApiPaths(),
                Tags = new List<OpenApiTag>()
            };

            foreach (var route in routes)
            {
                if (route.Name == $"GET_{this.configurationProvider.V2RouteTemplate}")
                {
                    continue;
                }

                if (route.Name == $"GET_{this.configurationProvider.V3RouteTemplate}")
                {
                    continue;
                }

                await this.AddRoute(
                    document: document, 
                    route: route, 
                    routes: routingTable, 
                    defaultRequestConfiguration: defaultRequestConfiguration).ConfigureAwait(false);
            }

            return document;
        }

        /// <summary>Adds the route.</summary>
        /// <param name="document">The document.</param>
        /// <param name="route">The route.</param>
        /// <param name="routes">The routes.</param>
        /// <param name="defaultRequestConfiguration">The default request configuration.</param>
        private async Task AddRoute(
            OpenApiDocument document, 
            ApiRoutingItem route, 
            IApiRoutingTable routes, 
            IDeepSleepRequestConfiguration defaultRequestConfiguration)
        {
            var pathName = route.Template.StartsWith("/")
                ? route.Template
                : $"/{route.Template}";

            var existingPath = document.Paths.FirstOrDefault(p => string.Equals(p.Key, pathName, StringComparison.OrdinalIgnoreCase));

            var template = ReplaceTemplateVars(route.Template);
            var routeMatch = await new ApiRouteResolver().MatchRoute(
                routes: routes,
                method: route.HttpMethod,
                requestPath: template).ConfigureAwait(false);

            var routeConfiguration = ApiRequestRoutingPipelineComponentExtensionMethods.MergeConfigurations(
                serviceProvider: this.serviceProvider,
                defaultConfig: defaultRequestConfiguration,
                endpointConfig: routeMatch.Configuration);

            if (existingPath.Key == null)
            {
                existingPath = new KeyValuePair<string, OpenApiPathItem>(pathName, new OpenApiPathItem());
                document.Paths.Add(existingPath.Key, existingPath.Value);
            }

            var operation = this.AddOperation(
                document: document, 
                pathItem: existingPath.Value, 
                route: route, 
                httpMethod: route.HttpMethod);

            if (operation == null)
            {
                return;
            }

            operation.Deprecated = routeConfiguration.Deprecated ?? false;
            operation.Summary = OasDocHelpers.GetDocumenationSummary(route.Location.MethodInfo, this.commentDocs) ?? string.Empty;
            operation.Description = OasDocHelpers.GetDocumentationDescription(route.Location.MethodInfo, this.commentDocs) ?? string.Empty;

            var successCode = await this.AddOperationResponses(
                document: document, 
                operation: operation, 
                route: route, 
                routeConfiguration: routeConfiguration,
                httpMethod: route.HttpMethod).ConfigureAwait(false);

            operation.Parameters = this.GetRouteParameters(
                document: document,
                route: route);

            var queryStringParameters = this.GetQueryStringParameters(
                document: document,
                route: route);

            if (queryStringParameters?.Any() ?? false)
            {
                foreach (var queryParameter in queryStringParameters)
                {
                    operation.Parameters.Add(queryParameter);
                }
            }

            var headerParameters = this.GetHeaderParameters(
                document: document,
                routeConfiguration: routeConfiguration,
                route: route);

            if (headerParameters?.Any() ?? false)
            {
                foreach (var headerParameter in headerParameters)
                {
                    operation.Parameters.Add(headerParameter);
                }
            }

            if (route.HttpMethod.ToLower() == "put" || route.HttpMethod.ToLower() == "post" || route.HttpMethod.ToLower() == "patch")
            {
                var bodyParameter = route.Location.GetBodyParameterInfo();

                if (bodyParameter != null)
                {
                    operation.RequestBody = await AddRequestBody(
                        document: document,
                        type: bodyParameter.ParameterType,
                        method: route.Location.MethodInfo,
                        memberName: bodyParameter.Name,
                        routeConfiguration: routeConfiguration,
                        useReferences: false).ConfigureAwait(false);
                }
            }

            if (string.Equals(route.HttpMethod, "get", StringComparison.OrdinalIgnoreCase))
            {
                var enableHeadForGetRequests = routeConfiguration.EnableHeadForGetRequests ?? true;

                if (enableHeadForGetRequests)
                {
                    var definedHeadRoute = routes.GetRoutes()
                        .Where(r => r.HttpMethod.Equals("head", StringComparison.OrdinalIgnoreCase))
                        .Where(r => r.Template.Equals(route.Template, StringComparison.OrdinalIgnoreCase))
                        .FirstOrDefault();

                    if (definedHeadRoute == null)
                    {
                        var headOperation = this.AddOperation(
                            document: document,
                            pathItem: existingPath.Value, 
                            route: route, 
                            httpMethod: "head",
                            isAutoHeadOperation: true);

                        if (headOperation != null)
                        {
                            await this.AddOperationResponses(
                                document: document,
                                operation: headOperation,
                                route: route,
                                httpMethod: "HEAD",
                                routeConfiguration: routeConfiguration,
                                overridingStatusCode: successCode).ConfigureAwait(false);


                            headOperation.Parameters = this.GetRouteParameters(
                                document: document,
                                route: route);

                            var headQueryStringParameters = this.GetQueryStringParameters(
                                document: document,
                                route: route);

                            if (headQueryStringParameters?.Any() ?? false)
                            {
                                foreach (var queryParameter in headQueryStringParameters)
                                {
                                    headOperation.Parameters.Add(queryParameter);
                                }
                            }

                            var headHeaderParameters = this.GetHeaderParameters(
                                document: document,
                                routeConfiguration: routeConfiguration,
                                route: route);

                            if (headHeaderParameters?.Any() ?? false)
                            {
                                foreach (var headerParameter in headHeaderParameters)
                                {
                                    headOperation.Parameters.Add(headerParameter);
                                }
                            }
                        }
                    }
                }
            }
        }

        /// <summary>Gets the route parameters.</summary>
        /// <param name="document">The document.</param>
        /// <param name="route">The route.</param>
        /// <returns></returns>
        private List<OpenApiParameter> GetRouteParameters(
            OpenApiDocument document,
            ApiRoutingItem route)
        {
            var parameters = new List<OpenApiParameter>();

            if (!string.IsNullOrWhiteSpace(route?.Template))
            {
                var template = new ApiRoutingTemplate(route.Template);

                if (template.Variables.Count > 0)
                {
                    foreach (var routeVar in template.Variables)
                    {
                        var schema = this.GetRouteVariableSchema(
                            document: document,
                            routeVar: routeVar,
                            location: route.Location);

                        var parameter = new OpenApiParameter
                        {
                            Description = schema?.Description,
                            AllowEmptyValue = false,
                            Name = this.configurationProvider.NamingPolicy.ConvertName(routeVar),
                            Required = true,
                            In = ParameterLocation.Path,
                            Schema = schema
                        };

                        parameter.Description = OasDocHelpers.GetParameterDescription(
                            methodInfo: route.Location.MethodInfo,
                            parameter: route.Location.MethodInfo.GetParameters().FirstOrDefault(p => p.Name == routeVar),
                            commentDocs: this.commentDocs) ?? parameter.Description;

                        parameters.Add(parameter);
                    }
                }
            }

            return parameters;
        }

        /// <summary>Gets the route variable schema.</summary>
        /// <param name="document">The document.</param>
        /// <param name="routeVar">The route variable.</param>
        /// <param name="location">The location.</param>
        /// <returns></returns>
        private OpenApiSchema GetRouteVariableSchema(
            OpenApiDocument document,
            string routeVar, 
            ApiEndpointLocation location)
        {
            // For some reason autorest fails when enum route parameters are using schema references
            // but only for v2 serialiation.
            var useReferences = specVersion != OpenApiSpecVersion.OpenApi2_0;

            if (location.SimpleParameters != null)
            {
                var simpleParameter = location.SimpleParameters.FirstOrDefault(p => string.Compare($"{p.Name}", routeVar, true) == 0);

                if (simpleParameter != null)
                {
                    var description = OasDocHelpers.GetParameterDescription(
                        methodInfo: location.MethodInfo, 
                        parameter: simpleParameter, 
                        commentDocs: this.commentDocs);
 
                    var schema = this.AddSchema(
                        document: document,
                        containingMember: location.MethodInfo,
                        type: simpleParameter.ParameterType,
                        memberName: simpleParameter.Name,
                        useReferences: useReferences);

                    schema.Description = description;
                    return schema;
                }
            }

            var uriParameter = location.GetUriParameterInfo();

            if (uriParameter != null)
            {
                var properties = uriParameter.ParameterType.GetProperties()
                   .Where(p => p.CanWrite)
                   .ToArray();

                var prop = properties.FirstOrDefault(p => string.Compare($"{p.Name}", routeVar, true) == 0);

                if (prop != null)
                {
                    var schema = this.AddSchema(
                        document: document,
                        containingMember: uriParameter.Member,
                        type: prop.PropertyType,
                        memberName: prop.Name,
                        useReferences: useReferences);

                    return schema;
                }
            }


            return null;
        }

        /// <summary>Adds the operation.</summary>
        /// <param name="document">The document.</param>
        /// <param name="pathItem">The path item.</param>
        /// <param name="route">The route.</param>
        /// <param name="httpMethod">The HTTP method.</param>
        /// <param name="isAutoHeadOperation">if set to <c>true</c> [is automatic head operation].</param>
        /// <returns></returns>
        private OpenApiOperation AddOperation(
            OpenApiDocument document,
            OpenApiPathItem pathItem, 
            ApiRoutingItem route, 
            string httpMethod,
            bool isAutoHeadOperation = false)
        {
            var method = httpMethod.ToLowerInvariant();

            Func<OperationType, OpenApiOperation> handler = (opType) =>
            {
                if (pathItem.Operations.Any(p => p.Key == opType))
                {
                    throw new InvalidOperationException($"{opType.ToString().ToUpperInvariant()} operation is already defined on the path item.");
                }

                var operationId = OasHelpers.GetOperationId(
                    httpMethod: opType.ToString().ToUpperInvariant(),
                    route: route,
                    isAutoHeadOperation: isAutoHeadOperation);

                var operation = new OpenApiOperation
                {
                    OperationId = operationId,
                    Tags = this.GetOperationTags(
                        document: document,
                        route: route,
                        useReferences: true),
                };

                pathItem.AddOperation(opType, operation);
                return operation;
            };


            if (method == "delete")
            {
                return handler(OperationType.Delete);
            }

            if (method == "get")
            {
                return handler(OperationType.Get);
            }


            if (method == "head")
            {
                return handler(OperationType.Head);
            }

            if (method == "options")
            {
                return handler(OperationType.Options);
            }

            if (method == "patch")
            {
                return handler(OperationType.Patch);
            }

            if (method == "post")
            {
                return handler(OperationType.Post);
            }

            if (method == "put")
            {
                return handler(OperationType.Put);
            }

            if (method == "trace")
            {
                return handler(OperationType.Trace);
            }

            return null;
        }

        /// <summary>Adds the operation responses.</summary>
        /// <param name="document">The document.</param>
        /// <param name="operation">The operation.</param>
        /// <param name="route">The route.</param>
        /// <param name="httpMethod">The HTTP method.</param>
        /// <param name="routeConfiguration">The route configuration.</param>
        /// <param name="overridingStatusCode">The overriding status code.</param>
        /// <returns></returns>
        private async Task<string> AddOperationResponses(
            OpenApiDocument document, 
            OpenApiOperation operation, 
            ApiRoutingItem route, 
            string httpMethod,
            IDeepSleepRequestConfiguration routeConfiguration,
            string overridingStatusCode = null)
        {
            var rootReturnTyoe = TypeExtensions.GetRootType(route.Location.GetMethodInfoReturnType());
            var responseTypes = new Dictionary<string, Type>();

            if (rootReturnTyoe == typeof(Task) || string.Equals(httpMethod, "head", StringComparison.OrdinalIgnoreCase))
            {
                rootReturnTyoe = typeof(void);
            }
            else if(rootReturnTyoe == typeof(IApiResponse) || rootReturnTyoe.GetInterface(nameof(IApiResponse)) != null)
            {
                rootReturnTyoe = typeof(object);
            }

            var successStatusCode = overridingStatusCode 
                ?? (rootReturnTyoe == typeof(void) ? "204" : "200");


            var openApiResponseAttributes = route.Location.MethodInfo.GetCustomAttributes(typeof(OasResponseAttribute), false)
                .Cast<OasResponseAttribute>()
                .ToList();


            foreach (var attribute in openApiResponseAttributes)
            {
                if (!responseTypes.Any(r => r.Key == attribute.StatusCode))
                {
                    responseTypes.Add(
                        key: attribute.StatusCode, 
                        value: TypeExtensions.GetRootType(attribute.ResponseType));
                }
            }

            if (!responseTypes.Keys.Any(k => k.StartsWith("2")))
            {
                responseTypes.Add(
                    key: successStatusCode ?? "204",
                    value: rootReturnTyoe);
            }

            if (!responseTypes.Keys.Any(k => k == "default"))
            {
                var validationProvider = routeConfiguration.ApiErrorResponseProvider != null
                    ? routeConfiguration.ApiErrorResponseProvider(this.serviceProvider)
                    : null;

                var validationErrorType = validationProvider?.GetErrorType();
                var validationErrorRootType = TypeExtensions.GetRootType(validationErrorType);

                if (validationErrorType != null)
                {
                    responseTypes.Add(
                        key: "default",
                        value: TypeExtensions.GetRootType(validationErrorType));
                }
            }

            successStatusCode = responseTypes.Keys.FirstOrDefault(k => k.StartsWith("2")) ?? successStatusCode;

            foreach (var responseType in responseTypes)
            {
                if (responseType.Value != typeof(void) && responseType.Value != typeof(object))
                {
                    var typeName = OasHelpers.GetDocumentTypeSchemaName(responseType.Value, this.configurationProvider.PrefixNamesWithNamespace);

                    operation.Responses.Add(responseType.Key, new OpenApiResponse
                    {
                        Description = OasDocHelpers.GetReturnTypeDocumentationSummary(route.Location.MethodInfo, this.commentDocs) 
                            ?? OasDocHelpers.GetDocumentationSummary(responseType.Value, this.commentDocs) 
                            ?? OasHelpers.GetDefaultResponseDescription(responseType.Key),

                        Content = await GetResponseContents(
                            document: document, 
                            responseType: responseType.Value,
                            routeConfiguration: routeConfiguration).ConfigureAwait(false),
                        
                        Headers = GetResponseHeaders(
                            document: document,
                            routeConfiguration: routeConfiguration,
                            route: route)
                    });;
                }
                else
                {
                    operation.Responses.Add(responseType.Key, new OpenApiResponse
                    {
                        Description = OasHelpers.GetDefaultResponseDescription(responseType.Key),

                        Headers = GetResponseHeaders(
                            document: document,
                            routeConfiguration: routeConfiguration,
                            route: route)
                    });
                }
            }

            return !string.IsNullOrWhiteSpace(successStatusCode)
                ? successStatusCode
                : null;
        }

        /// <summary>Gets the response contents.</summary>
        /// <param name="document">The document.</param>
        /// <param name="responseType">Type of the response.</param>
        /// <param name="routeConfiguration">The route configuration.</param>
        /// <returns></returns>
        private async Task<Dictionary<string, OpenApiMediaType>> GetResponseContents(
            OpenApiDocument document, 
            Type responseType,
            IDeepSleepRequestConfiguration routeConfiguration)
        {
            var contents = new Dictionary<string, OpenApiMediaType>();

            var contentTypes = await OasHelpers.GetResponseBodyContentTypes(
                responseBodyType: responseType,
                serviceProvider: this.serviceProvider,
                routeConfiguration: routeConfiguration,
                formatterFactory: this.formatStreamReaderWriterFactory).ConfigureAwait(false);

            foreach (var contentType in contentTypes.Distinct())
            {
                contents.Add(contentType, new OpenApiMediaType
                {
                    Schema = this.AddSchema(
                        document: document, 
                        containingMember: null, 
                        type: responseType,
                        memberName: null,
                        useReferences: true)
                });
            }

            return contents;
        }

        /// <summary>Adds the request body.</summary>
        /// <param name="document">The document.</param>
        /// <param name="type">The type.</param>
        /// <param name="method">The method.</param>
        /// <param name="memberName">Name of the member.</param>
        /// <param name="routeConfiguration">The route configuration.</param>
        /// <param name="useReferences">if set to <c>true</c> [use referencees].</param>
        /// <returns></returns>
        private async Task<OpenApiRequestBody> AddRequestBody(
            OpenApiDocument document, 
            Type type, 
            MethodInfo method, 
            string memberName,
            IDeepSleepRequestConfiguration routeConfiguration,
            bool useReferences = true)
        {
            OpenApiRequestBody requestBody = null;

            if (TypeExtensions.IsArrayType(type))
            {
                requestBody = await AddArrayRequestBody(
                    document: document,
                    method: method,
                    type: type,
                    memberName: memberName,
                    routeConfiguration: routeConfiguration,
                    useReferences: useReferences).ConfigureAwait(false);
            }
            else if (TypeExtensions.IsEnumType(type))
            {
                requestBody = await AddEnumRequestBody(
                    document: document,
                    method: method,
                    memberName: memberName,
                    type: type,
                    routeConfiguration: routeConfiguration,
                    useReferences: useReferences).ConfigureAwait(false);
            }
            else if (TypeExtensions.IsComplexType(type))
            {
                requestBody = await AddComplexTypeRequestBody(
                    document: document,
                    type: type,
                    method: method,
                    memberName: memberName,
                    routeConfiguration: routeConfiguration,
                    useReferences: useReferences).ConfigureAwait(false);
            }
            else
            {
                requestBody = await AddSimpleTypeRequestBody(
                    type: type,
                    routeConfiguration: routeConfiguration,
                    method: method).ConfigureAwait(false);
            }

            if (!useReferences)
            {
                return requestBody;
            }

            var existingRequestBody = document.Components.RequestBodies
                .FirstOrDefault(s => string.Equals(s.Key, OasHelpers.GetDocumentTypeSchemaName(type, this.configurationProvider.PrefixNamesWithNamespace), StringComparison.Ordinal));

            if (existingRequestBody.Key == null)
            {
                return requestBody;
            }

            return new OpenApiRequestBody
            {
                Reference = new OpenApiReference
                {
                    Id = $"{existingRequestBody.Key}",
                    Type = ReferenceType.RequestBody
                }
            };
        }

        /// <summary>Adds the array request body.</summary>
        /// <param name="document">The document.</param>
        /// <param name="method">The method.</param>
        /// <param name="type">The type.</param>
        /// <param name="memberName">Name of the member.</param>
        /// <param name="routeConfiguration">The route configuration.</param>
        /// <param name="useReferences">if set to <c>true</c> [use referencees].</param>
        /// <returns></returns>
        private async Task<OpenApiRequestBody> AddArrayRequestBody(
            OpenApiDocument document,
            MethodInfo method,
            Type type,
            string memberName,
            IDeepSleepRequestConfiguration routeConfiguration,
            bool useReferences = true)
        {
            var arrayType = TypeExtensions.GetRootType(TypeExtensions.GetArrayType(type));
            var contents = new Dictionary<string, OpenApiMediaType>();

            var contentTypes = await OasHelpers.GetRequestBodyContentTypes(
                requestBodyType: type,
                serviceProvider: this.serviceProvider,
                routeConfiguration: routeConfiguration,
                formatterFactory: this.formatStreamReaderWriterFactory,
                specVersion: this.specVersion).ConfigureAwait(false);

            foreach (var contentType in contentTypes.Distinct())
            {
                contents.Add(contentType, new OpenApiMediaType
                {
                    Schema = AddSchema(
                        document: document,
                        containingMember: method,
                        type: type,
                        memberName: memberName,
                        useReferences: useReferences)
                });
            }

            return new OpenApiRequestBody
            {
                Description = OasDocHelpers.GetParameterDescription(
                    methodInfo: method,
                    parameter: method.GetParameters().FirstOrDefault(p => p.Name == memberName),
                    commentDocs: this.commentDocs),

                Content = contents
            };
        }

        /// <summary>Adds the complex type request body.</summary>
        /// <param name="document">The document.</param>
        /// <param name="method">The method.</param>
        /// <param name="type">The type.</param>
        /// <param name="memberName">Name of the member.</param>
        /// <param name="routeConfiguration">The route configuration.</param>
        /// <param name="useReferences">if set to <c>true</c> [use referencees].</param>
        /// <returns></returns>
        private async Task<OpenApiRequestBody> AddComplexTypeRequestBody(
            OpenApiDocument document,
            MethodInfo method,
            Type type,
            string memberName,
            IDeepSleepRequestConfiguration routeConfiguration,
            bool useReferences = true)
        {
            var rootType = TypeExtensions.GetRootType(type);
            var contents = new Dictionary<string, OpenApiMediaType>();

            var contentTypes = await OasHelpers.GetRequestBodyContentTypes(
                requestBodyType: type,
                serviceProvider: this.serviceProvider,
                routeConfiguration: routeConfiguration,
                formatterFactory: this.formatStreamReaderWriterFactory,
                specVersion: this.specVersion).ConfigureAwait(false);

            foreach (var contentType in contentTypes.Distinct())
            {
                contents.Add(contentType, new OpenApiMediaType
                {
                    Schema = this.AddSchema(
                        document: document,
                        containingMember: method,
                        type: type,
                        memberName: memberName,
                        useReferences: true)
                });
            }

            var requestBody = new OpenApiRequestBody
            {
                Description = OasDocHelpers.GetDocumentationSummary(rootType, this.commentDocs) ?? string.Empty,
                Content = contents
            };

            if (!string.IsNullOrWhiteSpace(memberName))
            {
                requestBody.Description = OasDocHelpers.GetParameterDescription(
                    methodInfo: method,
                    parameter: method.GetParameters().FirstOrDefault(p => p.Name == memberName),
                    commentDocs: this.commentDocs);
            }


            if (useReferences)
            {
                var existingRequestBody = document.Components.RequestBodies
                    .FirstOrDefault(s => string.Equals(s.Key, OasHelpers.GetDocumentTypeSchemaName(rootType, this.configurationProvider.PrefixNamesWithNamespace), StringComparison.Ordinal));

                if (existingRequestBody.Value == null)
                {
                    document.Components.RequestBodies.Add(
                        key: OasHelpers.GetDocumentTypeSchemaName(rootType, this.configurationProvider.PrefixNamesWithNamespace),
                        value: requestBody);
                }
            }

            return requestBody;
        }

        /// <summary>Adds the simple type request body.</summary>
        /// <param name="type">The type.</param>
        /// <param name="routeConfiguration">The route configuration.</param>
        /// <param name="method">The methed.</param>
        /// <returns></returns>
        private async Task<OpenApiRequestBody> AddSimpleTypeRequestBody(
            Type type,
            IDeepSleepRequestConfiguration routeConfiguration,
            MethodInfo method)
        {
            var rootType = TypeExtensions.GetRootType(type);
            var openApiType = OasHelpers.GetOpenApiSchemaType(rootType);
            var contents = new Dictionary<string, OpenApiMediaType>();

            var contentTypes = await OasHelpers.GetRequestBodyContentTypes(
                requestBodyType: type,
                serviceProvider: this.serviceProvider,
                routeConfiguration: routeConfiguration,
                formatterFactory: this.formatStreamReaderWriterFactory,
                specVersion: this.specVersion).ConfigureAwait(false);

            foreach (var contentType in contentTypes.Distinct())
            {
                contents.Add(contentType, new OpenApiMediaType
                {
                    Schema = new OpenApiSchema
                    {
                        Description = OasDocHelpers.GetDocumenationSummary(rootType, this.commentDocs) ?? string.Empty,
                        Type = openApiType,
                        Format = OasHelpers.GetOpenApiSchemaFormat(openApiType, rootType),
                        Nullable = TypeExtensions.IsNullableType(type)
                    }
                });
            }

            var requestBody = new OpenApiRequestBody
            {
                Description = OasDocHelpers.GetDocumentationSummary(rootType, this.commentDocs) ?? string.Empty,
                Content = contents
            };

            return requestBody;
        }

        /// <summary>Adds the simple type request body.</summary>
        /// <param name="document">The document.</param>
        /// <param name="method">The methed.</param>
        /// <param name="type">The type.</param>
        /// <param name="memberName">Name of the member.</param>
        /// <param name="routeConfiguration">The route configuration.</param>
        /// <param name="useReferences">if set to <c>true</c> [use referencees].</param>
        /// <returns></returns>
        private async Task<OpenApiRequestBody> AddEnumRequestBody(
            OpenApiDocument document,
            MethodInfo method,
            Type type,
            string memberName,
            IDeepSleepRequestConfiguration routeConfiguration,
            bool useReferences = true)
        {
            var rootType = TypeExtensions.GetRootType(type);
            var contents = new Dictionary<string, OpenApiMediaType>();

            var contentTypes = await OasHelpers.GetRequestBodyContentTypes(
                requestBodyType: type,
                serviceProvider: this.serviceProvider,
                routeConfiguration: routeConfiguration,
                formatterFactory: this.formatStreamReaderWriterFactory,
                specVersion: this.specVersion).ConfigureAwait(false);

            foreach (var contentType in contentTypes.Distinct())
            {
                contents.Add(contentType, new OpenApiMediaType
                {
                    Schema = this.AddEnumSchema(
                        document: document,
                        containingMember: method,
                        memberName: memberName,
                        type: rootType,
                        useReferences: true)
                });
            }

            var requestBody = new OpenApiRequestBody
            {
                Description = OasDocHelpers.GetDocumentationSummary(rootType, this.commentDocs) ?? string.Empty,
                Content = contents
            };

            return requestBody;
        }

        /// <summary>Adds the schema.</summary>
        /// <param name="document">The document.</param>
        /// <param name="containingMember">The containing member.</param>
        /// <param name="type">The type.</param>
        /// <param name="memberName">Name of the member.</param>
        /// <param name="useReferences">if set to <c>true</c> [use referencees].</param>
        /// <returns></returns>
        private OpenApiSchema AddSchema(
            OpenApiDocument document,
            MemberInfo containingMember,
            Type type,
            string memberName,
            bool useReferences = true)
        {
            OpenApiSchema schema = null;

            if (TypeExtensions.IsArrayType(type))
            {
                schema = AddArraySchema(
                    document: document,
                    containingMember: containingMember,
                    memberName: memberName,
                    type: type);
            }
            else if (TypeExtensions.IsEnumType(type))
            {
                schema = AddEnumSchema(
                    document: document,
                    containingMember: containingMember,
                    type: type,
                    memberName: memberName,
                    useReferences: useReferences);
            }
            else if (TypeExtensions.IsComplexType(type))
            {
                schema = AddComplexTypeSchema(
                    document: document,
                    containingMember: containingMember,
                    type: type,
                    memberName: memberName,
                    useReferences: useReferences);
            }
            else
            {
                schema = AddSimpleTypeSchema(
                    document: document,
                    type: type,
                    memberName: memberName,
                    containingMember: containingMember);
            }


            if (!useReferences)
            {
                return schema;
            }

            var existingSchema = document.Components.Schemas
                .FirstOrDefault(s => string.Equals(s.Key, OasHelpers.GetDocumentTypeSchemaName(type, this.configurationProvider.PrefixNamesWithNamespace), StringComparison.Ordinal));

            if (existingSchema.Key == null)
            {
                return schema;
            }

            return new OpenApiSchema
            {
                Reference = new OpenApiReference
                {
                    Id = $"{existingSchema.Key}",
                    Type = ReferenceType.Schema
                }
            };
        }

        /// <summary>Adds the array schema.</summary>
        /// <param name="document">The document.</param>
        /// <param name="containingMember">The containing member.</param>
        /// <param name="type">The type.</param>
        /// <param name="memberName">Name of the member.</param>
        /// <param name="useReferences">if set to <c>true</c> [use referencees].</param>
        /// <returns></returns>
        private OpenApiSchema AddArraySchema(
            OpenApiDocument document, 
            MemberInfo containingMember, 
            Type type,
            string memberName,
            bool useReferences = true)
        {
            var arrayType = TypeExtensions.GetRootType(TypeExtensions.GetArrayType(type));

            var schema = new OpenApiSchema
            {
                Description = OasDocHelpers.GetDocumentationSummary(arrayType, this.commentDocs),
                Type = OasHelpers.GetOpenApiSchemaType(type),
                Items = AddSchema(
                    document: document, 
                    containingMember: containingMember,
                    type: arrayType,
                    memberName: memberName,
                    useReferences: useReferences)
            };

            if (schema.Items != null)
            {
                schema.Items.Description = null;
            }

            return schema;
        }

        /// <summary>Adds the complex type schema.</summary>
        /// <param name="document">The document.</param>
        /// <param name="type">The type.</param>
        /// <param name="memberName">Name of the member.</param>
        /// <param name="containingMember">The containing member.</param>
        /// <param name="useReferences">if set to <c>true</c> [use referencees].</param>
        /// <returns></returns>
        private OpenApiSchema AddComplexTypeSchema(
            OpenApiDocument document, 
            Type type,
            string memberName,
            MemberInfo containingMember,
            bool useReferences = true)
        {
            var rootType = TypeExtensions.GetRootType(type);

            var schema = new OpenApiSchema
            {
                Description = OasDocHelpers.GetDocumentationSummary(rootType, this.commentDocs) ?? string.Empty,
                Type = OasHelpers.GetOpenApiSchemaType(rootType),
                Properties = new Dictionary<string, OpenApiSchema>(),
                AdditionalPropertiesAllowed = false
            };

            foreach (var prop in rootType.GetProperties())
            {
                schema.Properties.Add(
                    key: this.configurationProvider.NamingPolicy.ConvertName(prop.Name), 
                    value: this.AddSchema(
                        document: document,
                        containingMember: rootType,
                        type: prop.PropertyType,
                        memberName: prop.Name,
                        useReferences: true));
            }


            if (useReferences)
            {
                var existingSchema = document.Components.Schemas
                   .FirstOrDefault(s => string.Equals(s.Key, OasHelpers.GetDocumentTypeSchemaName(rootType, this.configurationProvider.PrefixNamesWithNamespace), StringComparison.Ordinal));

                if (existingSchema.Value == null)
                {
                    document.Components.Schemas.Add(
                        key: OasHelpers.GetDocumentTypeSchemaName(rootType, this.configurationProvider.PrefixNamesWithNamespace), 
                        value: schema);
                }
            }

            return schema;
        }

        /// <summary>Adds the enum schema.</summary>
        /// <param name="document">The document.</param>
        /// <param name="containingMember">The containing member.</param>
        /// <param name="type">The type.</param>
        /// <param name="memberName">Name of the member.</param>
        /// <param name="useReferences">if set to <c>true</c> [use referencees].</param>
        /// <returns></returns>
        private OpenApiSchema AddEnumSchema(
            OpenApiDocument document, 
            MemberInfo containingMember, 
            Type type,
            string memberName,
            bool useReferences = true)
        {
            var rootType = TypeExtensions.GetRootType(type);
            var openApiType = OasHelpers.GetOpenApiSchemaType(rootType, this.configurationProvider.EnumModeling);
            var items = Enum.GetValues(rootType);
            var enumItems = new List<IOpenApiAny>();

            foreach (var item in items)
            {
                if (this.configurationProvider.EnumModeling != OasEnumModeling.AsIntegerEnum)
                {
                    enumItems.Add(new OpenApiString(this.configurationProvider.EnumNamingPolicy.ConvertName(item.ToString())));
                }
                else
                {
                    enumItems.Add(new OpenApiInteger((int)item));
                }
            }

            var schema = new OpenApiSchema
            {
                Description = OasDocHelpers.GetDocumentationSummary(rootType, this.commentDocs) ?? string.Empty,
                Type = openApiType,
                Enum = enumItems,
                Format = OasHelpers.GetOpenApiSchemaFormat(openApiType, rootType)
            };

            if (this.configurationProvider.EnumModeling != OasEnumModeling.AsIntegerEnum)
            {
                var msEnumDefinition = new OasXMsEnumExtension(
                    enumType: rootType,
                    commentDocs: this.commentDocs,
                    enumModeling: this.configurationProvider.EnumModeling,
                    namingPolicy: this.configurationProvider.EnumNamingPolicy);

                schema.Extensions = new Dictionary<string, IOpenApiExtension>
                {
                    { "x-ms-enum", msEnumDefinition }
                };
            }


            if (useReferences)
            {
                var existingSchema = document.Components.Schemas
                   .FirstOrDefault(s => string.Equals(s.Key, OasHelpers.GetDocumentTypeSchemaName(rootType, this.configurationProvider.PrefixNamesWithNamespace), StringComparison.Ordinal));

                if (existingSchema.Value == null)
                {
                    document.Components.Schemas.Add(
                        key: OasHelpers.GetDocumentTypeSchemaName(rootType, this.configurationProvider.PrefixNamesWithNamespace),
                        value: schema);
                }
            }

            return schema;
        }

        /// <summary>Adds the simple type schema.</summary>
        /// <param name="document">The document.</param>
        /// <param name="type">The type.</param>
        /// <param name="memberName">Name of the member.</param>
        /// <param name="containingMember">The containing member.</param>
        /// <returns></returns>
        private OpenApiSchema AddSimpleTypeSchema(
            OpenApiDocument document,
            Type type,
            string memberName,
            MemberInfo containingMember)
        {
            var rootType = TypeExtensions.GetRootType(type);
            var openApiType = OasHelpers.GetOpenApiSchemaType(rootType);

            var schema = new OpenApiSchema
            {
                Description = OasDocHelpers.GetDocumentationSummary(rootType, this.commentDocs),
                Type = openApiType,
                Format = OasHelpers.GetOpenApiSchemaFormat(openApiType, rootType),
                Nullable = TypeExtensions.IsNullableType(type)
            };

            if (containingMember is PropertyInfo prop)
            {
                schema.Description = OasDocHelpers.GetDocumenationSummary(prop, this.commentDocs) ?? schema.Description;

                if (prop.CanRead && !prop.CanRead)
                {
                    schema.ReadOnly = true;
                }

                if (prop.CanWrite && !prop.CanRead)
                {
                    schema.WriteOnly = true;
                }
            }
            else if (containingMember is MethodInfo method)
            {
                schema.Description = OasDocHelpers.GetParameterDescription(
                    methodInfo: method,
                    parameter: method.GetParameters().FirstOrDefault(p => p.Name == memberName),
                    commentDocs: this.commentDocs) ?? schema.Description;
            }
            else if (containingMember is Type containingType && !string.IsNullOrWhiteSpace(memberName))
            {
                var containingProp = containingType.GetProperty(memberName);

                schema.Description = OasDocHelpers.GetDocumenationSummary(
                    property: containingProp,
                    commentDocs: this.commentDocs) ?? schema.Description;

                if (containingProp.CanRead && !containingProp.CanRead)
                {
                    schema.ReadOnly = true;
                }

                if (containingProp.CanWrite && !containingProp.CanRead)
                {
                    schema.WriteOnly = true;
                }
            }

            return schema;
        }

        /// <summary>Gets the query string parameters.</summary>
        /// <param name="document">The document.</param>
        /// <param name="route">The route.</param>
        /// <returns></returns>
        private List<OpenApiParameter> GetQueryStringParameters(
            OpenApiDocument document, 
            ApiRoutingItem route)
        {
            var parameters = new List<OpenApiParameter>();
            var uriParameter = route.Location.GetUriParameterInfo();
            var template = new ApiRoutingTemplate(route.Template);

            var preparedRouteVars = template.Variables
                .Select(v => v.ToLower())
                .ToList();

            var properties = new PropertyInfo[] { };

            if (uriParameter != null)
            {
                properties = uriParameter.ParameterType.GetProperties()
                   .Where(p => p.CanWrite)
                   .Where(p => !preparedRouteVars.Contains(p.Name.ToLower()))
                   .ToArray();

                foreach (var prop in properties)
                {
                    var parameter = new OpenApiParameter
                    {
                        Description = OasDocHelpers.GetDocumenationSummary(prop, this.commentDocs) ?? string.Empty,
                        Name = this.configurationProvider.NamingPolicy.ConvertName(prop.Name),
                        Required = false,
                        In = ParameterLocation.Query,
                        Schema = this.AddSchema(
                            document: document,
                            containingMember: prop,
                            type: prop.PropertyType,
                            memberName: prop.Name,
                            useReferences: false)
                    };

                    if (TypeExtensions.IsArrayType(prop.PropertyType))
                    {
                        parameter.Style = ParameterStyle.Form;
                        parameter.Explode = true;
                    }

                    parameters.Add(parameter);
                }
            }

            var simpleParameters = route.Location.GetSimpleParametersInfo()
                .Where(p => !preparedRouteVars.Contains(p.Name.ToLower()))
                .Where(p => properties.Any(prop => prop.Name.ToLower() == p.Name.ToLower()) == false)
                .ToList();

            foreach (var param in simpleParameters)
            {
                var parameter = new OpenApiParameter
                {
                    Description = OasDocHelpers.GetParameterDescription(route.Location.MethodInfo, param, this.commentDocs) ?? string.Empty,
                    Name = this.configurationProvider.NamingPolicy.ConvertName(param.Name),
                    Required = false,
                    In = ParameterLocation.Query,
                    Schema = this.AddSchema(
                        document: document,
                        containingMember: param.Member,
                        type: param.ParameterType,
                        memberName: param.Name,
                        useReferences: false)
                };

                if (TypeExtensions.IsArrayType(param.ParameterType))
                {
                    parameter.Style = ParameterStyle.Form;
                    parameter.Explode = false;

                    if (this.specVersion == OpenApiSpecVersion.OpenApi2_0)
                    {
                        parameter.Extensions.Add("collectionFormat", new Microsoft.OpenApi.Any.OpenApiString("csv"));
                    }
                }

                parameters.Add(parameter);
            }

            return parameters.Count > 0
                ? parameters
                : null;
        }

        /// <summary>Gets the header parameters.</summary>
        /// <param name="document">The document.</param>
        /// <param name="routeConfiguration">The route configuration.</param>
        /// <param name="route">The route.</param>
        /// <returns></returns>
        private List<OpenApiParameter> GetHeaderParameters(
            OpenApiDocument document,
            IDeepSleepRequestConfiguration routeConfiguration,
            ApiRoutingItem route)
        {
            var parameters = new List<OpenApiParameter>();

            if (routeConfiguration.UseCorrelationIdHeader ?? false)
            {
                var xCorrelationIdHeader = new OpenApiParameter
                {
                    Name = "X-CorrelationId",
                    In = ParameterLocation.Header,
                    Schema = this.AddSimpleTypeSchema(
                        document: document,
                        type: typeof(string),
                        memberName: null,
                        containingMember: null)
                };

                xCorrelationIdHeader.Description = "A correlation value that will be echoed back within the X-CorrelationId response header.";

                parameters.Add(xCorrelationIdHeader);
            }

            return parameters;
        }

        /// <summary>Replaces the template vars.</summary>
        /// <param name="template">The template.</param>
        /// <returns></returns>
        private string ReplaceTemplateVars(string template)
        {
            var temp = template;

            while (temp.IndexOf("{") >= 0 && temp.IndexOf("}") >= 0)
            {
                var newTemp = string.Empty;
                var start = temp.IndexOf("{");
                var end = temp.IndexOf("}");

                if (start > 0)
                {
                    newTemp = temp.Substring(0, start);
                }

                newTemp += "1";

                if (end < temp.Length - 1)
                {
                    newTemp += temp.Substring(end + 1);
                }

                temp = newTemp;
            }

            return temp;
        }

        /// <summary>Gets the operation tags.</summary>
        /// <param name="document">The document.</param>
        /// <param name="route">The route.</param>
        /// <param name="useReferences">if set to <c>true</c> [use references].</param>
        /// <returns></returns>
        private IList<OpenApiTag> GetOperationTags(
            OpenApiDocument document,
            ApiRoutingItem route,
            bool useReferences = true)
        {
            var tags = this.GetOperationTags(route.Location.MethodInfo);

            if (tags == null || tags.Count == 0)
            {
                return tags;
            }

            var returnTags = new List<OpenApiTag>();

            foreach (var tag in tags)
            {
                var existingTag = document.Tags.FirstOrDefault(t => t.Name == tag.Name);

                if (existingTag == null)
                {
                    document.Tags.Add(tag);
                    existingTag = document.Tags.FirstOrDefault(t => t.Name == tag.Name);
                }

                if (!useReferences)
                {
                    returnTags.Add(tag);
                }
                else
                {
                    returnTags.Add(new OpenApiTag
                    {
                        Reference = new OpenApiReference
                        {
                            Id = $"{existingTag.Name}",
                            Type = ReferenceType.Tag
                        }
                    });
                }
            }

            return returnTags;
        }

        /// <summary>Gets the operation tags.</summary>
        /// <param name="methodInfo">The method information.</param>
        /// <returns></returns>
        private IList<OpenApiTag> GetOperationTags(MethodInfo methodInfo)
        {
            var attributeTags = methodInfo
                .GetCustomAttributes(typeof(OasApiOperationAttribute), false)
                .Cast<OasApiOperationAttribute>()
                .FirstOrDefault()
                ?.Tags ?? new List<string>();

            var tags = attributeTags
                .Where(a => a != null)
                .Where(a => !string.IsNullOrWhiteSpace(a))
                .ToList();

            if (tags.Count > 0)
            {
                return tags.Select(t => new OpenApiTag
                {
                    Name = t
                }).ToList();
            }

            return new List<OpenApiTag>
            {
                new OpenApiTag
                {
                    Name = methodInfo.DeclaringType.Name,
                    Description = OasDocHelpers.GetDocumenationSummary(methodInfo.DeclaringType, this.commentDocs)
                }
            };
        }

        /// <summary>Gets the response headers.</summary>
        /// <param name="document">The document.</param>
        /// <param name="routeConfiguration">The route configuration.</param>
        /// <param name="route">The route.</param>
        /// <returns></returns>
        private Dictionary<string, OpenApiHeader> GetResponseHeaders(
            OpenApiDocument document,
            IDeepSleepRequestConfiguration routeConfiguration,
            ApiRoutingItem route)
        {
            var headers = new Dictionary<string, OpenApiHeader>();

            if (routeConfiguration?.IncludeRequestIdHeaderInResponse ?? false)
            {
                headers.Add("X-RequestId", new OpenApiHeader
                {
                    Schema = this.AddSimpleTypeSchema(
                        document: document,
                        type: typeof(string),
                        memberName: null,
                        containingMember: null)
                });
            }

            if (routeConfiguration?.UseCorrelationIdHeader ?? false)
            {
                headers.Add("X-CorrelationId", new OpenApiHeader
                {
                    Schema = this.AddSimpleTypeSchema(
                        document: document,
                        type: typeof(string),
                        memberName: null,
                        containingMember: null)
                });
            }

            return headers;
        }
    }
}
