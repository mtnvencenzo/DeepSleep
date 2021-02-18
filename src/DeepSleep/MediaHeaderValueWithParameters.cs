using System.Diagnostics;

namespace DeepSleep
{
    /// <summary>
    /// 
    /// </summary>
    [DebuggerDisplay("{ToString()}")]
    public class MediaHeaderValueWithParameters
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="MediaHeaderValueWithParameters"/> class.
        /// </summary>
        /// <param name="value">The value.</param>
        public MediaHeaderValueWithParameters(string value)
        {
            Value = value;
            MediaValue = value.GetMediaHeaderWithParameters();
        }

        /// <summary>Gets the value.</summary>
        /// <value>The value.</value>
        public string Value { get; private set; }

        /// <summary>Gets the values.</summary>
        /// <value>The values.</value>
        protected MediaValueWithParameters MediaValue { get; private set; }
    }
}
