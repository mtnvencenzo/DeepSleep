using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DeepSleep.Validation
{
    /// <summary></summary>
    public interface IApiValidationProvider
    {

        /// <summary>Gets the validation invokers.</summary>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        IEnumerable<IApiValidationInvoker> GetInvokers();

        /// <summary>Registers the invoker.</summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        IApiValidationProvider RegisterInvoker<T>() where T : IApiValidationInvoker, new();

        /// <summary>Registers the invoker.</summary>
        /// <param name="invoker">The invoker.</param>
        /// <returns></returns>
        IApiValidationProvider RegisterInvoker(Type invoker);

        /// <summary>Removes the invoker.</summary>
        /// <param name="invoker">The invoker.</param>
        /// <returns></returns>
        IApiValidationProvider RemoveInvoker(Type invoker);

        /// <summary>Clears the invokers.</summary>
        /// <returns></returns>
        IApiValidationProvider ClearInvokers();
    }
}