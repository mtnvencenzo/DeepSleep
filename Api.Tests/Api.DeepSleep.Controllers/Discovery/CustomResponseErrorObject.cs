using System.Collections.Generic;

namespace Api.DeepSleep.Controllers.Discovery
{
    /// <summary>
    /// 
    /// </summary>
    public class CustomResponseErrorObject
    {
        /// <summary>Initializes a new instance of the <see cref="CustomResponseErrorObject"/> class.</summary>
        /// <param name="errors">The errors.</param>
        public CustomResponseErrorObject(IList<string> errors)
        {
            this.Errors = errors;
        }

        /// <summary>Gets the test.</summary>
        /// <value>The test.</value>
        public string Test => "Value";

        /// <summary>Gets or sets the errors.</summary>
        /// <value>The errors.</value>
        public IList<string> Errors { get; set; }
    }
}
