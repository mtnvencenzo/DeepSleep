namespace DeepSleep.OpenApi
{
    using DeepSleep.OpenApi.Decorators;
    using DeepSleep.OpenApi.v3_0;
    using Newtonsoft.Json;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;
    using System.Threading.Tasks;

    /// <summary></summary>
    /// <seealso cref="DeepSleep.OpenApi.IOpenApiGenerator" />
    public class DefaultOpenApiGenerator : IOpenApiGenerator
    {
        /// <summary>
        /// The prefix names with namespace
        /// </summary>
        public static bool PrefixNamesWithNamespace = true;

        /// <summary>
        /// The include head operations for gets
        /// </summary>
        public static bool IncludeHeadOperationsForGets = true;

        /// <summary>
        /// Generates the specified version.
        /// </summary>
        /// <param name="version">The version.</param>
        /// <param name="routingTable">The routing table.</param>
        /// <returns></returns>
        public OpenApiDocument3_0 Generate(OpenApiVersion version, IApiRoutingTable routingTable)
        {
            var routes = routingTable.GetRoutes();

            var document = new OpenApiDocument3_0
            {
                info = new OpenApiInfo3_0
                {
                    version = "1.0.0",
                    title = "my api",
                    termsOfService = "http://www.vecchi.us/termsOfService.htm",
                    license = new OpenApiLicense3_0
                    {
                        name = "MIT",
                        url = "http://www.vecchi.us/license.htm"
                    },
                    description = "My Api description",
                    contact = new OpenApiContact3_0
                    {
                        email = "rvecchi@gmail.com",
                        name = "Ronaldo Vecchi",
                        url = "http://www.vecchi.us/contact.htm"

                    }
                },
                components = new OpenApiComponents3_0(),
            };

            foreach (var route in routes)
            {
                if (route.Name == "GET_openapi")
                {
                    continue;
                }

                this.AddRoute(document, route);
            }

            return document;
        }

        /// <summary>
        /// Adds the route.
        /// </summary>
        /// <param name="document">The document.</param>
        /// <param name="route">The route.</param>
        private void AddRoute(OpenApiDocument3_0 document, ApiRoutingItem route)
        {
            var pathName = route.Template.StartsWith("/")
                ? route.Template
                : $"/{route.Template}";

            var existingPath = document.paths.FirstOrDefault(p => string.Equals(p.Key, pathName, StringComparison.OrdinalIgnoreCase));

            if (existingPath.Key == null)
            {
                existingPath = new KeyValuePair<string, OpenApiPathItem3_0>(pathName, new OpenApiPathItem3_0
                {
                    description = route.Name,
                    summary = route.Name,
                    parameters = this.GetParameters(document, route)
                });

                document.paths.Add(existingPath.Key, existingPath.Value);
            }

            var operation = this.AddOperation(document, existingPath.Value, route, route.HttpMethod);
            if (operation == null)
            {
                return;
            }

            this.AddOperationResponses(document, operation, route, route.HttpMethod);
            operation.parameters = this.GetQueryStringParameters(document, route);

            if (route.HttpMethod.ToLower() == "put" || route.HttpMethod.ToLower() == "post" || route.HttpMethod.ToLower() == "patch")
            {
                var bodyParameter = GetRequestBody(document, route);

                if (bodyParameter != null)
                {
                    operation.requestBody = bodyParameter;
                }
            }


            if (string.Equals(route.HttpMethod, "get", StringComparison.OrdinalIgnoreCase) && IncludeHeadOperationsForGets)
            {
                var headOperation = this.AddOperation(document, existingPath.Value, route, "head");
                if (headOperation != null)
                {
                    this.AddOperationResponses(document, headOperation, route, "head");
                    headOperation.parameters = this.GetQueryStringParameters(document, route);
                }
            }
        }

        /// <summary>
        /// Adds the operation.
        /// </summary>
        /// <param name="document">The document.</param>
        /// <param name="pathItem">The path item.</param>
        /// <param name="route">The route.</param>
        /// <param name="httpMethod">The HTTP method.</param>
        /// <returns></returns>
        /// <exception cref="InvalidOperationException">
        /// </exception>
        private OpenApiOperation3_0 AddOperation(OpenApiDocument3_0 document, OpenApiPathItem3_0 pathItem, ApiRoutingItem route, string httpMethod)
        {
            var method = httpMethod.ToLowerInvariant();

            if (method == "delete")
            {
                if (pathItem.delete != null)
                {
                    throw new InvalidOperationException($"{method} operation is alread defined on the path item.");
                }

                pathItem.delete = new OpenApiOperation3_0 { operationId = $"{Helpers.GetOperationId(httpMethod, route)}" };
                return pathItem.delete;
            }

            if (method == "get")
            {
                if (pathItem.get != null)
                {
                    throw new InvalidOperationException($"{method} operation is alread defined on the path item.");
                }

                pathItem.get = new OpenApiOperation3_0 { operationId = $"{Helpers.GetOperationId(httpMethod, route)}" };
                return pathItem.get;
            }

            if (method == "head")
            {
                if (pathItem.head != null)
                {
                    throw new InvalidOperationException($"{method} operation is alread defined on the path item.");
                }

                pathItem.head = new OpenApiOperation3_0 { operationId = $"{Helpers.GetOperationId(httpMethod, route)}" };
                return pathItem.head;
            }

            if (method == "options")
            {
                if (pathItem.options != null)
                {
                    throw new InvalidOperationException($"{method} operation is alread defined on the path item.");
                }

                pathItem.options = new OpenApiOperation3_0 { operationId = $"{Helpers.GetOperationId(httpMethod, route)}" };
                return pathItem.options;
            }

            if (method == "patch")
            {
                if (pathItem.patch != null)
                {
                    throw new InvalidOperationException($"{method} operation is alread defined on the path item.");
                }

                pathItem.patch = new OpenApiOperation3_0 { operationId = $"{Helpers.GetOperationId(httpMethod, route)}" };
                return pathItem.patch;
            }

            if (method == "post")
            {
                if (pathItem.post != null)
                {
                    throw new InvalidOperationException($"{method} operation is alread defined on the path item.");
                }

                pathItem.post = new OpenApiOperation3_0 { operationId = $"{Helpers.GetOperationId(httpMethod, route)}" };
                return pathItem.post;
            }

            if (method == "put")
            {
                if (pathItem.put != null)
                {
                    throw new InvalidOperationException($"{method} operation is alread defined on the path item.");
                }

                pathItem.put = new OpenApiOperation3_0 { operationId = $"{Helpers.GetOperationId(httpMethod, route)}" };
                return pathItem.put;
            }

            if (method == "trace")
            {
                if (pathItem.trace != null)
                {
                    throw new InvalidOperationException($"{method} operation is alread defined on the path item.");
                }

                pathItem.trace = new OpenApiOperation3_0 { operationId = $"{Helpers.GetOperationId(httpMethod, route)}" };
                return pathItem.trace;
            }

            return null;
        }

        /// <summary>
        /// Adds the operation responses.
        /// </summary>
        /// <param name="document">The document.</param>
        /// <param name="operation">The operation.</param>
        /// <param name="route">The route.</param>
        /// <param name="httpMethod">The HTTP method.</param>
        private void AddOperationResponses(OpenApiDocument3_0 document, OpenApiOperation3_0 operation, ApiRoutingItem route, string httpMethod)
        {
            var methodInfo = route.EndpointLocation.GetEndpointMethod();
            var returnType = Helpers.GetRootType(route.EndpointLocation.GetEndpointReturnType());

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

            if (returnType == typeof(ApiResponse) && responseTypes.Count == 0)
            {
                var typeDescriptorMethod = route.EndpointLocation.Controller.GetMethod($"{methodInfo.Name}TypeDescriptor", BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic);

                if (typeDescriptorMethod != null && typeDescriptorMethod.ReturnType == typeof(Type) && typeDescriptorMethod.IsStatic)
                {
                    var foundType = typeDescriptorMethod.Invoke(null, null) as Type;
                    if (foundType != null)
                    {
                        returnType = foundType;
                    }
                }
            }

            if (returnType != typeof(ApiResponse) && responseTypes.Keys.FirstOrDefault(f => f.StartsWith("2")) == null)
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

        /// <summary>
        /// Adds the response to operation.
        /// </summary>
        /// <param name="document">The document.</param>
        /// <param name="operation">The operation.</param>
        /// <param name="status">The status.</param>
        /// <param name="responseType">Type of the response.</param>
        /// <param name="route">The route.</param>
        private void AddResponseToOperation(OpenApiDocument3_0 document, OpenApiOperation3_0 operation, string status, Type responseType, ApiRoutingItem route)
        {
            if (operation.responses == null)
            {
                operation.responses = new Dictionary<string, OpenApiResponse3_0>();
            }


            if (responseType != typeof(void))
            {
                operation.responses.Add(status, new OpenApiResponse3_0
                {
                    @ref = this.AddResponseToDocument(document, responseType, route)
                });
            }
            else
            {
                operation.responses.Add(status, new OpenApiResponse3_0
                {
                    description = route.Name
                });
            }
        }

        /// <summary>
        /// Adds the response to document.
        /// </summary>
        /// <param name="document">The document.</param>
        /// <param name="responseType">Type of the response.</param>
        /// <param name="route">The route.</param>
        /// <returns></returns>
        private string AddResponseToDocument(OpenApiDocument3_0 document, Type responseType, ApiRoutingItem route)
        {
            var typeName = Helpers.GetDocumentTypeSchemaName(responseType);

            var existing = document.components.responses.FirstOrDefault(r => r.Key == typeName);

            if (existing.Value == null)
            {
                if (document.components.responses == null)
                {
                    document.components.responses = new Dictionary<string, OpenApiResponse3_0>();
                }

                document.components.responses.Add(typeName, new OpenApiResponse3_0
                {
                    description = typeName,
                    content = GetResponseContents(document, responseType)
                });
            }

            return $"#/components/responses/{typeName}";
        }

        /// <summary>
        /// Gets the response contents.
        /// </summary>
        /// <param name="document">The document.</param>
        /// <param name="responseType">Type of the response.</param>
        /// <returns></returns>
        private Dictionary<string, OpenApiMediaType3_0> GetResponseContents(OpenApiDocument3_0 document, Type responseType)
        {
            var contents = new Dictionary<string, OpenApiMediaType3_0>();

            var contentTypes = new string[]
            {
                "application/json",
                "text/xml"
            };

            foreach (var contentType in contentTypes.Distinct())
            {
                contents.Add(contentType, new OpenApiMediaType3_0
                {
                    schema = this.AddAndGetSchemaRefrence(document, responseType)
                });
            }

            return contents;
        }

        /// <summary>
        /// Adds the and get schema refrence.
        /// </summary>
        /// <param name="document">The document.</param>
        /// <param name="type">The type.</param>
        /// <returns></returns>
        private OpenApiSchema3_0 AddAndGetSchemaRefrence(OpenApiDocument3_0 document, Type type)
        {
            var existingSchema = document.components.schemas.FirstOrDefault(s => string.Equals(s.Key, Helpers.GetDocumentTypeSchemaName(type), StringComparison.Ordinal));

            if (existingSchema.Value == null)
            {
                var schema = AddSchema(document, type);
                existingSchema = document.components.schemas.FirstOrDefault(s => string.Equals(s.Key, Helpers.GetDocumentTypeSchemaName(type), StringComparison.Ordinal));

                if (existingSchema.Value == null)
                {
                    if (document.components.schemas == null)
                    {
                        document.components.schemas = new Dictionary<string, OpenApiSchema3_0>();
                    }

                    document.components.schemas.Add(Helpers.GetDocumentTypeSchemaName(type), schema);
                }
            }

            return new OpenApiSchema3_0
            {
                @ref = $"#/components/schemas/{Helpers.GetDocumentTypeSchemaName(type)}"
            };
        }

        /// <summary>
        /// Adds the schema.
        /// </summary>
        /// <param name="document">The document.</param>
        /// <param name="type">The type.</param>
        /// <param name="returnReferenceIfAddedAsRoot">if set to <c>true</c> [return reference if added as root].</param>
        /// <returns></returns>
        private OpenApiSchema3_0 AddSchema(OpenApiDocument3_0 document, Type type, bool returnReferenceIfAddedAsRoot = true)
        {
            var existingSchema = document.components.schemas.FirstOrDefault(s => string.Equals(s.Key, Helpers.GetDocumentTypeSchemaName(type), StringComparison.Ordinal));

            if (existingSchema.Value != null)
            {
                if (returnReferenceIfAddedAsRoot)
                {
                    return new OpenApiSchema3_0
                    {
                        @ref = $"#/components/schemas/{existingSchema.Key}"
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
        private OpenApiSchema3_0 AddArraySchema(OpenApiDocument3_0 document, Type type)
        {
            var arrayType = Helpers.GetRootType(Helpers.GetArrayType(type));

            return new OpenApiSchema3_0
            {
                description = Helpers.GetDocumentTypeSchemaName(arrayType),
                type = Helpers.GetOpenApiSchemaType(type),
                items = AddSchema(document, arrayType, true)
            };
        }

        /// <summary>
        /// Adds the simple type schema.
        /// </summary>
        /// <param name="type">The type.</param>
        /// <returns></returns>
        private OpenApiSchema3_0 AddSimpleTypeSchema(Type type)
        {
            var rootType = Helpers.GetRootType(type);

            var openApiType = Helpers.GetOpenApiSchemaType(rootType);

            return new OpenApiSchema3_0
            {
                description = rootType.Name,
                type = openApiType,
                format = Helpers.GetOpenApiSchemaFormat(openApiType, rootType),
                nullable = Helpers.IsNullableType(type) ? true : (bool?) null
            };
        }

        /// <summary>
        /// Adds the complex type schema.
        /// </summary>
        /// <param name="document">The document.</param>
        /// <param name="type">The type.</param>
        /// <param name="returnReferenceIfAddedAsRoot">if set to <c>true</c> [return reference if added as root].</param>
        /// <returns></returns>
        private OpenApiSchema3_0 AddComplexTypeSchema(OpenApiDocument3_0 document, Type type, bool returnReferenceIfAddedAsRoot = true)
        {
            var rootType = Helpers.GetRootType(type);
            var existingSchema = document.components.schemas.FirstOrDefault(s => string.Equals(s.Key, Helpers.GetDocumentTypeSchemaName(rootType), StringComparison.Ordinal));

            if (existingSchema.Value != null)
            {
                if (returnReferenceIfAddedAsRoot)
                {
                    return new OpenApiSchema3_0
                    {
                        @ref = $"#/components/schemas/{existingSchema.Key}"
                    };
                }
                else
                {
                    return existingSchema.Value;
                }
            }

            var schema = new OpenApiSchema3_0
            {
                description = rootType.FullName,
                type = Helpers.GetOpenApiSchemaType(rootType),
                properties = new Dictionary<string, OpenApiSchema3_0>()
            };

            document.components.schemas.Add(Helpers.GetDocumentTypeSchemaName(rootType), schema);

            foreach (var prop in rootType.GetProperties())
            {
                if (schema.properties == null)
                {
                    schema.properties = new Dictionary<string, OpenApiSchema3_0>();
                }

                schema.properties.Add(prop.Name, this.AddSchema(document, prop.PropertyType, true));
            }

            if (returnReferenceIfAddedAsRoot)
            {
                return new OpenApiSchema3_0
                {
                    @ref = $"#/components/schemas/{Helpers.GetDocumentTypeSchemaName(rootType)}"
                };
            }
            else
            {
                return schema;
            }
        }

        /// <summary>
        /// Gets the parameters.
        /// </summary>
        /// <param name="document">The document.</param>
        /// <param name="route">The route.</param>
        /// <returns></returns>
        private List<OpenApiParameter3_0> GetParameters(OpenApiDocument3_0 document, ApiRoutingItem route)
        {
            var container = new List<OpenApiParameter3_0>();

            if ((route.VariablesList?.Count ?? 0) > 0)
            {
                foreach (var routeVar in route.VariablesList)
                {
                    var schema = this.GetRouteVariableSchema(document, routeVar, route.EndpointLocation);

                    if (schema != null)
                    {
                        var paramName = routeVar.Replace("{", string.Empty).Replace("}", string.Empty);

                        container.Add(new OpenApiParameter3_0
                        {
                            name = paramName,
                            required = true,
                            @in = OpenApiIn3_0.path.ToString(),
                            schema = schema
                        });
                    }
                }
            }

            return container;
        }

        /// <summary>Gets the query string parameters.</summary>
        /// <param name="document">The document.</param>
        /// <param name="route">The route.</param>
        /// <returns></returns>
        private List<OpenApiParameter3_0> GetQueryStringParameters(OpenApiDocument3_0 document, ApiRoutingItem route)
        {
            var container = new List<OpenApiParameter3_0>();

            var uriParameter = route.EndpointLocation.GetUriParameter();

            if (uriParameter != null)
            {
                var preparedRouteVars = route.VariablesList != null
                    ? route.VariablesList.Select(v => v.ToLower().Replace("{", string.Empty).Replace("}", string.Empty)).ToList()
                    : new List<string>();

                var properties = uriParameter.ParameterType.GetProperties()
                   .Where(p => p.CanWrite)
                   .Where(p => !preparedRouteVars.Contains(p.Name.ToLower()))
                   .ToArray();

                foreach (var prop in properties)
                {
                    var schema = new OpenApiSchema3_0
                    {
                        type = Helpers.GetOpenApiSchemaType(prop.PropertyType),
                        nullable = !prop.PropertyType.IsValueType
                    };

                    container.Add(new OpenApiParameter3_0
                    {
                        name = prop.Name,
                        required = false,
                        @in = OpenApiIn3_0.query.ToString(),
                        schema = schema
                    });
                }
            }

            return container.Count > 0
                ? container
                : null;
        }

        /// <summary>Gets the request body parameter.</summary>
        /// <param name="document">The document.</param>
        /// <param name="route">The route.</param>
        /// <returns></returns>
        private OpenApiRequestBody3_0 GetRequestBody(OpenApiDocument3_0 document, ApiRoutingItem route)
        {
            var bodyParameter = route.EndpointLocation.GetBodyParameter();

            if (bodyParameter != null)
            {
                var schema = this.AddAndGetSchemaRefrence(document, bodyParameter.ParameterType);

                var content = new OpenApiMediaType3_0
                {
                    schema = schema,
                };

                return new OpenApiRequestBody3_0
                {
                    content = new Dictionary<string, OpenApiMediaType3_0>
                    {
                        { "*/*", content }
                    },
                    required = true
                };
            }

            return null;
        }

        /// <summary>
        /// Gets the route variable schema.
        /// </summary>
        /// <param name="document">The document.</param>
        /// <param name="routeVar">The route variable.</param>
        /// <param name="location">The location.</param>
        /// <returns></returns>
        private OpenApiSchema3_0 GetRouteVariableSchema(OpenApiDocument3_0 document, string routeVar, ApiEndpointLocation location)
        {
            var uriParameter = location.GetUriParameter();

            if (uriParameter != null)
            {
                var properties = uriParameter.ParameterType.GetProperties()
                   .Where(p => p.CanWrite)
                   .ToArray();
 
                var prop = properties.FirstOrDefault(p => string.Compare($"{{{p.Name}}}", routeVar, true) == 0);

                if (prop != null)
                {
                    return new OpenApiSchema3_0
                    {
                        type = Helpers.GetOpenApiSchemaType(prop.PropertyType)
                    };
                }
            }

            return null;
        }
    }
}
