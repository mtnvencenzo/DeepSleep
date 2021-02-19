namespace Api.DeepSleep.Web.OpenApiCheck.Controllers
{
    using global::DeepSleep;
    using System.Collections.Generic;
    using System.Linq;

    /// <summary>
    /// This is the collection of enpoints that represent simple array querystrings.
    /// </summary>
    /// <remarks>
    /// This is the description for the simple array endpoints with querystring arrays
    /// </remarks>
    public class SimpleArrayQueryStringEndpoints
    {
        /// <summary>Simples the i list int array query string.</summary>
        /// <param name="queryItems">The query items.</param>
        /// <returns>The count of items in the supplied query string array</returns>
        [ApiRoute(httpMethod: "GET", template: "/simple/ilist/int/array/querystring")]
        public int? SimpleIListIntArrayQueryString(IList<int> queryItems)
        {
            return queryItems?.Count;
        }

        /// <summary>Simples the i enumerable int array query string.</summary>
        /// <param name="queryItems">The query items.</param>
        /// <returns>The count of items in the supplied query string array</returns>
        [ApiRoute(httpMethod: "GET", template: "/simple/ienumerable/int/array/querystring")]
        public int? SimpleIEnumerableIntArrayQueryString(IEnumerable<int> queryItems)
        {
            return queryItems?.Count();
        }

        /// <summary>Simples the array int array query string.</summary>
        /// <param name="queryItems">The query items.</param>
        /// <returns>The count of items in the supplied query string array</returns>
        [ApiRoute(httpMethod: "GET", template: "/simple/array/int/array/querystring")]
        public int? SimpleArrayIntArrayQueryString(int[] queryItems)
        {
            return queryItems?.Length;
        }
    }
}
