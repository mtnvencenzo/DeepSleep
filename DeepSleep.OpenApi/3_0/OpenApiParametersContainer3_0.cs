namespace DeepSleep.OpenApi.v3_0
{
    using System.Collections.Generic;
    using System.Text.Json.Serialization;

    /// <summary>
    /// 
    /// </summary>
    public class OpenApiParametersContainer3_0
    {
        /// <summary>Gets or sets the extensions.</summary>
        /// <value>The extensions.</value>
        [JsonExtensionData]
        public Dictionary<string, object> parameters { get; set; }

        /// <summary>Adds the parameter.</summary>
        /// <param name="paramName">Name of the parameter.</param>
        /// <param name="parameter">The parameter.</param>
        public void AddParameter(string paramName, OpenApiParameter3_0 parameter)
        {
            if (parameters == null)
            {
                parameters = new Dictionary<string, object>();
            }

            parameters.Add(paramName, parameter);
        }
    }
}
