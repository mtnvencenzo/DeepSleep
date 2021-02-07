namespace Api.DeepSleep.Web.WebApiCheck.Controllers
{
    using Microsoft.AspNetCore.Mvc;
    using System;

    /// <summary>
    /// This is the collection of enpoints that represent int32 uri values.
    /// </summary>
    /// <remarks>
    /// This is the description for the int32 paths in this collection
    /// </remarks>
    [ApiController]
    [Route("[controller]")]
    public class Int32EndpointsController : ControllerBase
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
        [HttpPost]
        [Route(template: "/int32/uri/model/no/doc/attributes", Name = "PostInt32UriModelNoDocAttributes")]
        public Int32ObjectModelRq PostInt32UriModelNoDocAttributes([FromQuery] Int32UriObjectModelRs request)
        {
            return new Int32ObjectModelRq
            {
                Int32Property = request.Int32Property,
                NullableInt32Property = request.NullableInt32Property,
                UInt32Property = request.UInt32Property,
                NullableUInt32Property = request.NullableUInt32Property
            };
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
