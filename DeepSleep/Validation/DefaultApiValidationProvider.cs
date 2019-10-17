namespace DeepSleep.Validation
{
    using System;
    using System.Collections.Generic;

    /// <summary>
    /// 
    /// </summary>
    /// <seealso cref="DeepSleep.Validation.IApiValidationProvider" />
    public class DefaultApiValidationProvider : IApiValidationProvider
    {
        #region Constructors & Initialiation

        /// <summary>Initializes a new instance of the <see cref="DefaultApiValidationProvider" /> class.</summary>
        /// <param name="serviceProvider">The service provider.</param>
        public DefaultApiValidationProvider(IServiceProvider serviceProvider)
        {
            _invokers = new List<Type>();

            if (serviceProvider != null)
            {
                ServiceProvider = serviceProvider;
            }
        }

        private List<Type> _invokers;

        #endregion

        #region Public Properties

        /// <summary>Gets or sets the service provider.</summary>
        /// <value>The service provider.</value>
        public IServiceProvider ServiceProvider { get; set; }

        #endregion

        /// <summary>Gets the validation invokers.</summary>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public IEnumerable<IApiValidationInvoker> GetInvokers()
        {
            foreach (var type in _invokers)
            {
                IApiValidationInvoker invoker = null;

                if (ServiceProvider != null)
                {
                    try
                    {
                        invoker = ServiceProvider.GetService(type) as IApiValidationInvoker;
                    }
                    catch (System.Exception) { }
                }


                if (invoker == null)
                {
                    try
                    {
                        invoker = Activator.CreateInstance(type) as IApiValidationInvoker;
                    }
                    catch (System.Exception) { }
                }

                if (invoker == null)
                {
                    throw new NullReferenceException($"Api validation invoker '{type.FullName}' could not be retrieved and initialized or activated.");
                }

                yield return invoker;
            }

            yield break;
        }

        /// <summary>Registers the invoker.</summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public IApiValidationProvider RegisterInvoker<T>() where T : IApiValidationInvoker, new()
        {
            _invokers.Add(typeof(T));
            return this;
        }

        /// <summary>Registers the invoker.</summary>
        /// <param name="invoker"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentOutOfRangeException">
        /// invoker - Invoker type must implement '{typeof(IApiValidationInvoker).FullName}
        /// or
        /// invoker - Invoker type must contain a default constructor
        /// </exception>
        public IApiValidationProvider RegisterInvoker(Type invoker)
        {
            var invokerType = invoker.GetInterface(typeof(IApiValidationInvoker).FullName);
            if (invokerType == null)
            {
                throw new ArgumentOutOfRangeException(nameof(invoker), $"Invoker type must implement '{typeof(IApiValidationInvoker).FullName}'");
            }

            var defaultConstructor = invokerType.GetConstructor(new Type[] { });
            if (defaultConstructor == null || defaultConstructor.IsPublic == false)
            {
                throw new ArgumentOutOfRangeException(nameof(invoker), $"Invoker type must contain a default constructor");
            }

            _invokers.Add(invoker);
            return this;
        }

        /// <summary>Removes the invoker.</summary>
        /// <param name="invoker">The invoker.</param>
        /// <returns></returns>
        public IApiValidationProvider RemoveInvoker(Type invoker)
        {
            _invokers.RemoveAll(t => t.FullName == invoker.FullName);
            return this;
        }

        /// <summary>Clears the invokers.</summary>
        /// <returns></returns>
        public IApiValidationProvider ClearInvokers()
        {
            _invokers.Clear();
            return this;
        }
    }
}
