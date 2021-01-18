namespace DeepSleep
{
    using System.Collections.Generic;

    /// <summary>
    /// The Multi part Http Request
    /// </summary>
    public class MultipartHttpRequest
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="MultipartHttpRequest"/> class.
        /// </summary>
        /// <param name="boundary">The boundary.</param>
        public MultipartHttpRequest(string boundary)
        {
            Boundary = boundary;
        }

        /// <summary>Gets the boundary.</summary>
        /// <value>The boundary.</value>
        public string Boundary { get; internal set; }

        /// <summary>Gets the sections.</summary>
        /// <value>The sections.</value>
        public IList<MultipartHttpRequestSection> Sections { get; } = new List<MultipartHttpRequestSection>();
    }
}
