using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DeepSleep
{
    /// <summary>
    /// 
    /// </summary>
    /// <seealso cref="DeepSleep.IApiResponseMessageProcessorProvider" />
    public class DefaultApiResponseMessageProcessorProvider : IApiResponseMessageProcessorProvider
    {
        #region Constructors & Initialiation

        /// <summary>Initializes a new instance of the <see cref="DefaultApiResponseMessageProcessorProvider"/> class.</summary>
        /// <param name="serviceProvider">The service provider.</param>
        public DefaultApiResponseMessageProcessorProvider(IServiceProvider serviceProvider)
        {
            _processors = new List<Type>();

            if (serviceProvider != null)
            {
                ServiceProvider = serviceProvider;
            }
        }

        private List<Type> _processors;

        #endregion

        #region Public Properties

        /// <summary>Gets or sets the service provider.</summary>
        /// <value>The service provider.</value>
        public IServiceProvider ServiceProvider { get; set; }

        #endregion

        /// <summary>Gets the processors.</summary>
        /// <returns></returns>
        /// <exception cref="NullReferenceException">Api response message processor '{type.FullName}</exception>
        public IEnumerable<IApiResponseMessageProcessor> GetProcessors()
        {
            foreach (var type in _processors)
            {
                IApiResponseMessageProcessor processor = null;

                if (ServiceProvider != null)
                {
                    try
                    {
                        processor = ServiceProvider.GetService(type) as IApiResponseMessageProcessor;
                    }
                    catch (System.Exception) { }
                }


                if (processor == null)
                {
                    try
                    {
                        processor = Activator.CreateInstance(type) as IApiResponseMessageProcessor;
                    }
                    catch (System.Exception) { }
                }

                if (processor == null)
                {
                    throw new NullReferenceException($"Api response message processor '{type.FullName}' could not be retrieved and initialized or activated.");
                }

                yield return processor;
            }

            yield break;
        }

        /// <summary>Registers the invoker.</summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public IApiResponseMessageProcessorProvider RegisterProcessor<T>() where T : IApiResponseMessageProcessor, new()
        {
            _processors.Add(typeof(T));
            return this;
        }

        /// <summary>Registers the processor.</summary>
        /// <param name="processor">The processor.</param>
        /// <returns></returns>
        /// <exception cref="ArgumentOutOfRangeException">
        /// processor - Processor type must implement '{typeof(IApiResponseMessageProcessor).FullName}
        /// or
        /// processor - Processor type must contain a default constructor
        /// </exception>
        public IApiResponseMessageProcessorProvider RegisterProcessor(Type processor)
        {
            var processorType = processor.GetInterface(typeof(IApiResponseMessageProcessor).FullName);
            if (processorType == null)
            {
                throw new ArgumentOutOfRangeException(nameof(processor), $"Processor type must implement '{typeof(IApiResponseMessageProcessor).FullName}'");
            }

            var defaultConstructor = processorType.GetConstructor(new Type[] { });
            if (defaultConstructor == null || defaultConstructor.IsPublic == false)
            {
                throw new ArgumentOutOfRangeException(nameof(processor), $"Processor type must contain a default constructor");
            }

            _processors.Add(processor);
            return this;
        }

        /// <summary>Removes the processor.</summary>
        /// <param name="processor">The processor.</param>
        /// <returns></returns>
        public IApiResponseMessageProcessorProvider RemoveProcessor(Type processor)
        {
            _processors.RemoveAll(t => t.FullName == processor.FullName);
            return this;
        }

        /// <summary>Clears the processors.</summary>
        /// <returns></returns>
        public IApiResponseMessageProcessorProvider ClearProcessors()
        {
            _processors.Clear();
            return this;
        }
    }
}
