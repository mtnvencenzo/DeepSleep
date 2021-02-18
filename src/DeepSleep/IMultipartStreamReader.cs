namespace DeepSleep
{
    using System.IO;
    using System.Threading.Tasks;

    /// <summary>
    /// 
    /// </summary>
    public interface IMultipartStreamReader
    {
        /// <summary>Reads as multipart.</summary>
        /// <param name="body">The body.</param>
        /// <returns></returns>
        Task<MultipartHttpRequest> ReadAsMultipart(Stream body);
    }
}
