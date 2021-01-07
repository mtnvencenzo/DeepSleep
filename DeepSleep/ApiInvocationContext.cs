namespace DeepSleep
{
    using System.Collections.Generic;
    using System.Reflection;
    using System.Text.Json.Serialization;

    /// <summary>
    /// 
    /// </summary>
    public class ApiInvocationContext
    {
        /// <summary>Gets or sets the controller.</summary>
        /// <value>The controller.</value>
        public object ControllerInstance { get; set; }

        /// <summary>Gets or sets the URI model.</summary>
        /// <value>The URI model.</value>
        public object UriModel { get; set; }

        /// <summary>Gets or sets the body model.</summary>
        /// <value>The body model.</value>
        public object BodyModel { get; set; }

        /// <summary>Represents simple parameters on the endpoint method.
        /// </summary>
        [JsonIgnore]
        public IDictionary<ParameterInfo, object> SimpleParameters { get; set; } = new Dictionary<ParameterInfo, object>();

        /// <summary>Modelses this instance.</summary>
        /// <returns></returns>
        public virtual IEnumerable<object> Models()
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
        public virtual IEnumerable<T> Models<T>()
        {
            if (UriModel != null)
            {
                if (UriModel.GetType() == typeof(T) || UriModel.GetType().IsSubclassOf(typeof(T)))
                    yield return (T)UriModel;
            }

            if (BodyModel != null)
            {
                if (BodyModel.GetType() == typeof(T) || BodyModel.GetType().IsSubclassOf(typeof(T)))
                    yield return (T)BodyModel;
            }

            yield break;
        }
    }
}
