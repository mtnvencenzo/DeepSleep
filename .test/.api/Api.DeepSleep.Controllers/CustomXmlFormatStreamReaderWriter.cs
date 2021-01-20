namespace Api.DeepSleep.Controllers
{
    using global::DeepSleep.Formatting.Formatters;
    using System.Collections.Generic;

    /// <summary>
    /// 
    /// </summary>
    /// <seealso cref="global::DeepSleep.Formatting.Formatters.XmlHttpFormatter" />
    public class CustomXmlFormatStreamReaderWriter : XmlHttpFormatter
    {
        /// <summary>Gets the readable media types.</summary>
        /// <value>The readable media types.</value>
        public override IList<string> ReadableMediaTypes => new string[] { "application/xml", "other/xml" };

        /// <summary>Gets or sets the writeable media types.</summary>
        /// <value>The writeable media types.</value>
        public override IList<string> WriteableMediaTypes => new string[] { "application/xml", "other/xml" };
    }
}
