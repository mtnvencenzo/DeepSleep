namespace DeepSleep.OpenApi
{
    using DeepSleep.Configuration;
    using DeepSleep.OpenApi.Decorators;
    using Microsoft.OpenApi.Interfaces;
    using Microsoft.OpenApi.Models;
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Reflection;
    using System.Threading.Tasks;
    using System.Xml.Linq;

    /// <summary>
    /// 
    /// </summary>
    public class OpenApiGenerator : IOpenApiGenerator
    {
        private readonly IOpenApiConfigurationProvider configuration;
        private readonly IList<XDocument> commentDocs;

        /// <summary>Initializes a new instance of the <see cref="OpenApiGenerator"/> class.</summary>
        /// <param name="configuration">The configuration.</param>
        /// <exception cref="ArgumentNullException">configuration</exception>
        public OpenApiGenerator(IOpenApiConfigurationProvider configuration)
        {
            this.configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
            this.commentDocs = new List<XDocument>();

            if (this.configuration.XmlDocumentationFileNames != null)
            {
                foreach (var file in this.configuration.XmlDocumentationFileNames)
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
        }

        /// <summary>Generates the specified routing table.</summary>
        /// <param name="routingTable">The routing table.</param>
        /// <param name="defaultRequestConfiguration">The default request configuration.</param>
        /// <returns></returns>
        public async Task<OpenApiDocument> Generate(IApiRoutingTable routingTable, IApiRequestConfiguration defaultRequestConfiguration)
        {
            var routes = routingTable.GetRoutes();

            var document = new OpenApiDocument
            {
                Info = this.configuration.Info,
                Components = new OpenApiComponents(),
                Extensions = new Dictionary<string, IOpenApiExtension>(),
                Paths = new OpenApiPaths(),
                Tags = new List<OpenApiTag>()
            };

            foreach (var route in routes)
            {
                if (route.Name == $"GET_{configuration.V2RouteTemplate}")
                {
                    continue;
                }

                if (route.Name == $"GET_{configuration.V3RouteTemplate}")
                {
                    continue;
                }

                await this.AddRoute(document, route, routingTable, defaultRequestConfiguration).ConfigureAwait(false);
            }

            return document;
        }

        /// <summary>Adds the route.</summary>
        /// <param name="document">The document.</param>
        /// <param name="route">The route.</param>
        /// <param name="routes">The routes.</param>
        /// <param name="defaultRequestConfiguration">The default request configuration.</param>
        private async Task AddRoute(OpenApiDocument document, ApiRoutingItem route, IApiRoutingTable routes, IApiRequestConfiguration defaultRequestConfiguration)
        {
            var pathName = route.Template.StartsWith("/")
                ? route.Template
                : $"/{route.Template}";

            var existingPath = document.Paths.FirstOrDefault(p => string.Equals(p.Key, pathName, StringComparison.OrdinalIgnoreCase));

            var template = ReplaceTemplateVars(route.Template);
            var routeMatch = await new DefaultRouteResolver().MatchRoute(
                routes: routes,
                method: route.HttpMethod,
                requestPath: template).ConfigureAwait(false);

            if (existingPath.Key == null)
            {
                existingPath = new KeyValuePair<string, OpenApiPathItem>(pathName, new OpenApiPathItem
                {
                    Summary = GetDocumenationSummary(routeMatch?.Location?.GetEndpointMethodInfo()) ?? route.Template,
                    Description = GetDocumentationDescription(routeMatch?.Location?.GetEndpointMethodInfo()) ?? string.Empty,
                    Parameters = this.GetParameters(route)
                });

                document.Paths.Add(existingPath.Key, existingPath.Value);
            }

            var operation = this.AddOperation(existingPath.Value, route, route.HttpMethod);
            if (operation == null)
            {
                return;
            }

            var deprecated = routeMatch.Configuration?.Deprecated
                ?? defaultRequestConfiguration?.Deprecated
                ?? ApiRequestContext.GetDefaultRequestConfiguration().Deprecated
                ?? false;

            operation.Deprecated = deprecated;
            operation.Summary = GetDocumenationSummary(routeMatch?.Location?.GetEndpointMethodInfo()) ?? string.Empty;
            operation.Description = GetDocumentationDescription(routeMatch?.Location?.GetEndpointMethodInfo()) ?? string.Empty;

            this.AddOperationResponses(document, operation, route, route.HttpMethod);
            operation.Parameters = this.GetQueryStringParameters(document, route);

            if (route.HttpMethod.ToLower() == "put" || route.HttpMethod.ToLower() == "post" || route.HttpMethod.ToLower() == "patch")
            {
                var bodyParameter = await GetRequestBody(document, route, routes, defaultRequestConfiguration).ConfigureAwait(false);

                if (bodyParameter != null)
                {
                    operation.RequestBody = bodyParameter;
                }
            }

            if (string.Equals(route.HttpMethod, "get", StringComparison.OrdinalIgnoreCase) && this.configuration.IncludeHeadOperationsForGets)
            {
                var definedHeadRoute = routes.GetRoutes()
                    .Where(r => r.HttpMethod.Equals("head", StringComparison.OrdinalIgnoreCase))
                    .Where(r => r.Template.Equals(route.Template, StringComparison.OrdinalIgnoreCase))
                    .FirstOrDefault();

                if (definedHeadRoute == null)
                {
                    var enableHeadForGetRequests = routeMatch.Configuration?.EnableHeadForGetRequests
                        ?? defaultRequestConfiguration?.EnableHeadForGetRequests
                        ?? ApiRequestContext.GetDefaultRequestConfiguration().EnableHeadForGetRequests
                        ?? true;

                    if (enableHeadForGetRequests)
                    {
                        var headOperation = this.AddOperation(existingPath.Value, route, "head");
                        if (headOperation != null)
                        {
                            this.AddOperationResponses(document, headOperation, route, "head");
                            headOperation.Parameters = this.GetQueryStringParameters(document, route);
                        }
                    }
                }
            }
        }

        /// <summary>Gets the parameters.</summary>
        /// <param name="route">The route.</param>
        /// <returns></returns>
        private List<OpenApiParameter> GetParameters(ApiRoutingItem route)
        {
            var container = new List<OpenApiParameter>();

            if (!string.IsNullOrWhiteSpace(route?.Template))
            {
                var template = new ApiRoutingTemplate(route.Template);

                if (template.Variables.Count > 0)
                {
                    foreach (var routeVar in template.Variables)
                    {
                        var schema = this.GetRouteVariableSchema(routeVar, route.Location);

                        if (schema != null)
                        {
                            container.Add(new OpenApiParameter
                            {
                                Name = routeVar,
                                Required = true,
                                In = ParameterLocation.Path,
                                Schema = schema
                            });
                        }
                    }
                }
            }

            return container;
        }

        /// <summary>Gets the route variable schema.</summary>
        /// <param name="routeVar">The route variable.</param>
        /// <param name="location">The location.</param>
        /// <returns></returns>
        private OpenApiSchema GetRouteVariableSchema(string routeVar, ApiEndpointLocation location)
        {
            var uriParameter = location.GetUriParameterInfo();

            if (uriParameter != null)
            {
                var properties = uriParameter.ParameterType.GetProperties()
                   .Where(p => p.CanWrite)
                   .ToArray();

                var prop = properties.FirstOrDefault(p => string.Compare($"{{{p.Name}}}", routeVar, true) == 0);

                if (prop != null)
                {
                    return new OpenApiSchema
                    {
                        Type = Helpers.GetOpenApiSchemaType(prop.PropertyType)
                    };
                }
            }

            return null;
        }

        /// <summary>Adds the operation.</summary>
        /// <param name="pathItem">The path item.</param>
        /// <param name="route">The route.</param>
        /// <param name="httpMethod">The HTTP method.</param>
        /// <returns></returns>
        private OpenApiOperation AddOperation(OpenApiPathItem pathItem, ApiRoutingItem route, string httpMethod)
        {
            var method = httpMethod.ToLowerInvariant();

            Func<OperationType, OpenApiOperation> handler = (type) =>
            {
                if (pathItem.Operations.Any(p => p.Key == type))
                {
                    throw new InvalidOperationException($"{type.ToString().ToUpperInvariant()} operation is already defined on the path item.");
                }

                var operation = new OpenApiOperation
                {
                    OperationId = $"{Helpers.GetOperationId(type.ToString().ToUpperInvariant(), route)}"
                };

                pathItem.AddOperation(type, operation);
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
        private void AddOperationResponses(OpenApiDocument document, OpenApiOperation operation, ApiRoutingItem route, string httpMethod)
        {
            var methodInfo = route.Location.GetEndpointMethodInfo();
            var returnType = Helpers.GetRootType(route.Location.GetMethodInfoReturnType());

            if (returnType == typeof(Task) || string.Equals(httpMethod, "head", StringComparison.OrdinalIgnoreCase))
            {
                returnType = typeof(void);
            }

            var openApiResponseAttributes = methodInfo.GetCustomAttributes(typeof(OpenApiResponseAttribute), false)
                .Cast<OpenApiResponseAttribute>()
                .ToList();

            var responseTypes = new Dictionary<string, Type>();
            foreach (var attribute in openApiResponseAttributes)
            {
                var rootType = Helpers.GetRootType(attribute.ResponseType);
                if (!responseTypes.ContainsKey(attribute.StatusCode))
                {
                    responseTypes.Add(attribute.StatusCode, rootType);
                }
            }

            if (responseTypes.Keys.FirstOrDefault(f => f.StartsWith("2")) == null)
            {
                if (returnType == typeof(void))
                {
                    responseTypes.Add("204", returnType);
                }
                else
                {
                    responseTypes.Add("200", returnType);
                }
            }


            if (responseTypes.Count == 0)
            {
                responseTypes.Add("204", typeof(void));
            }

            foreach (var responseType in responseTypes)
            {
                this.AddResponseToOperation(document, operation, responseType.Key, responseType.Value, route);
            }
        }

        /// <summary>Adds the response to operation.</summary>
        /// <param name="document">The document.</param>
        /// <param name="operation">The operation.</param>
        /// <param name="status">The status.</param>
        /// <param name="responseType">Type of the response.</param>
        /// <param name="route">The route.</param>
        private void AddResponseToOperation(OpenApiDocument document, OpenApiOperation operation, string status, Type responseType, ApiRoutingItem route)
        {
            if (operation.Responses == null)
            {
                operation.Responses = new OpenApiResponses();
            }


            if (responseType != typeof(void))
            {
                operation.Responses.Add(status, new OpenApiResponse
                {
                    Reference = new OpenApiReference
                    {
                        Id = this.AddResponseToDocument(document, responseType),
                        Type = ReferenceType.Response
                    }
                });
            }
            else
            {
                operation.Responses.Add(status, new OpenApiResponse
                {
                    Description = route.Name
                });
            }
        }

        /// <summary>Adds the response to document.</summary>
        /// <param name="document">The document.</param>
        /// <param name="responseType">Type of the response.</param>
        /// <returns></returns>
        private string AddResponseToDocument(OpenApiDocument document, Type responseType)
        {
            var typeName = Helpers.GetDocumentTypeSchemaName(responseType, this.configuration.PrefixNamesWithNamespace);

            if (document.Components == null)
            {
                document.Components = new OpenApiComponents();
            }

            if (document.Components.Responses == null)
            {
                document.Components.Responses = new Dictionary<string, OpenApiResponse>();
            }

            var existing = document.Components.Responses.FirstOrDefault(r => r.Key == typeName);

            if (existing.Value == null)
            {
                document.Components.Responses.Add(typeName, new OpenApiResponse
                {
                    Description = GetDocumentationDescription(responseType) ?? string.Empty,
                    Content = GetResponseContents(document, responseType)
                });
            }

            return $"{typeName}";
        }

        /// <summary>Gets the response contents.</summary>
        /// <param name="document">The document.</param>
        /// <param name="responseType">Type of the response.</param>
        /// <returns></returns>
        private Dictionary<string, OpenApiMediaType> GetResponseContents(OpenApiDocument document, Type responseType)
        {
            var contents = new Dictionary<string, OpenApiMediaType>();

            // TODO: Need ot base this off of the route configuration
            var contentTypes = new string[]
            {
                "application/json",
                "text/xml"
            };

            foreach (var contentType in contentTypes.Distinct())
            {
                contents.Add(contentType, new OpenApiMediaType
                {
                    Schema = this.AddAndGetSchemaRefrence(document, responseType)
                });
            }

            return contents;
        }

        /// <summary>Adds the and get schema refrence.</summary>
        /// <param name="document">The document.</param>
        /// <param name="type">The type.</param>
        /// <param name="referenceType">Type of the reference.</param>
        /// <returns></returns>
        private OpenApiSchema AddAndGetSchemaRefrence(OpenApiDocument document, Type type, ReferenceType referenceType = ReferenceType.Schema)
        {
            if (document.Components == null)
            {
                document.Components = new OpenApiComponents();
            }

            if (document.Components.Schemas == null)
            {
                document.Components.Schemas = new Dictionary<string, OpenApiSchema>();
            }

            var existingSchema = document.Components.Schemas.FirstOrDefault(s => string.Equals(s.Key, Helpers.GetDocumentTypeSchemaName(type, this.configuration.PrefixNamesWithNamespace), StringComparison.Ordinal));

            if (existingSchema.Value == null)
            {
                var schema = AddSchema(document, type);
                existingSchema = document.Components.Schemas.FirstOrDefault(s => string.Equals(s.Key, Helpers.GetDocumentTypeSchemaName(type, this.configuration.PrefixNamesWithNamespace), StringComparison.Ordinal));

                if (existingSchema.Value == null)
                {
                    document.Components.Schemas.Add(Helpers.GetDocumentTypeSchemaName(type, this.configuration.PrefixNamesWithNamespace), schema);
                }
            }

            return new OpenApiSchema
            {
                Reference = new OpenApiReference
                {
                    Id = $"{Helpers.GetDocumentTypeSchemaName(type, this.configuration.PrefixNamesWithNamespace)}",
                    Type = referenceType
                }
            };
        }

        /// <summary>Adds the schema.</summary>
        /// <param name="document">The document.</param>
        /// <param name="type">The type.</param>
        /// <param name="returnReferenceIfAddedAsRoot">if set to <c>true</c> [return reference if added as root].</param>
        /// <returns></returns>
        private OpenApiSchema AddSchema(OpenApiDocument document, Type type, bool returnReferenceIfAddedAsRoot = true)
        {
            var existingSchema = document.Components.Schemas.FirstOrDefault(s => string.Equals(s.Key, Helpers.GetDocumentTypeSchemaName(type, this.configuration.PrefixNamesWithNamespace), StringComparison.Ordinal));

            if (existingSchema.Value != null)
            {
                if (returnReferenceIfAddedAsRoot)
                {
                    return new OpenApiSchema
                    {
                        Reference = new OpenApiReference
                        {
                            Id = $"{existingSchema.Key}",
                            Type = ReferenceType.Schema
                        }
                    };
                }
                else
                {
                    return existingSchema.Value;
                }
            }


            var schema = existingSchema.Value;

            if (existingSchema.Value == null)
            {
                if (Helpers.IsArrayType(type))
                {
                    schema = AddArraySchema(document, type);
                }
                else if (Helpers.IsComplexType(type))
                {
                    schema = AddComplexTypeSchema(document, type, returnReferenceIfAddedAsRoot);
                }
                else
                {
                    schema = AddSimpleTypeSchema(type);
                }
            }

            return schema;
        }

        /// <summary>
        /// Adds the array schema.
        /// </summary>
        /// <param name="document">The document.</param>
        /// <param name="type">The type.</param>
        /// <returns></returns>
        private OpenApiSchema AddArraySchema(OpenApiDocument document, Type type)
        {
            var arrayType = Helpers.GetRootType(Helpers.GetArrayType(type));

            return new OpenApiSchema
            {
                Description = GetDocumentationDescription(arrayType) ?? string.Empty,
                Type = Helpers.GetOpenApiSchemaType(type),
                Items = AddSchema(document, arrayType, true)
            };
        }

        /// <summary>
        /// Adds the complex type schema.
        /// </summary>
        /// <param name="document">The document.</param>
        /// <param name="type">The type.</param>
        /// <param name="returnReferenceIfAddedAsRoot">if set to <c>true</c> [return reference if added as root].</param>
        /// <returns></returns>
        private OpenApiSchema AddComplexTypeSchema(OpenApiDocument document, Type type, bool returnReferenceIfAddedAsRoot = true)
        {
            var rootType = Helpers.GetRootType(type);
            var existingSchema = document.Components.Schemas.FirstOrDefault(s => string.Equals(s.Key, Helpers.GetDocumentTypeSchemaName(rootType, this.configuration.PrefixNamesWithNamespace), StringComparison.Ordinal));

            if (existingSchema.Value != null)
            {
                if (returnReferenceIfAddedAsRoot)
                {
                    return new OpenApiSchema
                    {
                        Reference = new OpenApiReference
                        {
                            Id = $"{existingSchema.Key}",
                            Type = ReferenceType.Schema
                        }
                    };
                }
                else
                {
                    return existingSchema.Value;
                }
            }

            var schema = new OpenApiSchema
            {
                Description = GetDocumentationDescription(rootType) ?? string.Empty,
                Type = Helpers.GetOpenApiSchemaType(rootType),
                Properties = new Dictionary<string, OpenApiSchema>()
            };

            document.Components.Schemas.Add(Helpers.GetDocumentTypeSchemaName(rootType, this.configuration.PrefixNamesWithNamespace), schema);

            foreach (var prop in rootType.GetProperties())
            {
                schema.Properties.Add(prop.Name, this.AddSchema(document, prop.PropertyType, true));
            }

            if (returnReferenceIfAddedAsRoot)
            {
                return new OpenApiSchema
                {
                    Reference = new OpenApiReference
                    {
                        Id = $"{Helpers.GetDocumentTypeSchemaName(rootType, this.configuration.PrefixNamesWithNamespace)}",
                        Type = ReferenceType.Schema
                    }
                };
            }
            else
            {
                return schema;
            }
        }

        /// <summary>Adds the simple type schema.</summary>
        /// <param name="type">The type.</param>
        /// <returns></returns>
        private OpenApiSchema AddSimpleTypeSchema(Type type)
        {
            var rootType = Helpers.GetRootType(type);

            var openApiType = Helpers.GetOpenApiSchemaType(rootType);

            return new OpenApiSchema
            {
                Description = GetDocumentationDescription(rootType) ?? string.Empty,
                Type = openApiType,
                Format = Helpers.GetOpenApiSchemaFormat(openApiType, rootType),
                Nullable = Helpers.IsNullableType(type)
            };
        }

        /// <summary>Gets the query string parameters.</summary>
        /// <param name="document">The document.</param>
        /// <param name="route">The route.</param>
        /// <returns></returns>
        private List<OpenApiParameter> GetQueryStringParameters(OpenApiDocument document, ApiRoutingItem route)
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
                    var schema = new OpenApiSchema
                    {
                        Type = Helpers.GetOpenApiSchemaType(prop.PropertyType),
                        Nullable = !prop.PropertyType.IsValueType
                    };

                    parameters.Add(new OpenApiParameter
                    {
                        Description = GetDocumentationDescription(prop) ?? string.Empty,
                        Name = prop.Name,
                        Required = false,
                        In = ParameterLocation.Query,
                        Schema = schema
                    });
                }
            }

            var simpleParameters = route.Location.GetSimpleParametersInfo()
                .Where(p => !preparedRouteVars.Contains(p.Name.ToLower()))
                .Where(p => properties.Any(prop => prop.Name.ToLower() == p.Name.ToLower()) == false)
                .ToList();

            foreach (var param in simpleParameters)
            {
                var schema = new OpenApiSchema
                {
                    Type = Helpers.GetOpenApiSchemaType(param.ParameterType),
                    Nullable = !param.ParameterType.IsValueType
                };

                parameters.Add(new OpenApiParameter
                {
                    Description = GetDocumentationDescription(route.Location.GetEndpointMethodInfo(), param) ?? string.Empty,
                    Name = param.Name,
                    Required = false,
                    In = ParameterLocation.Query,
                    Schema = schema
                });
            }

            return parameters.Count > 0
                ? parameters
                : null;
        }

        /// <summary>Gets the request body.</summary>
        /// <param name="document">The document.</param>
        /// <param name="route">The route.</param>
        /// <param name="routes">The routes.</param>
        /// <param name="defaultRequestConfiguration">The default request configuration.</param>
        /// <returns></returns>
        private async Task<OpenApiRequestBody> GetRequestBody(OpenApiDocument document, ApiRoutingItem route, IApiRoutingTable routes, IApiRequestConfiguration defaultRequestConfiguration)
        {
            var bodyParameter = route.Location.GetBodyParameterInfo();

            if (bodyParameter != null)
            {
                var schema = this.AddAndGetSchemaRefrence(document, bodyParameter.ParameterType, ReferenceType.RequestBody);

                var content = new OpenApiMediaType
                {
                    Schema = schema,
                };

                //var template = ReplaceTemplateVars(route.Template);
                //var routeMatch = await new DefaultRouteResolver().MatchRoute(
                //    routes: routes,
                //    method: route.HttpMethod,
                //    requestPath: template).ConfigureAwait(false);

                //var readerResolver = routeMatch.Configuration?.ReadWriteConfiguration?.ReaderResolver
                //    ?? defaultRequestConfiguration?.ReadWriteConfiguration?.ReaderResolver
                //    ?? ApiRequestContext.GetDefaultRequestConfiguration().ReadWriteConfiguration?.ReaderResolver;

                //var supportedReadableMediaTypes = routeMatch.Configuration?.ReadWriteConfiguration?.ReadableMediaTypes
                //    ?? defaultRequestConfiguration?.ReadWriteConfiguration?.ReadableMediaTypes
                //    ?? ApiRequestContext.GetDefaultRequestConfiguration().ReadWriteConfiguration?.ReadableMediaTypes;

                //var formatters

                // TODO: */* ?
                return new OpenApiRequestBody
                {
                    Content = new Dictionary<string, OpenApiMediaType>
                    {
                        { "*/*", content }
                    },
                    Required = true
                };
            }

            return null;
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

        private string GetDocumenationSummary(MethodInfo methodInfo)
        {
            var methodDocDescriptor = GenerateDocMethodDescriptor(methodInfo);

            foreach (var commentDoc in this.commentDocs)
            {
                var summary = commentDoc
                    .Root
                    ?.Element("members")
                    ?.Elements("member")
                    ?.Where(e => e.Attribute("name") != null)
                    ?.Where(e => e.Attribute("name").Value == methodDocDescriptor)
                    ?.FirstOrDefault()
                    ?.Element("summary")
                    ?.Value;

                if (summary != null)
                {
                    return summary;
                }
            }

            return null;
        }

        private string GetDocumentationDescription(MethodInfo methodInfo)
        {
            var methodDocDescriptor = GenerateDocMethodDescriptor(methodInfo);

            foreach (var commentDoc in this.commentDocs)
            {
                var remarks = commentDoc
                    .Root
                    ?.Element("members")
                    ?.Elements("member")
                    ?.Where(e => e.Attribute("name") != null)
                    ?.Where(e => e.Attribute("name").Value == methodDocDescriptor)
                    ?.FirstOrDefault()
                    ?.Element("remarks")
                    ?.Value;

                if (remarks != null)
                {
                    return remarks;
                }
            }

            return null;
        }

        private string GetDocumentationDescription(PropertyInfo property)
        {
            var propertyDocDescriptor = GenerateDocPropertyDescriptor(property);

            foreach (var commentDoc in this.commentDocs)
            {
                var summary = commentDoc
                    .Root
                    ?.Element("members")
                    ?.Elements("member")
                    ?.Where(e => e.Attribute("name") != null)
                    ?.Where(e => e.Attribute("name").Value == propertyDocDescriptor)
                    ?.FirstOrDefault()
                    ?.Element("summary")
                    ?.Value;

                if (summary != null)
                {
                    return summary;
                }
            }

            return null;
        }

        private string GetDocumentationDescription(MethodInfo methodInfo, ParameterInfo parameter)
        {
            var methodDocDescriptor = GenerateDocMethodDescriptor(methodInfo);

            foreach (var commentDoc in this.commentDocs)
            {
                var summary = commentDoc
                    .Root
                    ?.Element("members")
                    ?.Elements("member")
                    ?.Where(e => e.Attribute("name") != null)
                    ?.Where(e => e.Attribute("name").Value == methodDocDescriptor)
                    ?.FirstOrDefault()
                    ?.Elements("param")
                    ?.Where(e => e.Attribute("name") != null)
                    ?.Where(e => e.Attribute("name").Value == parameter.Name)
                    ?.FirstOrDefault()
                    ?.Value;

                if (summary != null)
                {
                    return summary;
                }
            }

            return null;
        }

        private string GetDocumentationDescription(Type type)
        {
            var typeDocDescriptor = GenerateDocTypeDescriptor(type);

            foreach (var commentDoc in this.commentDocs)
            {
                var summary = commentDoc
                    .Root
                    ?.Element("members")
                    ?.Elements("member")
                    ?.Where(e => e.Attribute("name") != null)
                    ?.Where(e => e.Attribute("name").Value == typeDocDescriptor)
                    ?.FirstOrDefault()
                    ?.Element("summary")
                    ?.Value;

                if (summary != null)
                {
                    return summary;
                }
            }

            return null;
        }

        private string GenerateDocMethodDescriptor(MethodInfo methodInfo)
        {
            var name = $"M:{GenerateTypeDescriptor(methodInfo.DeclaringType)}.{methodInfo.Name}(";

            var parameters = methodInfo.GetParameters();

            foreach (var p in parameters)
            {
                name += $"{GenerateTypeDescriptor(p.ParameterType)},";
            }
            name = name.Substring(0, name.Length - 1);
            name += ")";

            return name;
        }

        private string GenerateDocPropertyDescriptor(PropertyInfo property)
        {
            var name = $"P:{GenerateTypeDescriptor(property.DeclaringType)}.{property.Name}";

            //var parameters = methodInfo.GetParameters();

            //foreach (var p in parameters)
            //{
            //    name += $"{GenerateTypeDescriptor(p.ParameterType)},";
            //}
            //name = name.Substring(0, name.Length - 1);
            //name += ")";

            return name;
        }

        private string GenerateDocTypeDescriptor(Type type)
        {
            var name = $"T:{GenerateTypeDescriptor(type)}";

            return name;
        }

        private string GenerateTypeDescriptor(Type type)
        {
            if (type.IsGenericType)
            {
                var typeName = type.Name.Contains("`")
                    ? type.Name.Substring(0, type.Name.IndexOf("`"))
                    : type.Name;

                var name = $"{type.Namespace}.{typeName}{{";

                var parameters = type.GetGenericArguments();
                parameters.ToList().ForEach(p => name += $"{GenerateTypeDescriptor(p)},");

                name = name.Substring(0, name.Length - 1);
                name += "}";
                return name;
            }
            else
            {
                return type.FullName;
            }
        }
    }
}
