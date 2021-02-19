namespace DeepSleep
{
    using System;
    using System.Threading.Tasks;

    /// <summary>
    /// 
    /// </summary>
    public interface IFormUrlEncodedObjectSerializer
    {
        /// <summary>Deserializes the specified data.</summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="data">The data.</param>
        /// <param name="urlDecoded">if set to <c>true</c> [URL decoded].</param>
        /// <returns></returns>
        Task<T> Deserialize<T>(string data, bool urlDecoded = false);

        /// <summary>Deserializes the specified data.</summary>
        /// <param name="data">The data.</param>
        /// <param name="objType">Type of the object.</param>
        /// <param name="urlDecoded">if set to <c>true</c> [URL decoded].</param>
        /// <returns></returns>
        Task<object> Deserialize(string data, Type objType, bool urlDecoded = false);
    }
}
