namespace DeepSleep.Validation
{
    using System.Reflection;
    using System.Threading.Tasks;

    /// <summary>
    /// 
    /// </summary>
    public interface IApiValidationInvoker
    {
        /// <summary>Invokes the method validation.</summary>
        /// <param name="method">The method.</param>
        /// <param name="context">The context.</param>
        /// <returns></returns>
        Task<bool> InvokeMethodValidation(MethodInfo method, ApiRequestContext context);

        /// <summary>Invokes the object validation.</summary>
        /// <param name="obj">The object.</param>
        /// <param name="context">The context.</param>
        /// <returns></returns>
        Task<bool> InvokeObjectValidation(object obj, ApiRequestContext context);
    }
}
