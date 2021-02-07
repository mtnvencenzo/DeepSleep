namespace Api.DeepSleep.Web.OpenApiCheck.Controllers
{
    using global::DeepSleep;
    using System;
    using System.Collections.Generic;

    /// <summary>
    /// This is the collection of enpoints that represent simple arrays.
    /// </summary>
    /// <remarks>
    /// This is the description for the simple array endpoints
    /// </remarks>
    public class SimpleArrayEndpoints
    {
        /// <summary>Simples the i list int array response.</summary>
        /// <param name="count">The count.</param>
        /// <returns>The number of ilist integers</returns>
        [ApiRoute(httpMethod: "GET", template: "/simple/ilist/int/array/response")]
        public IList<int> SimpleIListIntArrayResponse(int count)
        {
            var ints = new List<int>();

            for (int i = 0; i < count; i++)
            {
                ints.Add(i);
            }

            return ints;
        }

        /// <summary>Simples the i list int array request.</summary>
        /// <param name="ints">The ints.</param>
        /// <returns>The number of ilist integers</returns>
        [ApiRoute(httpMethod: "POST", template: "/simple/ilist/int/array/request")]
        public IList<int> SimpleIListIntArrayRequest([BodyBound] IList<int> ints)
        {
            return ints;
        }

        /// <summary>Simples the i enumerable int array response.</summary>
        /// <param name="count">The count.</param>
        /// <returns>The number of ienumerable integers</returns>
        [ApiRoute(httpMethod: "GET", template: "/simple/ienumerable/int/array/response")]
        public IEnumerable<int> SimpleIEnumerableIntArrayResponse(int count)
        {
            var ints = new List<int>();

            for (int i = 0; i < count; i++)
            {
                ints.Add(i);
            }

            return ints;
        }

        /// <summary>Simples the i enumerable int array request.</summary>
        /// <param name="ints">The ints.</param>
        /// <returns>The number of ienumerable integers</returns>
        [ApiRoute(httpMethod: "POST", template: "/simple/ienumerable/int/array/request")]
        public IEnumerable<int> SimpleIEnumerableIntArrayRequest([BodyBound] IEnumerable<int> ints)
        {
            return ints;
        }

        /// <summary>Simples the array int array response.</summary>
        /// <param name="count">The count.</param>
        /// <returns>The number of array integers</returns>
        [ApiRoute(httpMethod: "GET", template: "/simple/array/int/array/response")]
        public int[] SimpleArrayIntArrayResponse(int count)
        {
            var ints = new List<int>();

            for (int i = 0; i < count; i++)
            {
                ints.Add(i);
            }

            return ints.ToArray();
        }

        /// <summary>Simples the array int array request.</summary>
        /// <param name="ints">The ints.</param>
        /// <returns>The number of array integers</returns>
        [ApiRoute(httpMethod: "POST", template: "/simple/array/int/array/request")]
        public int[] SimpleArrayIntArrayRequest([BodyBound] int[] ints)
        {
            return ints;
        }
    }
}
