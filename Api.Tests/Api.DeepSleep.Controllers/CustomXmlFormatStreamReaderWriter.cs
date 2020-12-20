namespace Api.DeepSleep.Controllers
{
    using global::DeepSleep.Formatting.Formatters;
    using System.Collections.Generic;

    public class CustomXmlFormatStreamReaderWriter : XmlHttpFormatter
    {
        public override IList<string> ReadableMediaTypes => new string[] { "application/xml", "other/xml" };

        public override IList<string> WriteableMediaTypes => new string[] { "application/xml", "other/xml" };
    }
}
