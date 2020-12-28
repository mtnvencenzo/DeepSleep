namespace Api.DeepSleep.Controllers
{
    using global::DeepSleep;

    public class ContextDumpController
    {
        private readonly IApiRequestContextResolver apiRequestContextResolver;

        public ContextDumpController(IApiRequestContextResolver apiRequestContextResolver)
        {
            this.apiRequestContextResolver = apiRequestContextResolver;
        }

        /// <summary>Gets the with items.</summary>
        /// <returns></returns>
        public void GetDump()
        {
        }

        /// <summary>Gets the with items.</summary>
        /// <returns></returns>
        public ContextDump PostDump([BodyBound]ContextDump model)
        {
            return model;
        }
    }

    public class ContextDump
    {
        /// <summary>Gets or sets the value.</summary>
        /// <value>The value.</value>
        public string Value { get; set; }
    }
}
