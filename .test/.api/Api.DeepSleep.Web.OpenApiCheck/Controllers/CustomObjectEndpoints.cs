namespace Api.DeepSleep.Web.OpenApiCheck.Controllers
{
    using global::DeepSleep;
    using global::DeepSleep.Configuration;
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Text.Json;
    using System.Threading.Tasks;

    /// <summary>
    /// 
    /// </summary>
    public class CustomObjectEndpoints
    {
        private readonly JsonMediaSerializerConfiguration jsonFormattingConfiguration;

        /// <summary>Initializes a new instance of the <see cref="CustomObjectEndpoints"/> class.</summary>
        /// <param name="jsonFormattingConfiguration">The json formatting configuration.</param>
        public CustomObjectEndpoints(JsonMediaSerializerConfiguration jsonFormattingConfiguration)
        {
            this.jsonFormattingConfiguration = jsonFormattingConfiguration;
        }

        /// <summary>Posts the basic object model no document attributes.</summary>
        /// <param name="request">The request.</param>
        /// <param name="uri">The URI.</param>
        /// <returns></returns>
        [ApiRoute(httpMethod: "POST", template: "/custom/object/deep/{id}/models")]
        public async Task<CustomObjectIdModel> PostBasicObjectModelNoDocAttributes([InBody] CustomObjectModel request, [InUri] CustomUriIdModel uri)
        {
            CustomObjectIdModel idModel = null;
            var settings = this.jsonFormattingConfiguration?.SerializerOptions;

            using (var stream = new MemoryStream())
            {
                await JsonSerializer.SerializeAsync(stream, request, settings).ConfigureAwait(false);

                stream.Seek(0, SeekOrigin.Begin);

                idModel = (CustomObjectIdModel)(await JsonSerializer.DeserializeAsync(stream, typeof(CustomObjectIdModel), settings).ConfigureAwait(false));
            }

            idModel.Id = uri.Id;
            return idModel;
        }

        /// <summary>
        /// 
        /// </summary>
        public class CustomObjectModel
        {
            /// <summary>Gets or sets the value.</summary>
            /// <value>The value.</value>
            public string Value { get; set; }

            /// <summary>Gets or sets the inherited.</summary>
            /// <value>The inherited.</value>
            public CustomObjectModel Inherited { get; set; }

            /// <summary>Gets or sets the inner models.</summary>
            /// <value>The inner models.</value>
            public IList<CustomObjectModel> InnerModels { get; set; }

            /// <summary>Gets or sets the keyed models.</summary>
            /// <value>The keyed models.</value>
            public IDictionary<string, CustomObjectModel> KeyedModels { get; set; }

            /// <summary>Gets or sets the second object.</summary>
            /// <value>The second object.</value>
            public SecondCustomObject SecondObject { get; set; }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <seealso cref="Api.DeepSleep.Web.OpenApiCheck.Controllers.CustomObjectEndpoints.CustomDescriptionModel" />
        public class CustomObjectIdModel : CustomObjectModel
        {
            /// <summary>Gets or sets the identifier.</summary>
            /// <value>The identifier.</value>
            public int Id { get; set; }
        }

        /// <summary>
        /// 
        /// </summary>
        public class CustomUriIdModel
        {
            /// <summary>Gets or sets the identifier.</summary>
            /// <value>The identifier.</value>
            public int Id { get; set; }
        }

        /// <summary>
        /// Second Object
        /// </summary>
        public class SecondCustomObject
        {
            /// <summary>Gets or sets the third object.</summary>
            /// <value>The third object.</value>
            public ThirdCustomObject ThirdObject { get; set; }

            /// <summary>Gets or sets the second object.</summary>
            /// <value>The second object.</value>
            public SecondCustomObject SecondObject { get; set; }
        }

        /// <summary>
        /// The third custom object
        /// </summary>
        public class ThirdCustomObject
        {
            /// <summary>Gets or sets the inherited.</summary>
            /// <value>The inherited.</value>
            public CustomObjectModel Inherited { get; set; }

            /// <summary>Gets or sets the inner models.</summary>
            /// <value>The inner models.</value>
            public IList<CustomObjectModel> InnerModels { get; set; }

            /// <summary>Gets or sets the keyed models.</summary>
            /// <value>The keyed models.</value>
            public IDictionary<string, CustomObjectModel> KeyedModels { get; set; }

            /// <summary>Gets or sets the keyed models.</summary>
            /// <value>The keyed models.</value>
            public IDictionary<string, ThirdCustomObject> KeyedThirdModels { get; set; }

            /// <summary>Gets or sets the keyed second models.</summary>
            /// <value>The keyed second models.</value>
            public IDictionary<string, SecondCustomObject> KeyedSecondModels { get; set; }

            /// <summary>Gets or sets the second object.</summary>
            /// <value>The second object.</value>
            public SecondCustomObject SecondObject { get; set; }

            /// <summary>Gets or sets the third object.</summary>
            /// <value>The third object.</value>
            public ThirdCustomObject ThirdObject { get; set; }

            /// <summary>Gets or sets the identifier model.</summary>
            /// <value>The identifier model.</value>
            public CustomObjectIdModel IdModel { get; set; }

            /// <summary>Gets or sets the model.</summary>
            /// <value>The model.</value>
            public CustomObjectModel Model { get; set; }
        }
    }
}
