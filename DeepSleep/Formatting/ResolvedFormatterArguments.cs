using System;

namespace DeepSleep.Formatting
{
    /// <summary>
    /// 
    /// </summary>
    public class ResolvedFormatterArguments
    {
        /// <summary>Initializes a new instance of the <see cref="ResolvedFormatterArguments"/> class.</summary>
        /// <param name="context">The context.</param>
        public ResolvedFormatterArguments(ApiRequestContext context)
        {
            this.Context = context;
        }

        /// <summary>Gets the context.</summary>
        /// <value>The context.</value>
        public ApiRequestContext Context { get; private set; }
    }
}
