namespace Api.DeepSleep.Controllers
{
    using global::DeepSleep;

    /// <summary>
    /// 
    /// </summary>
    public class ContextDumpController
    {
        /// <summary>Gets the with items.</summary>
        /// <returns></returns>
        public void GetDump()
        {
        }

        /// <summary>Gets the with items.</summary>
        /// <param name="model">The model.</param>
        /// <returns></returns>
        public ContextDump PostDump([BodyBound] ContextDump model)
        {
            return model;
        }
    }

    /// <summary>
    /// 
    /// </summary>
    public class ContextDump
    {
        /// <summary>Gets or sets the value.</summary>
        /// <value>The value.</value>
        public string Value { get; set; }
    }
}
