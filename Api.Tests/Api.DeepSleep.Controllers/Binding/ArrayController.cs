namespace Api.DeepSleep.Controllers.Binding
{
    using global::DeepSleep;
    using System.Collections.Generic;

    public class ArrayController
    {
        [ApiRoute(new[] { "POST" }, "binding/array/ilist")]
        public IList<SimpleRs> IList([BodyBound] IList<SimpleRs> request)
        {
            return request;
        }

        [ApiRoute(new[] { "POST" }, "binding/array/list")]
        public List<SimpleRs> List([BodyBound] List<SimpleRs> request)
        {
            return request;
        }

        [ApiRoute(new[] { "POST" }, "binding/array/array")]
        public SimpleRs[] Array([BodyBound] SimpleRs[] request)
        {
            return request;
        }

        [ApiRoute(new[] { "POST" }, "binding/array/ienumerable")]
        public IEnumerable<SimpleRs> IEnumerable([BodyBound] IEnumerable<SimpleRs> request)
        {
            return request;
        }

        [ApiRoute(new[] { "POST" }, "binding/array/icollection")]
        public ICollection<SimpleRs> IListRequestResponse([BodyBound] ICollection<SimpleRs> request)
        {
            return request;
        }
    }

    public class SimpleRs
    {
        public string Value { get; set; }
    }
}
