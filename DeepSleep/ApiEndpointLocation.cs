namespace DeepSleep
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Linq;
    using System.Reflection;
    using System.Text.Json.Serialization;
    using System.Threading.Tasks;

    /// <summary>
    /// 
    /// </summary>
    [DebuggerDisplay("{Controller?.Name} {Endpoint}")]
    public class ApiEndpointLocation
    {
        private MethodInfo methodInfo;
        private ParameterInfo uriParameter;
        private ParameterInfo bodyParameter;
        private ParameterInfo[] boundParameters;
        private ParameterInfo[] simpleParameters;

        /// <summary>Gets or sets the controller.</summary>
        /// <value>The controller.</value>
        [JsonIgnore]
        public Type Controller { get; set; }

        /// <summary>Gets or sets the endpoint.</summary>
        /// <value>The endpoint.</value>
        public string Endpoint { get; set; }

        /// <summary>Gets or sets the HTTP method.</summary>
        /// <value>The HTTP method.</value>
        public string HttpMethod { get; set; }

        /// <summary>Gets the endpoint method.</summary>
        /// <returns></returns>
        /// <exception cref="Exception">Routing items endpoint method '{this.Endpoint}' does not exist on controller '{this.Controller.Name}'.</exception>
        public MethodInfo GetEndpointMethod()
        {
            if (this.methodInfo == null)
            {
                var methods = this.Controller.GetMethods(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.InvokeMethod);
                if (methods != null)
                {
                    foreach (var methodinfo in methods)
                    {
                        if (string.Compare(methodinfo.Name, this.Endpoint, false) == 0)
                        {
                            methodInfo = methodinfo;
                            break;
                        }
                    }
                }

                if (this.methodInfo == null)
                {
                    throw new Exception($"Routing items endpoint method '{this.Endpoint}' does not exist on controller '{this.Controller.Name}'.");
                }
            }

            return methodInfo;
        }

        /// <summary>Gets the type of the endpoint return.</summary>
        /// <returns></returns>
        public Type GetEndpointReturnType()
        {
            var methodInfo = this.GetEndpointMethod();

            if (methodInfo.ReturnType.IsSubclassOf(typeof(Task)) && methodInfo.ReturnType.IsGenericType)
            {
                var type = methodInfo.ReturnType.GenericTypeArguments[0];
                return type;
            }

            return methodInfo.ReturnType;
        }

        /// <summary>Gets the URI parameter.</summary>
        /// <returns></returns>
        public ParameterInfo GetUriParameter()
        {
            if (this.boundParameters == null)
            {
                var method = GetEndpointMethod();

                this.boundParameters = method.GetParameters()
                    .Where(p => p.GetCustomAttribute<BodyBoundAttribute>() != null || p.GetCustomAttribute<UriBoundAttribute>() != null)
                    .Where(p => p.GetCustomAttribute<NoBindAttribute>() == null)
                    .ToArray();
            }

            this.uriParameter = boundParameters.FirstOrDefault(p => p.GetCustomAttribute<UriBoundAttribute>() != null);

            return this.uriParameter;
        }

        /// <summary>Gets the body parameter.</summary>
        /// <returns></returns>
        public ParameterInfo GetBodyParameter()
        {
            if (this.boundParameters == null)
            {
                var method = GetEndpointMethod();

                this.boundParameters = method.GetParameters()
                    .Where(p => p.GetCustomAttribute<BodyBoundAttribute>() != null || p.GetCustomAttribute<UriBoundAttribute>() != null)
                    .Where(p => p.GetCustomAttribute<NoBindAttribute>() == null)
                    .ToArray();
            }

            if (this.HttpMethod.In(StringComparison.InvariantCultureIgnoreCase, "POST", "PUT", "PATCH") == true)
            {
                this.bodyParameter = boundParameters.FirstOrDefault(p => p.GetCustomAttribute<BodyBoundAttribute>() != null);
            }

            return this.bodyParameter;
        }

        /// <summary>Gets the simple parameters.</summary>
        /// <returns></returns>
        public IDictionary<ParameterInfo, object> GetSimpleParameters()
        {
            if (this.simpleParameters == null)
            {
                var method = GetEndpointMethod();

                this.simpleParameters = method.GetParameters()
                    .Where(p => p.GetCustomAttribute<BodyBoundAttribute>() == null)
                    .Where(p => p.GetCustomAttribute<UriBoundAttribute>() == null)
                    .Where(p => p.GetCustomAttribute<NoBindAttribute>() == null)
                    .ToArray();
            }

            return this.simpleParameters
                .ToDictionary((p) => p, (p) => null as object);
        }
    }
}
