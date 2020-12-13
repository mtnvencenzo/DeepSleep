namespace DeepSleep
{
    using System;

    /// <summary>
    /// 
    /// </summary>
    /// <seealso cref="DeepSleep.IApiRequestContextResolver" />
    public class DefaultApiRequestContextResolver : IApiRequestContextResolver
    {
        private ApiRequestContext _context;

        /// <summary>Gets the context.</summary>
        /// <returns></returns>
        public ApiRequestContext GetContext() => _context;

        /// <summary>Sets the context.</summary>
        /// <param name="context">The context.</param>
        /// <returns></returns>
        /// <exception cref="System.InvalidOperationException">Attempt to overwrite existing context not allowed</exception>
        public IApiRequestContextResolver SetContext(ApiRequestContext context)
        {
            if (_context != null)
                throw new InvalidOperationException("Attempt to overwrite existing context not allowed");

            _context = context;
            return this;
        }
    }
}
