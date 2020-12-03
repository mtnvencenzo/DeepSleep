namespace DeepSleep
{
    using System;
    using System.Threading.Tasks;

    /// <summary>
    /// 
    /// </summary>
    public interface IFormUrlEncodedObjectSerializer
    {
        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="data"></param>
        /// <param name="urlDecoded"></param>
        /// <returns></returns>
        Task<T> Deserialize<T>(string data, bool urlDecoded = false);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="data"></param>
        /// <param name="objType"></param>
        /// <param name="urlDecoded"></param>
        /// <returns></returns>
        Task<object> Deserialize(string data, Type objType, bool urlDecoded = false);
    }
}
