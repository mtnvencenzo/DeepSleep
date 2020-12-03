using System;
using System.Collections.Generic;
using System.Text;

namespace DeepSleep
{
    /// <summary>
    /// 
    /// </summary>
    public interface IServiceResolver : IServiceProvider
    {
        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        T GetService<T>();

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        IEnumerable<T> GetServices<T>();

        /// <summary>
        /// 
        /// </summary>
        /// <param name="serviceType"></param>
        /// <returns></returns>
        IEnumerable<object> GetServices(Type serviceType);
    }
}
