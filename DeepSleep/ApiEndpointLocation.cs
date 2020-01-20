namespace DeepSleep
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;
    using System.Threading.Tasks;

    /// <summary>
    /// 
    /// </summary>
    public class ApiEndpointLocation
    {
        private static List<Type> NonBindableTypes = new List<Type>
        {
            typeof(string),
            typeof(DateTime),
            typeof(DateTime?),
            typeof(short),
            typeof(short?),
            typeof(int),
            typeof(int?),
            typeof(long),
            typeof(long?),
            typeof(ushort),
            typeof(ushort?),
            typeof(uint),
            typeof(uint?),
            typeof(ulong),
            typeof(ulong?),
            typeof(byte),
            typeof(byte?),
            typeof(byte[])
        };
        private MethodInfo methodInfo;
        private ParameterInfo uriParameter;
        private ParameterInfo bodyParameter;
        private ParameterInfo[] parameters;


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
            if (methodInfo == null)
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

                if (methodInfo == null)
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
            if (this.parameters == null)
            {
                var method = GetEndpointMethod();

                this.parameters = method.GetParameters()
                   .Where(p => p.ParameterType.IsPrimitive == false)
                   .Where(p => p.ParameterType.IsEnum == false)
                   .Where(p => NonBindableTypes.Contains(p.ParameterType) == false)
                   .ToArray();
            }

            this.uriParameter = parameters.FirstOrDefault(p => p.GetCustomAttribute<UriBoundAttribute>() != null);

            return this.uriParameter;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public ParameterInfo GetBodyParameter()
        {
            if (this.parameters == null)
            {
                var method = GetEndpointMethod();

                this.parameters = method.GetParameters()
                   .Where(p => p.ParameterType.IsPrimitive == false)
                   .Where(p => p.ParameterType.IsEnum == false)
                   .Where(p => NonBindableTypes.Contains(p.ParameterType) == false)
                   .ToArray();
            }

            if (this.HttpMethod.In(StringComparison.InvariantCultureIgnoreCase, "POST", "PUT", "PATCH") == true)
            {
                this.bodyParameter = parameters.FirstOrDefault(p => p.GetCustomAttribute<BodyBoundAttribute>() != null);
            }

            return this.bodyParameter;
        }
    }
}
