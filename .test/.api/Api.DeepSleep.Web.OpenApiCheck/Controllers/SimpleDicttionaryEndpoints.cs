namespace Api.DeepSleep.Web.OpenApiCheck.Controllers
{
    using global::DeepSleep;
    using System;
    using System.Collections.Generic;

    /// <summary>
    /// This is the collection of enpoints that represent simple dictionaries.
    /// </summary>
    /// <remarks>
    /// This is the description for the simple dictionary endpoints
    /// </remarks>
    public class SimpleDicttionaryEndpoints
    {
        /// <summary>Simples the i dictionary string string response.</summary>
        /// <param name="count">The count.</param>
        /// <returns></returns>
        [ApiRoute(httpMethod: "GET", template: "/simple/idctionary/string/string/response")]
        public IDictionary<string, string> SimpleIDictionaryStringStringResponse(int count)
        {
            var dict = new Dictionary<string, string>();

            for (int i = 0; i < count; i++)
            {
                dict.Add(i.ToString(), i.ToString());
            }

            return dict;
        }

        /// <summary>Simples the i dictionary int string response.</summary>
        /// <param name="count">The count.</param>
        /// <returns></returns>
        [ApiRoute(httpMethod: "GET", template: "/simple/idctionary/int/string/response")]
        public IDictionary<int, string> SimpleIDictionaryIntStringResponse(int count)
        {
            var dict = new Dictionary<int, string>();

            for (int i = 0; i < count; i++)
            {
                dict.Add(i, i.ToString());
            }

            return dict;
        }

        /// <summary>Objects the i dictionary string dictionary object response.</summary>
        /// <param name="count">The count.</param>
        /// <returns>The Dictionary Object</returns>
        [ApiRoute(httpMethod: "GET", template: "/object/idctionary/string/dictionaryobject/response")]
        public IDictionary<string, DictionaryObject> ObjectIDictionaryStringDictionaryObjectResponse(int count)
        {
            var dict = new Dictionary<string, DictionaryObject>();

            for (int i = 0; i < count; i++)
            {
                dict.Add(i.ToString(), new DictionaryObject
                {
                    Id = i,
                    Items = new Dictionary<string, string>
                    {
                        { "Key1", "Value1" }
                    }
                });
            }

            return dict;
        }
    }

    /// <summary>
    /// THe disctionary object
    /// </summary>
    public class DictionaryObject
    {
        /// <summary>Gets or sets the identifier.</summary>
        /// <value>The identifier.</value>
        public int Id { get; set; }

        /// <summary>Gets or sets the items.</summary>
        /// <value>The items.</value>
        public Dictionary<string, string> Items { get; set; }
    }
}
