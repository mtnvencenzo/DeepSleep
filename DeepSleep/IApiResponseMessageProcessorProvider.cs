namespace DeepSleep
{
    using System;
    using System.Collections.Generic;

    /// <summary></summary>
    public interface IApiResponseMessageProcessorProvider
    {
        /// <summary>Gets the processors.</summary>
        /// <returns></returns>
        IEnumerable<IApiResponseMessageProcessor> GetProcessors();

        /// <summary>Registers the processor.</summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        IApiResponseMessageProcessorProvider RegisterProcessor<T>() where T : IApiResponseMessageProcessor, new();

        /// <summary>Registers the processor.</summary>
        /// <param name="invoker">The invoker.</param>
        /// <returns></returns>
        IApiResponseMessageProcessorProvider RegisterProcessor(Type invoker);

        /// <summary>Removes the processor.</summary>
        /// <param name="invoker">The invoker.</param>
        /// <returns></returns>
        IApiResponseMessageProcessorProvider RemoveProcessor(Type invoker);

        /// <summary>Clears the processors.</summary>
        /// <returns></returns>
        IApiResponseMessageProcessorProvider ClearProcessors();
    }
}