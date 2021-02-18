namespace DeepSleep
{
    using System;
    using System.IO;

    /// <summary>
    /// Multipart Http Request Section
    /// </summary>
    public class MultipartHttpRequestSection : IDisposable
    {
        private bool disposedValue;

        /// <summary>Gets the type of the content.</summary>
        /// <value>The type of the content.</value>
        public virtual ContentTypeHeader ContentType { get; set; }

        /// <summary>Gets the content disposition.</summary>
        /// <value>The content disposition.</value>
        public virtual ContentDispositionHeader ContentDisposition { get; set; }

        /// <summary>Gets the name.</summary>
        /// <value>The name.</value>
        public virtual string Name => ContentDisposition?.Name ?? string.Empty;

        /// <summary>Gets the value.</summary>
        /// <value>The value.</value>
        public virtual string Value { get; set; }

        /// <summary>Gets the stream.</summary>
        /// <returns></returns>
        public virtual Stream GetStream() => this.TempFileName != null ? new FileStream(this.TempFileName, FileMode.Open, FileAccess.Read, FileShare.Read | FileShare.Delete) : null;

        /// <summary>Gets or sets the name of the temporary file.</summary>
        /// <value>The name of the temporary file.</value>
        public string TempFileName { get; set; }

        /// <summary>Releases unmanaged and - optionally - managed resources.</summary>
        /// <param name="disposing"><c>true</c> to release both managed and unmanaged resources; <c>false</c> to release only unmanaged resources.</param>
        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    if (!string.IsNullOrWhiteSpace(this.TempFileName) && File.Exists(this.TempFileName))
                    {
                        File.Delete(this.TempFileName);
                    }
                }

                disposedValue = true;
            }
        }

        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        public void Dispose()
        {
            // Do not change this code. Put cleanup code in 'Dispose(bool disposing)' method
            Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }
    }

    /// <summary>
    /// 
    /// </summary>
    public static class MultipartHttpRequestSectionExtensionMethods
    {
        /// <summary>
        /// Determines whether [is file section].
        /// </summary>
        /// <param name="section">The section.</param>
        /// <returns>
        ///   <c>true</c> if [is file section] [the specified section]; otherwise, <c>false</c>.
        /// </returns>
        public static bool IsFileSection(this MultipartHttpRequestSection section)
        {
            if (section == null)
            {
                return false;
            }

            if (string.IsNullOrWhiteSpace(section.ContentDisposition?.Value))
            {
                return false;
            }

            if (!string.IsNullOrWhiteSpace(section.ContentDisposition.FileName))
            {
                return true;
            }

            if (!string.IsNullOrWhiteSpace(section.ContentDisposition.FileNameStar))
            {
                return true;
            }

            if (string.IsNullOrWhiteSpace(section.ContentType?.ToString()))
            {
                return false;
            }

            return false;
        }
    }
}
