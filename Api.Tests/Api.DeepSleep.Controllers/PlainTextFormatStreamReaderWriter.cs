namespace Api.DeepSleep.Controllers
{
    using global::DeepSleep.Formatting;
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Threading.Tasks;

    public class PlainTextFormatStreamReaderWriter : IFormatStreamReaderWriter
    {
        public IList<string> ReadableMediaTypes => new string[] { "text/plain" };

        public IList<string> WriteableMediaTypes => new string[] { "text/plain" };

        public bool SupportsPrettyPrint => false;

        public bool SupportsRead => true;

        public bool SupportsWrite => true;

        public async Task<object> ReadType(Stream stream, Type objType)
        {
            return await ReadType(stream, objType, null);
        }

        public async Task<object> ReadType(Stream stream, Type objType, IFormatStreamOptions options)
        {
            string obj = null;

            using (var reader = new StreamReader(stream, options?.Encoding, true, 1024))
            {
                obj = await reader.ReadToEndAsync().ConfigureAwait(false);
            }

            return obj;
        }

        public async Task<long> WriteType(Stream stream, object obj, Action<long> preWriteCallback = null)
        {
            return await WriteType(stream, obj, preWriteCallback);
        }

        public async Task<long> WriteType(Stream stream, object obj, IFormatStreamOptions options, Action<long> preWriteCallback = null)
        {
            long length = 0;

            if (obj != null)
            {
                using (var ms = new MemoryStream())
                using (var writer = new StreamWriter(ms, options?.Encoding, 1024))
                {
                    writer.Write(obj.ToString());
                    writer.Flush();
                    length = ms.Length;
                    ms.Seek(0, SeekOrigin.Begin);

                    preWriteCallback?.Invoke(length);

                    await ms.CopyToAsync(stream).ConfigureAwait(false);
                }
            }

            return length;
        }
    }
}
