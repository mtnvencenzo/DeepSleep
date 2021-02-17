namespace Api.DeepSleep.Controllers.Binding
{
    using global::DeepSleep;
    using System.Collections.Generic;

    /// <summary>
    /// 
    /// </summary>
    public class ArrayController
    {
        /// <summary>is the list.</summary>
        /// <param name="request">The request.</param>
        /// <response code="201" />
        /// <response code="201" />
        /// <returns></returns>
        [ApiRoute(new[] { "POST" }, "binding/array/ilist")]
        public IList<SimpleRs> IList([InBody] IList<SimpleRs> request)
        {
            return request;
        }

        /// <summary>Lists the specified request.</summary>
        /// <param name="request">The request.</param>
        /// <returns></returns>
        [ApiRoute(new[] { "POST" }, "binding/array/list")]
        public List<SimpleRs> List([InBody] List<SimpleRs> request)
        {
            return request;
        }

        /// <summary>Arrays the specified request.</summary>
        /// <param name="request">The request.</param>
        /// <returns></returns>
        [ApiRoute(new[] { "POST" }, "binding/array/array")]
        public SimpleRs[] Array([InBody] SimpleRs[] request)
        {
            return request;
        }

        /// <summary>is the enumerable.</summary>
        /// <param name="request">The request.</param>
        /// <returns></returns>
        [ApiRoute(new[] { "POST" }, "binding/array/ienumerable")]
        public IEnumerable<SimpleRs> IEnumerable([InBody] IEnumerable<SimpleRs> request)
        {
            return request;
        }

        /// <summary>is the list request response.</summary>
        /// <param name="request">The request.</param>
        /// <returns></returns>
        [ApiRoute(new[] { "POST" }, "binding/array/icollection")]
        public ICollection<SimpleRs> IListRequestResponse([InBody] ICollection<SimpleRs> request)
        {
            return request;
        }
    }

    /// <summary>
    /// The Simple Rs
    /// </summary>
    /// <remarks>
    /// Description for the simple rs
    /// </remarks>
    public class SimpleRs
    {
        /// <summary>Gets or sets the value.</summary>
        /// <value>The value.</value>
        public string Value { get; set; }
    }
}
