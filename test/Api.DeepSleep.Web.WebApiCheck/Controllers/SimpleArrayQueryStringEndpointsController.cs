namespace Api.DeepSleep.Web.WebApiCheck.Controllers
{
    using Microsoft.AspNetCore.Mvc;
    using System;
    using System.Collections.Generic;

    /// <summary>
    /// This is the collection of enpoints that represent simple array querystrings.
    /// </summary>
    /// <remarks>
    /// This is the description for the simple array endpoints with querystring arrays
    /// </remarks>
    public class SimpleArrayQueryStringEndpointsController
    {
        /// <summary>Simples the i list int array query string.</summary>
        /// <param name="queryItems">The query items.</param>
        /// <returns>The count of items in the supplied query string array</returns>
        [HttpGet]
        [Route(template: "/simple/ilist/int/array/querystring", Name = "SimpleIListIntArrayQueryString")]
        public int? SimpleIListIntArrayQueryString(IList<int> queryItems)
        {
            return queryItems?.Count;
        }
    }
}
