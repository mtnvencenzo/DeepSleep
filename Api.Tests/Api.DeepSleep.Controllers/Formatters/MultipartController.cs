namespace Api.DeepSleep.Controllers.Formatters
{
    using global::DeepSleep;
    using System;
    using System.Collections.Generic;

    public class MultipartController
    {
        public void Post([BodyBound] MultipartHttpRequest data)
        {
        }

        public void PostCustom([BodyBound] CustomMultiPart data)
        {
        }
    }

    /// <summary>
    /// 
    /// </summary>
    public class CustomMultiPart
    {
        /// <summary>Gets or sets a date.</summary>
        /// <value>a date.</value>
        public DateTime ADate { get; set; }

        /// <summary>Gets or sets the files.</summary>
        /// <value>The files.</value>
        public IList<MultipartHttpRequestSection> Files { get; set; }

        /// <summary>Gets or sets the files.</summary>
        /// <value>The files.</value>
        public IList<MultipartHttpRequestSection> MoreFiles { get; set; }

        /// <summary>Gets or sets the files.</summary>
        /// <value>The files.</value>
        public IList<MultipartHttpRequestSection> SingleFile { get; set; }

    }
}
