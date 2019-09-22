using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DeepSleep
{
    /// <summary>Represents a Multipart content disposition header</summary>
    /// <remarks>
    /// In [RFC2183] there is a discussion of the "Content-Disposition" header
    /// field and the description of the initial values allowed in this
    /// header.  Additional values may be registered with the IANA
    /// following the procedures in section 9 of [RFC2183].
    /// </remarks>
    public class ContentDisposition
    {
        /// <summary>Gets or sets the type.</summary>
        /// <value>The type.</value>
        public ContentDispositionType Type { get; set; }

        /// <summary>Gets or sets the name of the file.</summary>
        /// <value>The name of the file.</value>
        public string FileName { get; set; }

        /// <summary>Gets or sets the name.</summary>
        /// <value>The name.</value>
        public string Name { get; set; }

        /// <summary>Gets or sets the size.</summary>
        /// <value>The size.</value>
        public long? Size { get; set; }
    }
}
