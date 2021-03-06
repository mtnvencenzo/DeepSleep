// <auto-generated>
// Code generated by Microsoft (R) AutoRest Code Generator.
// Changes may cause incorrect behavior and will be lost if the code is
// regenerated.
// </auto-generated>

namespace DeepSleep.Api.OpenApiCheckTests.v3.Models
{
    using Newtonsoft.Json;
    using System.Collections;
    using System.Collections.Generic;
    using System.Linq;

    /// <summary>
    /// THe disctionary object
    /// </summary>
    public partial class DictionaryObject
    {
        /// <summary>
        /// Initializes a new instance of the DictionaryObject class.
        /// </summary>
        public DictionaryObject()
        {
            CustomInit();
        }

        /// <summary>
        /// Initializes a new instance of the DictionaryObject class.
        /// </summary>
        /// <param name="id">Gets or sets the identifier.</param>
        public DictionaryObject(int? id = default(int?), IDictionary<string, string> items = default(IDictionary<string, string>))
        {
            Id = id;
            Items = items;
            CustomInit();
        }

        /// <summary>
        /// An initialization method that performs custom operations like setting defaults
        /// </summary>
        partial void CustomInit();

        /// <summary>
        /// Gets or sets the identifier.
        /// </summary>
        [JsonProperty(PropertyName = "id")]
        public int? Id { get; set; }

        /// <summary>
        /// </summary>
        [JsonProperty(PropertyName = "items")]
        public IDictionary<string, string> Items { get; set; }

    }
}
