namespace DeepSleep
{
    using System;

    /// <summary>
    /// 
    /// </summary>
    /// <seealso cref="DeepSleep.IApiRequestContextResolver" />
    public class ApiRequestContextResolver : IApiRequestContextResolver
    {
        private ApiRequestContext context;

        /// <summary>Gets the context.</summary>
        /// <returns></returns>
        public ApiRequestContext GetContext() => this.context;

        /// <summary>Sets the context.</summary>
        /// <param name="context">The context.</param>
        /// <returns></returns>
        /// <exception cref="System.InvalidOperationException">Attempt to overwrite existing context not allowed</exception>
        public IApiRequestContextResolver SetContext(ApiRequestContext context)
        {
            if (this.context != null)
            {
                throw new InvalidOperationException("Attempt to overwrite existing context not allowed");
            }

            this.context = context;
            return this;
        }
    }
}
