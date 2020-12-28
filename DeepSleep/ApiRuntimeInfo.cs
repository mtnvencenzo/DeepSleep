namespace DeepSleep
{
    using System;
    using System.Collections.Generic;
    using System.Text.Json.Serialization;

    /// <summary>The API processing info.</summary>
    public class ApiRuntimeInfo
    {
        /// <summary>Gets or sets the exception.</summary>
        /// <value>The exception.</value>
        [JsonIgnore]
        public IList<Exception> Exceptions { get; internal set; } = new List<Exception>();

        /// <summary>Gets or sets the duration of the request.</summary>
        /// <value>The duration of the request.</value>
        public ApiRequestDuration Duration { get; internal set; } = new ApiRequestDuration();

        /// <summary>Gets the internals.</summary>
        /// <value>The internals.</value>
        internal ApiInternals Internals { get; } = new ApiInternals();
    }
}