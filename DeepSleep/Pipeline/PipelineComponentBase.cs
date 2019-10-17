namespace DeepSleep.Pipeline
{
    using Microsoft.Extensions.Logging;
    using System;

    /// <summary>
    /// 
    /// </summary>
    public abstract class PipelineComponentBase
    {
        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="serviceProvider"></param>
        /// <returns></returns>
        public virtual ILogger<T> GetLogger<T>(IServiceProvider serviceProvider)
        {
            ILogger<T> logger = null;

            if (serviceProvider != null)
            {
                try
                {
                    var loggerFactory = serviceProvider?.GetService(typeof(ILoggerFactory)) as ILoggerFactory;
                    logger = loggerFactory.CreateLogger<T>();
                }
                catch (Exception) { }
            }

            return logger;
        }
    }
}
