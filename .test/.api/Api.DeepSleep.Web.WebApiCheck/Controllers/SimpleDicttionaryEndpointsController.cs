namespace Api.DeepSleep.Web.WebApiCheck.Controllers
{
    using Microsoft.AspNetCore.Mvc;
    using System;
    using System.Collections.Generic;

    /// <summary>
    /// This is the collection of enpoints that represent simple dictionaries.
    /// </summary>
    /// <remarks>
    /// This is the description for the simple dictionary endpoints
    /// </remarks>
    public class SimpleDicttionaryEndpointsController
    {
        /// <summary>Simples the i dictionary string string response.</summary>
        /// <param name="count">The count.</param>
        /// <returns></returns>
        [HttpGet]
        [Route(template: "/simple/idctionary/string/string/response", Name = "SimpleIDictionaryStringStringResponse")]
        public IDictionary<string, string> SimpleIDictionaryStringStringResponse(int count)
        {
            var dict = new Dictionary<string, string>();

            for (int i = 0; i < count; i++)
            {
                dict.Add(1.ToString(), i.ToString());
            }

            return dict;
        }

        /// <summary>Simples the i dictionary int string response.</summary>
        /// <param name="count">The count.</param>
        /// <returns></returns>
        [HttpGet]
        [Route(template: "/simple/idctionary/int/string/response", Name = "SimpleIDictionaryIntStringResponse")]
        public IDictionary<int, string> SimpleIDictionaryIntStringResponse(int count)
        {
            var dict = new Dictionary<int, string>();

            for (int i = 0; i < count; i++)
            {
                dict.Add(1, i.ToString());
            }

            return dict;
        }
    }
}
