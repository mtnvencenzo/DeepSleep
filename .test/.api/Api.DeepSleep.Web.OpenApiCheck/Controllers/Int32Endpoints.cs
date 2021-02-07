namespace Api.DeepSleep.Web.OpenApiCheck.Controllers
{
    using global::DeepSleep;
    using global::DeepSleep.OpenApi.Decorators;
    using System;

    /// <summary>
    /// This is the collection of enpoints that represent int32 uri values.
    /// </summary>
    /// <remarks>
    /// This is the description for the int32 paths in this collection
    /// </remarks>
    public class Int32Endpoints
    {
        /// <summary>Posts the int32 model with URI bound values.</summary>
        /// <remarks>
        /// Offically posts the int32 uri to the service
        /// 
        /// Here's some documentation:
        /// <a href="http://www.google.com"/>.
        /// </remarks>
        /// <param name="request">The request.</param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        [ApiRoute(httpMethod: "POST", template: "/int32/uri/model/no/doc/attributes")]
        public Int32ObjectModelRq PostInt32UriModelNoDocAttributes([UriBound] Int32UriObjectModelRs request)
        {
            return new Int32ObjectModelRq
            {
                Int32Property = request.Int32Property,
                NullableInt32Property = request.NullableInt32Property,
                UInt32Property = request.UInt32Property,
                NullableUInt32Property = request.NullableUInt32Property
            };
        }

        /// <summary>Gets the int32 combined route and query values.</summary>
        /// <remarks>
        /// Offically gets the combined int32 values
        /// </remarks>
        /// <param name="routeInt">The route int.</param>
        /// <param name="queryInt1">The query int1.</param>
        /// <param name="queryInt2">The query int2.</param>
        /// <returns>The combines integer</returns>
        [ApiRoute(httpMethod: "GET", template: "/int32/uri/{routeint}/value")]
        [OasApiOperation(operationId: "GetInt32ValuesOverriddenOpId", tags: new string[] { "Ints", "AndMoreInts" })]
        public int GetInt32Values(int routeInt, int? queryInt1, uint queryInt2)
        {
            return Convert.ToInt32(routeInt + (queryInt1 ?? 0) + queryInt2);
        }

        /// <summary>Adding overload to test that this doesn't get called.</summary>
        /// <param name="item">The item.</param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public int GetInt32Values(int item)
        {
            throw new NotImplementedException();
        }

        /// <summary>Adding overload to test that this doesn't get called.</summary>
        /// <param name="routeInt">The route int.</param>
        /// <param name="queryInt1">The query int1.</param>
        /// <param name="queryInt2">The query int2.</param>
        /// <param name="extra">The extra.</param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public int GetInt32Values(int routeInt, int? queryInt1, uint queryInt2, string extra)
        {
            throw new NotImplementedException();
        }
    }

    #region Models and Tests

    /// <summary>
    /// Represents a uri object model used in responses that contains int32 items
    /// </summary>
    /// <remarks>
    /// Remarks about the Int32UriObjectModelRs object
    /// </remarks>
    public class Int32UriObjectModelRs
    {
        /// <summary>Gets or sets the int32 property.</summary>
        /// <value>The int32 property.</value>
        public int Int32Property { get; set; }

        /// <summary>Gets or sets the nullable int32 property.</summary>
        /// <value>The nullable int32 property.</value>
        public int? NullableInt32Property { get; set; }

        /// <summary>Gets or sets the u int32 property.</summary>
        /// <value>The u int32 property.</value>
        public uint UInt32Property { get; set; }

        /// <summary>Gets or sets the nullable u int32 property.</summary>
        /// <value>The nullable u int32 property.</value>
        public uint? NullableUInt32Property { get; set; }
    }

    /// <summary>
    /// Represents a uri object model used in requests that contains int32 items
    /// </summary>
    /// <remarks>
    /// Remarks about the Int32ObjectModelRq object
    /// </remarks>
    public class Int32ObjectModelRq
    {
        /// <summary>Gets or sets the int32 property.</summary>
        /// <value>The int32 property.</value>
        public int Int32Property { get; set; }

        /// <summary>Gets or sets the nullable int32 property.</summary>
        /// <value>The nullable int32 property.</value>
        public int? NullableInt32Property { get; set; }

        /// <summary>Gets or sets the u int32 property.</summary>
        /// <value>The u int32 property.</value>
        public uint UInt32Property { get; set; }

        /// <summary>Gets or sets the nullable u int32 property.</summary>
        /// <value>The nullable u int32 property.</value>
        public uint? NullableUInt32Property { get; set; }
    }

    #endregion
}
