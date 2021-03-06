// <auto-generated>
// Code generated by Microsoft (R) AutoRest Code Generator.
// Changes may cause incorrect behavior and will be lost if the code is
// regenerated.
// </auto-generated>

namespace DeepSleep.Api.OpenApiCheckTests.v3.Models
{
    using Newtonsoft.Json;
    using System.Linq;

    public partial class SecondCustomObject
    {
        /// <summary>
        /// Initializes a new instance of the SecondCustomObject class.
        /// </summary>
        public SecondCustomObject()
        {
            CustomInit();
        }

        /// <summary>
        /// Initializes a new instance of the SecondCustomObject class.
        /// </summary>
        public SecondCustomObject(ThirdCustomObject thirdObject = default(ThirdCustomObject), SecondCustomObject secondObject = default(SecondCustomObject))
        {
            ThirdObject = thirdObject;
            SecondObject = secondObject;
            CustomInit();
        }

        /// <summary>
        /// An initialization method that performs custom operations like setting defaults
        /// </summary>
        partial void CustomInit();

        /// <summary>
        /// </summary>
        [JsonProperty(PropertyName = "thirdObject")]
        public ThirdCustomObject ThirdObject { get; set; }

        /// <summary>
        /// </summary>
        [JsonProperty(PropertyName = "secondObject")]
        public SecondCustomObject SecondObject { get; set; }

    }
}
