namespace Api.DeepSleep.Web.WebApiCheck.Controllers
{
    using Microsoft.AspNetCore.Mvc;
    using System;
    using System.Collections.Generic;

    /// <summary>
    /// This is the collection of enpoints that represent simple arrays.
    /// </summary>
    /// <remarks>
    /// This is the description for the simple array endpoints
    /// </remarks>
    public class SimpleArrayEndpointsController : ControllerBase
    {
        /// <summary>Simples the i list int array response.</summary>
        /// <param name="count">The count.</param>
        /// <returns>The number of ilist integers</returns>
        [HttpGet]
        [Route(template: "/simple/ilist/int/array/response", Name = "SimpleIListIntArrayResponse")]
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
        [HttpPost]
        [Route(template: "/simple/ilist/int/array/request", Name = "SimpleIListIntArrayRequest")]
        public IList<int> SimpleIListIntArrayRequest([FromBody] IList<int> ints)
        {
            return ints;
        }

        /// <summary>Simples the i enumerable int array response.</summary>
        /// <param name="count">The count.</param>
        /// <returns>The number of ienumerable integers</returns>
        [HttpGet]
        [Route(template: "/simple/ienumerable/int/array/response", Name = "SimpleIEnumerableIntArrayResponse")]
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
        [HttpPost]
        [Route(template: "/simple/ienumerable/int/array/request", Name = "SimpleIEnumerableIntArrayRequest")]
        public IEnumerable<int> SimpleIEnumerableIntArrayRequest([FromBody] IEnumerable<int> ints)
        {
            return ints;
        }

        /// <summary>Simples the array int array response.</summary>
        /// <param name="count">The count.</param>
        /// <returns>The number of array integers</returns>
        [HttpGet]
        [Route(template: "/simple/array/int/array/response", Name = "SimpleArrayIntArrayResponse")]
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
        [HttpPost]
        [Route(template: "/simple/array/int/array/request", Name = "SimpleArrayIntArrayRequest")]
        public int[] SimpleArrayIntArrayRequest([FromBody] int[] ints)
        {
            return ints;
        }

        /// <summary>Simples the array segment int array response.</summary>
        /// <param name="count">The count.</param>
        /// <returns>The number of arraysegment integers</returns>
        [HttpGet]
        [Route(template: "/simple/arraysegment/int/array/response", Name = "SimpleArraySegmentIntArrayResponse")]
        public ArraySegment<int> SimpleArraySegmentIntArrayResponse(int count)
        {
            var ints = new List<int>();

            for (int i = 0; i < count; i++)
            {
                ints.Add(i);
            }

            return ints.ToArray();
        }

        /// <summary>Simples the array segment int array request.</summary>
        /// <param name="ints">The ints.</param>
        /// <returns>The number of arraysegment integers</returns>
        [HttpPost]
        [Route(template: "/simple/arraysegment/int/array/request", Name = "SimpleArraySegmentIntArrayRequest")]
        public ArraySegment<int> SimpleArraySegmentIntArrayRequest([FromBody] ArraySegment<int> ints)
        {
            return ints;
        }
    }
}
