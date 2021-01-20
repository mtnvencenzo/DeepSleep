namespace Api.DeepSleep.Controllers.Formatters
{
    using global::DeepSleep;
    using System;
    using System.Collections.Generic;

    /// <summary>
    /// 
    /// </summary>
    public class MultipartController
    {
        /// <summary>Posts the specified data.</summary>
        /// <param name="data">The data.</param>
        /// <param name="simpleMultiPartParameter">The simple multi part parameter.</param>
        /// <param name="simpleMultiPartParameterNullableInt">The simple multi part parameter nullable int.</param>
        public void Post([BodyBound] MultipartHttpRequest data, string simpleMultiPartParameter, int? simpleMultiPartParameterNullableInt)
        {
        }

        /// <summary>Posts the custom.</summary>
        /// <param name="data">The data.</param>
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
