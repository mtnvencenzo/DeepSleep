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
    [DebuggerDisplay("{Controller?.Name} {MethodInfo?.Name}")]
    public class ApiEndpointLocation
    {
        private ParameterInfo[] boundParameters;

        /// <summary>Initializes a new instance of the <see cref="ApiEndpointLocation" /> class.</summary>
        /// <param name="controller">The controller.</param>
        /// <param name="methodInfo">The method information.</param>
        /// <param name="httpMethod">The HTTP method.</param>
        public ApiEndpointLocation(
            Type controller,
            MethodInfo methodInfo,
            string httpMethod)
        {
            this.HttpMethod = httpMethod;

            if (controller != null)
            {
                this.Controller = Type.GetType(controller.AssemblyQualifiedName);
            }

            this.MethodInfo = this.ResolveMethodInfo(this.Controller, methodInfo);
            this.UriParameterType = this.GetUriParameterInfo()?.ParameterType;
            this.BodyParameterType = this.GetBodyParameterInfo()?.ParameterType;
            this.MethodReturnType = this.GetMethodInfoReturnType();
            this.SimpleParameters = this.GetSimpleParametersInfo();
        }

        /// <summary>Initializes a new instance of the <see cref="ApiEndpointLocation" /> class.</summary>
        /// <param name="controller">The controller.</param>
        /// <param name="methodInfo">The method information.</param>
        /// <param name="httpMethod">The HTTP method.</param>
        /// <param name="uriParameterType">The URI parameter type.</param>
        /// <param name="bodyParameterType">The body parameter.</param>
        /// <param name="simpleParameters">The simple parameters.</param>
        /// <param name="methodReturnType">Type of the method return.</param>
        internal ApiEndpointLocation(
            Type controller,
            MethodInfo methodInfo,
            string httpMethod,
            Type uriParameterType,
            Type bodyParameterType,
            IList<ParameterInfo> simpleParameters,
            Type methodReturnType)
        {
            this.Controller = controller;
            this.MethodInfo = methodInfo;
            this.HttpMethod = httpMethod;
            this.UriParameterType = uriParameterType;
            this.BodyParameterType = bodyParameterType;
            this.MethodReturnType = methodReturnType;
            this.SimpleParameters = simpleParameters;
        }

        /// <summary>Gets or sets the controller.</summary>
        /// <value>The controller.</value>
        [JsonIgnore]
        public Type Controller { get; }

        /// <summary>Gets or sets the HTTP method.</summary>
        /// <value>The HTTP method.</value>
        [JsonIgnore]
        public string HttpMethod { get; }

        /// <summary>Gets the method information.</summary>
        /// <value>The method information.</value>
        internal MethodInfo MethodInfo { get; }

        /// <summary>Gets the URI parameter.</summary>
        /// <value>The URI parameter.</value>
        internal Type UriParameterType { get; }

        /// <summary>Gets the body parameter.</summary>
        /// <value>The body parameter.</value>
        internal Type BodyParameterType { get; }

        /// <summary>Gets the simple parameters.</summary>
        /// <value>The simple parameters.</value>
        internal IList<ParameterInfo> SimpleParameters { get; }

        /// <summary>Gets the type of the method return.</summary>
        /// <value>The type of the method return.</value>
        internal Type MethodReturnType { get; }

        /// <summary>Gets the type of the endpoint return.</summary>
        /// <returns></returns>
        public Type GetMethodInfoReturnType()
        {
            if (this.MethodInfo != null)
            {
                if (this.MethodInfo.ReturnType.IsSubclassOf(typeof(Task)) && this.MethodInfo.ReturnType.IsGenericType)
                {
                    var type = this.MethodInfo.ReturnType.GenericTypeArguments[0];
                    return type;
                }

                return this.MethodInfo.ReturnType;
            }

            return null;
        }

        /// <summary>Gets the URI parameter.</summary>
        /// <returns></returns>
        public ParameterInfo GetUriParameterInfo()
        {
            if (this.MethodInfo != null)
            {
                if (this.boundParameters == null)
                {
                    this.boundParameters = this.MethodInfo.GetParameters()
                        .Where(p => p.GetCustomAttribute<InBodyAttribute>() != null || p.GetCustomAttribute<InUriAttribute>() != null)
                        .Where(p => p.GetCustomAttribute<NoBindAttribute>() == null)
                        .ToArray();
                }

                return this.boundParameters.FirstOrDefault(p => p.GetCustomAttribute<InUriAttribute>() != null);
            }

            return null;
        }

        /// <summary>Gets the body parameter.</summary>
        /// <returns></returns>
        public ParameterInfo GetBodyParameterInfo()
        {
            if (this.MethodInfo != null)
            {
                if (this.boundParameters == null)
                {
                    this.boundParameters = this.MethodInfo.GetParameters()
                        .Where(p => p.GetCustomAttribute<InBodyAttribute>() != null || p.GetCustomAttribute<InUriAttribute>() != null)
                        .Where(p => p.GetCustomAttribute<NoBindAttribute>() == null)
                        .ToArray();
                }

                if (this.HttpMethod.In(StringComparison.InvariantCultureIgnoreCase, "POST", "PUT", "PATCH") == true)
                {
                    return this.boundParameters.FirstOrDefault(p => p.GetCustomAttribute<InBodyAttribute>() != null);
                }
            }

            return null;
        }

        /// <summary>Gets the simple parameters.</summary>
        /// <returns></returns>
        public IList<ParameterInfo> GetSimpleParametersInfo()
        {
            if (this.MethodInfo != null)
            {
                return this.MethodInfo.GetParameters()
                    .Where(p => p.GetCustomAttribute<InBodyAttribute>() == null)
                    .Where(p => p.GetCustomAttribute<InUriAttribute>() == null)
                    .Where(p => p.GetCustomAttribute<NoBindAttribute>() == null)
                    .ToList();
            }

            return new List<ParameterInfo>();
        }

        /// <summary>Resolves the method information.</summary>
        /// <param name="controller">The controller.</param>
        /// <param name="methodInfo">The method information.</param>
        /// <returns></returns>
        internal MethodInfo ResolveMethodInfo(Type controller, MethodInfo methodInfo)
        {
            if (controller == null || methodInfo == null)
            {
                return null;
            }

            var parameterTypes = methodInfo
                .GetParameters()
                .Select(p => Type.GetType(p.ParameterType.AssemblyQualifiedName))
                .ToList();

            var methods = Controller
                .GetMethods(bindingAttr: BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.InvokeMethod)
                .Where(m => m.Name == methodInfo.Name)
                .Where(m => m.GetParameters().Length == parameterTypes.Count)
                .ToList();

            if (methods.Count <= 1)
            {
                return methods.FirstOrDefault();
            }

            foreach (var method in methods)
            {
                bool hasUnmatched = false;

                var methodParameterTypes = method
                   .GetParameters()
                   .Select(p => Type.GetType(p.ParameterType.AssemblyQualifiedName))
                   .ToList();

                for (int i = 0; i < parameterTypes.Count; i++)
                {
                    if (methodParameterTypes[i] != parameterTypes[i])
                    {
                        hasUnmatched = true;
                        break;
                    }
                }

                if (!hasUnmatched)
                {
                    return method;
                }
            }

            throw new Exception($"Method {methodInfo.Name}({parameterTypes.Count}) not found on controller {controller.FullName}");
        }
    }
}
