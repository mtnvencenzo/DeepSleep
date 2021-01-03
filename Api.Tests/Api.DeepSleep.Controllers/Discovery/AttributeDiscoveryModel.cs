namespace Api.DeepSleep.Controllers.Discovery
{
    /// <summary>
    /// 
    /// </summary>
    public class AttributeDiscoveryModel
    {
        /// <summary>Gets the value.</summary>
        /// <value>The value.</value>
        public string Value => "Test";

        /// <summary>Gets or sets the post value.</summary>
        /// <value>The post value.</value>
        public string PostValue { get; set; }

        /// <summary>Gets or sets the custom int.</summary>
        /// <value>The custom int.</value>
        public int? CustomInt { get; set; }
    }
}
