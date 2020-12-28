namespace DeepSleep
{
    using System.Diagnostics;

    /// <summary>
    /// 
    /// </summary>
    [DebuggerDisplay("{Code} {Message}")]
    public class ApiResultMessage
    {
        /// <summary>Gets or sets the code.</summary>
        /// <value>The code.</value>
        public string Code { get; set; }

        /// <summary>Gets or sets the message.</summary>
        /// <value>The message.</value>
        public string Message { get; set; }
    }
}