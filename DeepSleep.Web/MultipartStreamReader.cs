namespace DeepSleep.Web
{
    using Microsoft.AspNetCore.WebUtilities;
    using System.IO;
    using System.Text;
    using System.Threading.Tasks;

    /// <summary>
    /// 
    /// </summary>
    /// <seealso cref="DeepSleep.IMultipartStreamReader" />
    public class MultipartStreamReader : IMultipartStreamReader
    {
        private readonly IApiRequestContextResolver requestContextResolver;

        /// <summary>
        /// Initializes a new instance of the <see cref="MultipartStreamReader"/> class.
        /// </summary>
        /// <param name="requestContextResolver">The request context resolver.</param>
        public MultipartStreamReader(IApiRequestContextResolver requestContextResolver)
        {
            this.requestContextResolver = requestContextResolver;
        }

        /// <summary>Reads as multipart.</summary>
        /// <param name="body">The body.</param>
        /// <returns></returns>
        public async virtual Task<MultipartHttpRequest> ReadAsMultipart(Stream body)
        {
            if (body == null)
                return null;

            var context = this.requestContextResolver.GetContext();
            var boundary = context.Request.ContentType.Boundary;
            MultipartHttpRequest multipart = null;
            MultipartSection section;
            MultipartReader reader = new MultipartReader(boundary, body);

            while ((section = await reader.ReadNextSectionAsync(context.RequestAborted).ConfigureAwait(false)) != null)
            {
                if (multipart == null)
                {
                    multipart = new MultipartHttpRequest(boundary);
                }

                var multipartSection = new MultipartHttpRequestSection
                {
                    ContentType = section.ContentType,
                    ContentDisposition = section.ContentDisposition,
                };

                context.RegisterForDispose(multipartSection);

                if (!multipartSection.IsFileSection())
                {
                    using (var valueReader = new StreamReader(section.Body, Encoding.Default, bufferSize: 1024, detectEncodingFromByteOrderMarks: true, leaveOpen: true))
                    {
                        multipartSection.Value = await valueReader.ReadToEndAsync().ConfigureAwait(false);
                    }
                }
                else
                {
                    multipartSection.TempFileName = $"{Path.GetTempPath()}{Path.GetRandomFileName()}";

                    using (var targetStream = File.Create(multipartSection.TempFileName))
                    {
                        await section.Body.CopyToAsync(targetStream, 1024, context.RequestAborted).ConfigureAwait(false);
                    }
                }

                multipart.Sections.Add(multipartSection);
            }

            return multipart;
        }
    }
}
