namespace DeepSleep
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Linq;
    using System.Reflection;
    using System.Threading.Tasks;

    /// <summary>
    /// 
    /// </summary>
    public class ApiEndpointLocation
    {
        private static readonly List<Type> SimpleBindableTypes = new List<Type>
        {
            typeof(string),
            typeof(char),
            typeof(char?),
            typeof(DateTime),
            typeof(DateTime?),
            typeof(DateTimeOffset),
            typeof(DateTimeOffset?),
            typeof(TimeSpan),
            typeof(TimeSpan?),
            typeof(short),
            typeof(short?),
            typeof(ushort),
            typeof(ushort?),
            typeof(int),
            typeof(int?),
            typeof(uint),
            typeof(uint?),
            typeof(long),
            typeof(long?),
            typeof(ulong),
            typeof(ulong?),
            typeof(byte),
            typeof(byte?),
            typeof(sbyte),
            typeof(sbyte?),
            typeof(double),
            typeof(double?),
            typeof(float),
            typeof(float?),
            typeof(decimal),
            typeof(decimal?),
            typeof(bool),
            typeof(bool?),
            typeof(Guid),
            typeof(Guid?)
        };
        private MethodInfo methodInfo;
        private ParameterInfo uriParameter;
        private ParameterInfo bodyParameter;
        private ParameterInfo[] complexParameters;
        private ParameterInfo[] simpleParameters;


        /// <summary>Gets or sets the controller.</summary>
        /// <value>The controller.</value>
        public Type Controller { get; set; }

        /// <summary>Gets or sets the endpoint.</summary>
        /// <value>The endpoint.</value>
        public string Endpoint { get; set; }

        /// <summary>Gets or sets the HTTP method.</summary>
        /// <value>The HTTP method.</value>
        public string HttpMethod { get; set; }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
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

        /// <summary>
        /// 
        /// </summary>
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

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public ParameterInfo GetUriParameter()
        {
            if (this.complexParameters == null)
            {
                var method = GetEndpointMethod();

                this.complexParameters = method.GetParameters()
                   .Where(p => p.ParameterType.IsPrimitive == false)
                   .Where(p => p.ParameterType.IsEnum == false)
                   .Where(p => p.ParameterType != typeof(byte[]))
                   .Where(p => SimpleBindableTypes.Contains(p.ParameterType) == false)
                   .ToArray();
            }

            this.uriParameter = complexParameters.FirstOrDefault(p => p.GetCustomAttribute<UriBoundAttribute>() != null);

            return this.uriParameter;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public ParameterInfo GetBodyParameter()
        {
            if (this.complexParameters == null)
            {
                var method = GetEndpointMethod();

                this.complexParameters = method.GetParameters()
                   .Where(p => p.ParameterType.IsPrimitive == false)
                   .Where(p => p.ParameterType.IsEnum == false)
                   .Where(p => p.ParameterType != typeof(byte[]))
                   .Where(p => SimpleBindableTypes.Contains(p.ParameterType) == false)
                   .ToArray();
            }

            if (this.HttpMethod.In(StringComparison.InvariantCultureIgnoreCase, "POST", "PUT", "PATCH") == true)
            {
                this.bodyParameter = complexParameters.FirstOrDefault(p => p.GetCustomAttribute<BodyBoundAttribute>() != null);
            }

            return this.bodyParameter;
        }

        /// <summary>
        /// 
        /// </summary>
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
