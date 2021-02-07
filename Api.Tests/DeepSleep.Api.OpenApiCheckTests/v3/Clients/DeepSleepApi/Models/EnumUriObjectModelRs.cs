// <auto-generated>
// Code generated by Microsoft (R) AutoRest Code Generator.
// Changes may cause incorrect behavior and will be lost if the code is
// regenerated.
// </auto-generated>

namespace DeepSleep.Api.OpenApiCheckTests.v3.Models
{
    using Newtonsoft.Json;
    using System.Linq;

    /// <summary>
    /// Represents a uri object model used in responses that contains enum
    /// items
    /// </summary>
    public partial class EnumUriObjectModelRs
    {
        /// <summary>
        /// Initializes a new instance of the EnumUriObjectModelRs class.
        /// </summary>
        public EnumUriObjectModelRs()
        {
            CustomInit();
        }

        /// <summary>
        /// Initializes a new instance of the EnumUriObjectModelRs class.
        /// </summary>
        /// <param name="testEnumProperty">Possible values include: 'None',
        /// 'Item1', 'Item2'</param>
        public EnumUriObjectModelRs(string testEnumProperty = default(string))
        {
            TestEnumProperty = testEnumProperty;
            CustomInit();
        }

        /// <summary>
        /// An initialization method that performs custom operations like setting defaults
        /// </summary>
        partial void CustomInit();

        /// <summary>
        /// Gets or sets possible values include: 'None', 'Item1', 'Item2'
        /// </summary>
        [JsonProperty(PropertyName = "testEnumProperty")]
        public string TestEnumProperty { get; set; }

    }
}
