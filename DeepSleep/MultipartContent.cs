namespace DeepSleep
{
    /// <summary>
    /// 
    /// </summary>
    public class MultipartContent
    {
        /// <summary>Gets or sets the content.</summary>
        /// <value>The content.</value>
        public object Content { get; set; }

        /// <summary>Gets or sets the type of the content.</summary>
        /// <value>The type of the content.</value>
        public string ContentType { get; set; }

        /// <summary>Gets or sets the disposition.</summary>
        /// <value>The disposition.</value>
        public ContentDisposition Disposition { get; set; }

        /// <summary>Gets or sets the length of the content.</summary>
        /// <value>The length of the content.</value>
        public long? ContentLength { get; set; }

        /// <summary>Gets or sets the description.</summary>
        /// <value>The description.</value>
        public string Description { get; set; }
    }
}
