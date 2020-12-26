namespace DeepSleep
{
    using System;
    using System.Collections.Generic;
    using System.Reflection;

    /// <summary>
    /// 
    /// </summary>
    public class ApiInvocationContext
    {
        /// <summary>Gets or sets the controller.</summary>
        /// <value>The controller.</value>
        public object Controller { get; set; }

        /// <summary>Gets or sets the method.</summary>
        /// <value>The method.</value>
        public MethodInfo ControllerMethod { get; set; }

        /// <summary>Gets or sets the URI model.</summary>
        /// <value>The URI model.</value>
        public Type UriModelType { get; set; }

        /// <summary>Gets or sets the URI model.</summary>
        /// <value>The URI model.</value>
        public object UriModel { get; set; }

        /// <summary>Represents simple parameters on the endpoint method.
        /// </summary>
        public IDictionary<ParameterInfo, object> SimpleParameters { get; set; } = new Dictionary<ParameterInfo, object>();

        /// <summary>Gets or sets the body model.</summary>
        /// <value>The body model.</value>
        public Type BodyModelType { get; set; }

        /// <summary>Gets or sets the body model.</summary>
        /// <value>The body model.</value>
        public object BodyModel { get; set; }

        /// <summary>Modelses this instance.</summary>
        /// <returns></returns>
        public IEnumerable<object> Models()
        {
            if (UriModel != null)
                yield return UriModel;

            if (BodyModel != null)
                yield return BodyModel;

            yield break;
        }

        /// <summary>Modelses this instance.</summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public IEnumerable<T> Models<T>()
        {
            if (UriModelType != null)
            {
                if (UriModelType == typeof(T) || UriModelType.IsSubclassOf(typeof(T)))
                    yield return (T)UriModel;
            }

            if (BodyModelType != null)
            {
                if (BodyModelType == typeof(T) || BodyModelType.IsSubclassOf(typeof(T)))
                    yield return (T)BodyModel;
            }

            yield break;
        }
    }
}
