namespace Api.DeepSleep.Web.OpenApiCheck.Controllers
{
    using global::DeepSleep;
    using System;

    /// <summary>
    /// This is the collection of enpoints that represent uri bound enum values.
    /// </summary>
    /// <remarks>
    /// This is the description for the enum paths in this collection
    /// </remarks>
    public class EnumEndpoints
    {
        /// <summary>Posts the enum model with URI bound enums.</summary>
        /// <remarks>
        /// Offically posts the enum object to the service
        /// 
        /// Here's some documentation:
        /// <a href="http://www.google.com"/>.
        /// </remarks>
        /// <param name="request">The request.</param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        [ApiRoute(httpMethod: "POST", template: "/enum/uri/model/no/doc/attributes")]
        public EnumUriObjectModelRs PostEnumUriObjectNoDocAttributes([UriBound] EnumUriObjectModelRq request)
        {
            return new EnumUriObjectModelRs
            {
                TestEnumProperty = request.ExplicitEnumProperty
            };
        }
        
        /// <summary>Puts the enum model with URI bound enums.</summary>
        /// <remarks>
        /// Offically puts the enum object to the service
        /// 
        /// Here's some documentation:
        /// <a href="http://www.google.com"/>.
        /// </remarks>
        /// <param name="request">The request.</param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        [ApiRoute(httpMethod: "PUT", template: "/enum/uri/model/no/doc/attributes")]
        public EnumUriObjectModelRs PutEnumUriObjectNoDocAttributes([UriBound] EnumUriObjectModelRq request)
        {
            return new EnumUriObjectModelRs
            {
                TestEnumProperty = request.ExplicitEnumProperty
            };
        }

        /// <summary>Patch the enum model with URI bound enums.</summary>
        /// <param name="ExplicitEnumProperty">The explicit enum property.</param>
        /// <param name="NullableExplicitEnumProperty">The nullable explicit enum property.</param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        /// <exception cref="NotImplementedException"></exception>
        /// <remarks>
        /// Offically patches the enum object to the service
        /// Here's some documentation:
        /// <a href="http://www.google.com" />.
        /// </remarks>
        [ApiRoute(httpMethod: "PATCH", template: "/enum/uri/model/no/doc/attributes")]
        public EnumUriObjectModelRs PatchEnumUriObjectNoDocAttributes(TestEnum ExplicitEnumProperty, TestEnum? NullableExplicitEnumProperty)
        {
            return new EnumUriObjectModelRs
            {
                TestEnumProperty = NullableExplicitEnumProperty ?? ExplicitEnumProperty
            };
        }

        /// <summary>Gets the enum from route.</summary>
        /// <param name="enumValue">The enum value.</param>
        /// <returns></returns>
        /// <remarks>
        /// Offically posts the enum nul object to the service
        /// Here's some documentation:
        /// <a href="http://www.google.com" />.
        /// </remarks>
        [ApiRoute(httpMethod: "GET", template: "/enum/in/route/{enumValue}/simple/member")]
        public TestEnum GetEnumInRouteSimpleMemberNoDocAttributes(TestEnum? enumValue)
        {
            return enumValue ?? TestEnum.None;
        }
    }

    #region Models and Tests

    /// <summary>
    /// Represents a uri object model used in responses that contains enum items
    /// </summary>
    /// <remarks>
    /// Remarks about the EnumUriObjectModelRs object
    /// </remarks>
    public class EnumUriObjectModelRs
    {
        /// <summary>Gets or sets the test enum property.</summary>
        /// <value>The test enum property.</value>
        public TestEnum TestEnumProperty { get; set; }
    }

    /// <summary>
    /// Represents a uri object model used in requests that contains enum items
    /// </summary>
    /// <remarks>
    /// Remarks about the EnumUriObjectModelRq object
    /// </remarks>
    public class EnumUriObjectModelRq
    {
        /// <summary>Gets or sets the explicit enum property.</summary>
        /// <remarks>
        /// Remarks for the ExplicitEnumProperty property
        /// </remarks>
        /// <value>The explicit enum property.</value>
        public TestEnum ExplicitEnumProperty { get; set; }
    }

    /// <summary>
    /// Represents a bunch of items of something.
    /// </summary>
    /// <remarks>
    /// Remarks for TestEnum enum
    /// </remarks>
    public enum TestEnum
    {
        /// <summary>Represents the default value of the enum</summary>
        None = 0,

        /// <summary>The item 1 member</summary>
        Item1 = 1,

        /// <summary>The item 2 member</summary>
        Item2 = 2
    }

    #endregion
}
