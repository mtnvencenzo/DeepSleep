using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace DeepSleep.Validation
{
    /// <summary>
    /// 
    /// </summary>
    public interface IApiValidationInvoker
    {
        /// <summary>Invokes the method validation.</summary>
        /// <param name="method">The method.</param>
        /// <param name="context">The context.</param>
        /// <param name="serviceProvider">The service provider.</param>
        /// <param name="responseMessageConverter">The response message converter.</param>
        /// <returns></returns>
        Task<bool> InvokeMethodValidation(MethodInfo method, ApiRequestContext context, IServiceProvider serviceProvider, IApiResponseMessageConverter responseMessageConverter);

        /// <summary>Invokes the object validation.</summary>
        /// <param name="obj">The object.</param>
        /// <param name="context">The context.</param>
        /// <param name="serviceProvider">The service provider.</param>
        /// <param name="responseMessageConverter">The response message converter.</param>
        /// <returns></returns>
        Task<bool> InvokeObjectValidation(object obj, ApiRequestContext context, IServiceProvider serviceProvider, IApiResponseMessageConverter responseMessageConverter);
    }
}
