namespace DeepSleep
{
    using System;
    using System.Diagnostics;
    using System.Linq;

    /// <summary>Represents a Multipart content disposition header</summary>
    /// <remarks>
    /// In [RFC2183] there is a discussion of the "Content-Disposition" header
    /// field and the description of the initial values allowed in this
    /// header.  Additional values may be registered with the IANA
    /// following the procedures in section 9 of [RFC2183].
    /// </remarks>
    [DebuggerDisplay("{ToString()}")]
    public class ContentDisposition : MediaHeaderValueWithParameters
    {
        /// <summary>Initializes a new instance of the <see cref="ContentDisposition"/> class.
        /// </summary>
        /// <param name="value">The value.</param>
        public ContentDisposition(string value)
            : base(value)
        {
        }

        /// <summary>Gets or sets the type.</summary>
        /// <value>The type.</value>
        public ContentDispositionType Type { get; set; }

        /// <summary>Gets or sets the name of the file.</summary>
        /// <value>The name of the file.</value>
        public string FileName
        {
            get
            {
                var fileNameParameter = base.MediaValue.Parameters.FirstOrDefault(p => p.Trim().StartsWith("filename="));

                if (fileNameParameter == null)
                {
                    return null;
                }

                return fileNameParameter
                    .Trim()
                    .Replace("filename=", string.Empty)
                    .Replace("\"", string.Empty)
                    .Trim();
            }
        }

        /// <summary>Gets or sets the name of the file.</summary>
        /// <value>The name of the file.</value>
        public string FileNameStar
        {
            get
            {
                var fileNameParameter = base.MediaValue.Parameters.FirstOrDefault(p => p.Trim().StartsWith("filename*="));

                if (fileNameParameter == null)
                {
                    return null;
                }

                return fileNameParameter
                    .Trim()
                    .Replace("filename*=", string.Empty)
                    .Replace("\"", string.Empty)
                    .Trim();
            }
        }

        /// <summary>Gets or sets the name.</summary>
        /// <value>The name.</value>
        public string Name
        {
            get
            {
                var fileNameParameter = base.MediaValue.Parameters.FirstOrDefault(p => p.Trim().StartsWith("name="));

                if (fileNameParameter == null)
                {
                    return null;
                }

                return fileNameParameter
                    .Trim()
                    .Replace("name=", string.Empty)
                    .Replace("\"", string.Empty)
                    .Trim();
            }
        }

        /// <summary>Gets or sets the size.</summary>
        /// <value>The size.</value>
        public long? Size { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public DateTimeOffset? CreationDate { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public DateTimeOffset? ModificationDate { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public DateTimeOffset? ReadDate { get; set; }
    }
}
