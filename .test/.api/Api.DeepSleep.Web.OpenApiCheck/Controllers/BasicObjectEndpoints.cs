namespace Api.DeepSleep.Web.OpenApiCheck.Controllers
{
    using global::DeepSleep;
    using System;

    /// <summary>
    /// This is the collection of enpoints that represent a basic object.
    /// </summary>
    /// <remarks>
    /// This is the description for the enum paths in this collection
    /// </remarks>
    public class BasicObjectEndpoints
    {
        /// <summary>Posts the basic object model with no doc attributes.</summary>
        /// <param name="request">The request.</param>
        /// <param name="id">The route identifier.</param>
        /// <returns>THe basic object being returned</returns>
        /// <remarks>
        /// Offically posts the basic object model.
        /// Here's some documentation:
        /// <a href="http://www.google.com" />.
        /// </remarks>
        [ApiRoute(httpMethod: "POST", template: "/basic/object/model/no/doc/attributes/{id}")]
        public BasicObject PostBasicObjectModelNoDocAttributes([BodyBound] BasicObject request, int id)
        {
            return new BasicObject
            {
                DecimalObject = request.DecimalObject,
                DoubleObject = request.DoubleObject,
                EnumObject = request.EnumObject,
                FloatObject = request.FloatObject,
                GuidObject = request.GuidObject,
                Int32 = request.Int32,
                NullableEnumObject = request.NullableEnumObject,
                NullableGuidObject = request.NullableGuidObject,
                NullableInt32 = request.NullableInt32,
                StringObject = request.StringObject,
                UriObject = request.UriObject,
                ByteObject = request.ByteObject,
                SByteObject = request.SByteObject,
                ShortObject = request.ShortObject,
                TimeSpanObject = request.TimeSpanObject,
                BoolObject = request.BoolObject,
                DateTimeObject = request.DateTimeObject,
                DateTimeOffsetObject = request.DateTimeOffsetObject,
                NullableBoolObject = request.NullableBoolObject,
                NullableDateTimeObject = request.NullableDateTimeObject,
                NullableDateTimeOffsetObject = request.NullableDateTimeOffsetObject,
                LongObject = request.LongObject,
                CharObject = request.CharObject
            };
        }
    }

    #region Models and Tests

    /// <summary>
    /// The basic object
    /// </summary>
    public class BasicObject
    {
        /// <summary>Gets or sets the nullable int32.</summary>
        /// <value>The nullable int32.</value>
        public int? NullableInt32 { get; set; }

        /// <summary>Gets or sets the int32.</summary>
        /// <value>The int32.</value>
        public int Int32 { get; set; }

        /// <summary>Gets or sets the unique identifier object.</summary>
        /// <value>The unique identifier object.</value>
        public Guid GuidObject { get; set; }

        /// <summary>Gets or sets the nullable unique identifier object.</summary>
        /// <value>The nullable unique identifier object.</value>
        public Guid? NullableGuidObject { get; set; }

        /// <summary>Gets or sets the URI object.</summary>
        /// <value>The URI object.</value>
        public Uri UriObject { get; set; }

        /// <summary>Gets or sets the string object.</summary>
        /// <value>The string object.</value>
        public string StringObject { get; set; }

        /// <summary>Gets or sets the enum object.</summary>
        /// <value>The enum object.</value>
        public BasicEnum EnumObject { get; set; }

        /// <summary>Gets or sets the nullable enum object.</summary>
        /// <value>The nullable enum object.</value>
        public BasicEnum? NullableEnumObject { get; set; }

        /// <summary>Gets or sets the float object.</summary>
        /// <value>The float object.</value>
        public float FloatObject { get; set; }

        /// <summary>Gets or sets the decimal object.</summary>
        /// <value>The decimal object.</value>
        public decimal DecimalObject { get; set; }

        /// <summary>Gets or sets the double object.</summary>
        /// <value>The double object.</value>
        public double DoubleObject { get; set; }

        /// <summary>Gets or sets the short object.</summary>
        /// <value>The short object.</value>
        public short ShortObject { get; set; }

        /// <summary>Gets or sets the byte object.</summary>
        /// <value>The byte object.</value>
        public byte ByteObject { get; set; }

        /// <summary>Gets or sets the s byte object.</summary>
        /// <value>The s byte object.</value>
        public sbyte SByteObject { get; set; }

        /// <summary>Gets or sets the time span object.</summary>
        /// <value>The time span object.</value>
        public TimeSpan TimeSpanObject { get; set; }

        /// <summary>Gets or sets a value indicating whether [bool object].</summary>
        /// <value><c>true</c> if [bool object]; otherwise, <c>false</c>.</value>
        public bool BoolObject { get; set; }

        /// <summary>Gets or sets the nullable bool object.</summary>
        /// <value>The nullable bool object.</value>
        public bool? NullableBoolObject { get; set; }

        /// <summary>Gets or sets the date time object.</summary>
        /// <value>The date time object.</value>
        public DateTime DateTimeObject { get; set; }

        /// <summary>Gets or sets the nullable date time object.</summary>
        /// <value>The nullable date time object.</value>
        public DateTime? NullableDateTimeObject { get; set; }

        /// <summary>Gets or sets the date time offset object.</summary>
        /// <value>The date time offset object.</value>
        public DateTimeOffset DateTimeOffsetObject { get; set; }

        /// <summary>Gets or sets the nullable date time offset object.</summary>
        /// <value>The nullable date time offset object.</value>
        public DateTimeOffset? NullableDateTimeOffsetObject { get; set; }

        /// <summary>Gets or sets the long object.</summary>
        /// <value>The long object.</value>
        public long LongObject { get; set; }

        /// <summary>Gets or sets the character object.</summary>
        /// <value>The character object.</value>
        public char CharObject { get; set; }
    }

    /// <summary>
    /// The basic enum
    /// </summary>
    public enum BasicEnum
    {
        /// <summary>The none</summary>
        None = 0,

        /// <summary>The something1</summary>
        Something1 = 1,

        /// <summary>The something2</summary>
        Something2 = 2
    }

    #endregion
}
