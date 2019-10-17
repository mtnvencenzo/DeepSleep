namespace DeepSleep
{
    using System;

    /// <summary>
    /// 
    /// </summary>
    /// <seealso cref="DeepSleep.IApiRequestContextResolver" />
    public class DefaultApiRequestContextResolver : IApiRequestContextResolver
    {
        #region Constructors & Initialization

        private ApiRequestContext _context;

        #endregion

        /// <summary>Gets the context.</summary>
        /// <returns></returns>
        public ApiRequestContext GetContext() => _context;

        /// <summary>
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        public IApiRequestContextResolver SetContext(ApiRequestContext context)
        {
            if (_context != null)
                throw new InvalidOperationException("Attempt to overwrite existing context not allowed");

            _context = context;
            return this;
        }
    }
}
