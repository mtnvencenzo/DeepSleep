namespace Api.DeepSleep.Controllers.Binding
{
    using global::DeepSleep;
    using System;
    using System.Collections.Generic;

    /// <summary>
    /// Simple Url Binding Controller
    /// </summary>
    public class SimpleUrlBindingController
    {
        /// <summary>Gets the with query.</summary>
        /// <param name="stringVar">The string variable.</param>
        /// <param name="charVar">The character variable.</param>
        /// <param name="nullCharVar">The null character variable.</param>
        /// <param name="int16Var">The int16 variable.</param>
        /// <param name="uInt16Var">The u int16 variable.</param>
        /// <param name="nullInt16Var">The null int16 variable.</param>
        /// <param name="nullUInt16Var">The null u int16 variable.</param>
        /// <param name="int32Var">The int32 variable.</param>
        /// <param name="uInt32Var">The u int32 variable.</param>
        /// <param name="nullInt32Var">The null int32 variable.</param>
        /// <param name="nullUInt32Var">The null u int32 variable.</param>
        /// <param name="int64Var">The int64 variable.</param>
        /// <param name="uInt64Var">The u int64 variable.</param>
        /// <param name="nullInt64Var">The null int64 variable.</param>
        /// <param name="nullUInt64Var">The null u int64 variable.</param>
        /// <param name="doubleVar">The double variable.</param>
        /// <param name="nullDoubleVar">The null double variable.</param>
        /// <param name="decimalVar">The decimal variable.</param>
        /// <param name="nullDecimalVar">The null decimal variable.</param>
        /// <param name="floatVar">The float variable.</param>
        /// <param name="nullFloatVar">The null float variable.</param>
        /// <param name="boolVar">if set to <c>true</c> [bool variable].</param>
        /// <param name="nullBoolVar">The null bool variable.</param>
        /// <param name="dateTimeVar">The date time variable.</param>
        /// <param name="nullDateTimeVar">The null date time variable.</param>
        /// <param name="dateTimeOffsetVar">The date time offset variable.</param>
        /// <param name="nullDateTimeOffsetVar">The null date time offset variable.</param>
        /// <param name="timeSpanVar">The time span variable.</param>
        /// <param name="nullTimeSpanVar">The null time span variable.</param>
        /// <param name="byteVar">The byte variable.</param>
        /// <param name="nullByteVar">The null byte variable.</param>
        /// <param name="sByteVar">The s byte variable.</param>
        /// <param name="nullSByteVar">The null s byte variable.</param>
        /// <param name="guidVar">The unique identifier variable.</param>
        /// <param name="nullGuidVar">The null unique identifier variable.</param>
        /// <param name="enumVar">The enum variable.</param>
        /// <param name="nullEnumVar">The null enum variable.</param>
        /// <returns></returns>
        public SimpleUrlBindingRs GetWithQuery(
            string stringVar,
            char charVar,
            char? nullCharVar,
            short int16Var,
            ushort uInt16Var,
            short? nullInt16Var,
            ushort? nullUInt16Var,
            int int32Var,
            uint uInt32Var,
            int? nullInt32Var,
            uint? nullUInt32Var,
            long int64Var,
            ulong uInt64Var,
            long? nullInt64Var,
            ulong? nullUInt64Var,
            double doubleVar,
            double? nullDoubleVar,
            decimal decimalVar,
            decimal? nullDecimalVar,
            float floatVar,
            float? nullFloatVar,
            bool boolVar,
            bool? nullBoolVar,
            DateTime dateTimeVar,
            DateTime? nullDateTimeVar,
            DateTimeOffset dateTimeOffsetVar,
            DateTimeOffset? nullDateTimeOffsetVar,
            TimeSpan timeSpanVar,
            TimeSpan? nullTimeSpanVar,
            byte byteVar,
            byte? nullByteVar,
            sbyte sByteVar,
            sbyte? nullSByteVar,
            Guid guidVar,
            Guid? nullGuidVar,
            SimpleUrlBindingEnum enumVar,
            SimpleUrlBindingEnum? nullEnumVar)
        {
            return new SimpleUrlBindingRs
            {
                StringVar = stringVar,
                CharVar = charVar,
                NullCharVar = nullCharVar,
                Int16Var = int16Var,
                UInt16Var = uInt16Var,
                NullInt16Var = nullInt16Var,
                NullUInt16Var = nullUInt16Var,
                Int32Var = int32Var,
                UInt32Var = uInt32Var,
                NullInt32Var = nullInt32Var,
                NullUInt32Var = nullUInt32Var,
                Int64Var = int64Var,
                UInt64Var = uInt64Var,
                NullInt64Var = nullInt64Var,
                NullUInt64Var = nullUInt64Var,
                DoubleVar = doubleVar,
                NullDoubleVar = nullDoubleVar,
                DecimalVar = decimalVar,
                NullDecimalVar = nullDecimalVar,
                FloatVar = floatVar,
                NullFloatVar = nullFloatVar,
                BoolVar = boolVar,
                NullBoolVar = nullBoolVar,
                DateTimeVar = dateTimeVar,
                NullDateTimeVar = nullDateTimeVar,
                DateTimeOffsetVar = dateTimeOffsetVar,
                NullDateTimeOffsetVar = nullDateTimeOffsetVar,
                TimeSpanVar = timeSpanVar,
                NullTimeSpanVar = nullTimeSpanVar,
                ByteVar = byteVar,
                NullByteVar = nullByteVar,
                SByteVar = sByteVar,
                NullSByteVar = nullSByteVar,
                GuidVar = guidVar,
                NullGuidVar = nullGuidVar,
                EnumVar = enumVar,
                NullEnumVar = nullEnumVar
            };
        }

        /// <summary>Gets the with route.</summary>
        /// <param name="stringVar">The string variable.</param>
        /// <param name="charVar">The character variable.</param>
        /// <param name="nullCharVar">The null character variable.</param>
        /// <param name="int16Var">The int16 variable.</param>
        /// <param name="uInt16Var">The u int16 variable.</param>
        /// <param name="nullInt16Var">The null int16 variable.</param>
        /// <param name="nullUInt16Var">The null u int16 variable.</param>
        /// <param name="int32Var">The int32 variable.</param>
        /// <param name="uInt32Var">The u int32 variable.</param>
        /// <param name="nullInt32Var">The null int32 variable.</param>
        /// <param name="nullUInt32Var">The null u int32 variable.</param>
        /// <param name="int64Var">The int64 variable.</param>
        /// <param name="uInt64Var">The u int64 variable.</param>
        /// <param name="nullInt64Var">The null int64 variable.</param>
        /// <param name="nullUInt64Var">The null u int64 variable.</param>
        /// <param name="doubleVar">The double variable.</param>
        /// <param name="nullDoubleVar">The null double variable.</param>
        /// <param name="decimalVar">The decimal variable.</param>
        /// <param name="nullDecimalVar">The null decimal variable.</param>
        /// <param name="floatVar">The float variable.</param>
        /// <param name="nullFloatVar">The null float variable.</param>
        /// <param name="boolVar">if set to <c>true</c> [bool variable].</param>
        /// <param name="nullBoolVar">The null bool variable.</param>
        /// <param name="dateTimeVar">The date time variable.</param>
        /// <param name="nullDateTimeVar">The null date time variable.</param>
        /// <param name="dateTimeOffsetVar">The date time offset variable.</param>
        /// <param name="nullDateTimeOffsetVar">The null date time offset variable.</param>
        /// <param name="timeSpanVar">The time span variable.</param>
        /// <param name="nullTimeSpanVar">The null time span variable.</param>
        /// <param name="byteVar">The byte variable.</param>
        /// <param name="nullByteVar">The null byte variable.</param>
        /// <param name="sByteVar">The s byte variable.</param>
        /// <param name="nullSByteVar">The null s byte variable.</param>
        /// <param name="guidVar">The unique identifier variable.</param>
        /// <param name="nullGuidVar">The null unique identifier variable.</param>
        /// <param name="enumVar">The enum variable.</param>
        /// <param name="nullEnumVar">The null enum variable.</param>
        /// <returns></returns>
        public SimpleUrlBindingRs GetWithRoute(
            string stringVar,
            char charVar,
            char? nullCharVar,
            short int16Var,
            ushort uInt16Var,
            short? nullInt16Var,
            ushort? nullUInt16Var,
            int int32Var,
            uint uInt32Var,
            int? nullInt32Var,
            uint? nullUInt32Var,
            long int64Var,
            ulong uInt64Var,
            long? nullInt64Var,
            ulong? nullUInt64Var,
            double doubleVar,
            double? nullDoubleVar,
            decimal decimalVar,
            decimal? nullDecimalVar,
            float floatVar,
            float? nullFloatVar,
            bool boolVar,
            bool? nullBoolVar,
            DateTime dateTimeVar,
            DateTime? nullDateTimeVar,
            DateTimeOffset dateTimeOffsetVar,
            DateTimeOffset? nullDateTimeOffsetVar,
            TimeSpan timeSpanVar,
            TimeSpan? nullTimeSpanVar,
            byte byteVar,
            byte? nullByteVar,
            sbyte sByteVar,
            sbyte? nullSByteVar,
            Guid guidVar,
            Guid? nullGuidVar,
            SimpleUrlBindingEnum enumVar,
            SimpleUrlBindingEnum? nullEnumVar)
        {
            return new SimpleUrlBindingRs
            {
                StringVar = stringVar,
                CharVar = charVar,
                NullCharVar = nullCharVar,
                Int16Var = int16Var,
                UInt16Var = uInt16Var,
                NullInt16Var = nullInt16Var,
                NullUInt16Var = nullUInt16Var,
                Int32Var = int32Var,
                UInt32Var = uInt32Var,
                NullInt32Var = nullInt32Var,
                NullUInt32Var = nullUInt32Var,
                Int64Var = int64Var,
                UInt64Var = uInt64Var,
                NullInt64Var = nullInt64Var,
                NullUInt64Var = nullUInt64Var,
                DoubleVar = doubleVar,
                NullDoubleVar = nullDoubleVar,
                DecimalVar = decimalVar,
                NullDecimalVar = nullDecimalVar,
                FloatVar = floatVar,
                NullFloatVar = nullFloatVar,
                BoolVar = boolVar,
                NullBoolVar = nullBoolVar,
                DateTimeVar = dateTimeVar,
                NullDateTimeVar = nullDateTimeVar,
                DateTimeOffsetVar = dateTimeOffsetVar,
                NullDateTimeOffsetVar = nullDateTimeOffsetVar,
                TimeSpanVar = timeSpanVar,
                NullTimeSpanVar = nullTimeSpanVar,
                ByteVar = byteVar,
                NullByteVar = nullByteVar,
                SByteVar = sByteVar,
                NullSByteVar = nullSByteVar,
                GuidVar = guidVar,
                NullGuidVar = nullGuidVar,
                EnumVar = enumVar,
                NullEnumVar = nullEnumVar
            };
        }

        /// <summary>Gets the with mixed.</summary>
        /// <param name="stringVar">The string variable.</param>
        /// <param name="charVar">The character variable.</param>
        /// <param name="nullCharVar">The null character variable.</param>
        /// <param name="int16Var">The int16 variable.</param>
        /// <param name="uInt16Var">The u int16 variable.</param>
        /// <param name="nullInt16Var">The null int16 variable.</param>
        /// <param name="nullUInt16Var">The null u int16 variable.</param>
        /// <param name="int32Var">The int32 variable.</param>
        /// <param name="uInt32Var">The u int32 variable.</param>
        /// <param name="nullInt32Var">The null int32 variable.</param>
        /// <param name="nullUInt32Var">The null u int32 variable.</param>
        /// <param name="int64Var">The int64 variable.</param>
        /// <param name="uInt64Var">The u int64 variable.</param>
        /// <param name="nullInt64Var">The null int64 variable.</param>
        /// <param name="nullUInt64Var">The null u int64 variable.</param>
        /// <param name="doubleVar">The double variable.</param>
        /// <param name="nullDoubleVar">The null double variable.</param>
        /// <param name="decimalVar">The decimal variable.</param>
        /// <param name="nullDecimalVar">The null decimal variable.</param>
        /// <param name="floatVar">The float variable.</param>
        /// <param name="nullFloatVar">The null float variable.</param>
        /// <param name="boolVar">if set to <c>true</c> [bool variable].</param>
        /// <param name="nullBoolVar">The null bool variable.</param>
        /// <param name="dateTimeVar">The date time variable.</param>
        /// <param name="nullDateTimeVar">The null date time variable.</param>
        /// <param name="dateTimeOffsetVar">The date time offset variable.</param>
        /// <param name="nullDateTimeOffsetVar">The null date time offset variable.</param>
        /// <param name="timeSpanVar">The time span variable.</param>
        /// <param name="nullTimeSpanVar">The null time span variable.</param>
        /// <param name="byteVar">The byte variable.</param>
        /// <param name="nullByteVar">The null byte variable.</param>
        /// <param name="sByteVar">The s byte variable.</param>
        /// <param name="nullSByteVar">The null s byte variable.</param>
        /// <param name="guidVar">The unique identifier variable.</param>
        /// <param name="nullGuidVar">The null unique identifier variable.</param>
        /// <param name="enumVar">The enum variable.</param>
        /// <param name="nullEnumVar">The null enum variable.</param>
        /// <param name="boundType">Type of the bound.</param>
        /// <returns></returns>
        public IList<SimpleUrlBindingRs> GetWithMixed(
            string stringVar,
            char charVar,
            char? nullCharVar,
            short int16Var,
            ushort uInt16Var,
            short? nullInt16Var,
            ushort? nullUInt16Var,
            int int32Var,
            uint uInt32Var,
            int? nullInt32Var,
            uint? nullUInt32Var,
            long int64Var,
            ulong uInt64Var,
            long? nullInt64Var,
            ulong? nullUInt64Var,
            double doubleVar,
            double? nullDoubleVar,
            decimal decimalVar,
            decimal? nullDecimalVar,
            float floatVar,
            float? nullFloatVar,
            bool boolVar,
            bool? nullBoolVar,
            DateTime dateTimeVar,
            DateTime? nullDateTimeVar,
            DateTimeOffset dateTimeOffsetVar,
            DateTimeOffset? nullDateTimeOffsetVar,
            TimeSpan timeSpanVar,
            TimeSpan? nullTimeSpanVar,
            byte byteVar,
            byte? nullByteVar,
            sbyte sByteVar,
            sbyte? nullSByteVar,
            Guid guidVar,
            Guid? nullGuidVar,
            SimpleUrlBindingEnum enumVar,
            SimpleUrlBindingEnum? nullEnumVar,
            [UriBound] SimpleUrlBindingRs boundType)
        {
            return new List<SimpleUrlBindingRs>
            {
                boundType,
                new SimpleUrlBindingRs {
                    StringVar = stringVar,
                    CharVar = charVar,
                    NullCharVar = nullCharVar,
                    Int16Var = int16Var,
                    UInt16Var = uInt16Var,
                    NullInt16Var = nullInt16Var,
                    NullUInt16Var = nullUInt16Var,
                    Int32Var = int32Var,
                    UInt32Var = uInt32Var,
                    NullInt32Var = nullInt32Var,
                    NullUInt32Var = nullUInt32Var,
                    Int64Var = int64Var,
                    UInt64Var = uInt64Var,
                    NullInt64Var = nullInt64Var,
                    NullUInt64Var = nullUInt64Var,
                    DoubleVar = doubleVar,
                    NullDoubleVar = nullDoubleVar,
                    DecimalVar = decimalVar,
                    NullDecimalVar = nullDecimalVar,
                    FloatVar = floatVar,
                    NullFloatVar = nullFloatVar,
                    BoolVar = boolVar,
                    NullBoolVar = nullBoolVar,
                    DateTimeVar = dateTimeVar,
                    NullDateTimeVar = nullDateTimeVar,
                    DateTimeOffsetVar = dateTimeOffsetVar,
                    NullDateTimeOffsetVar = nullDateTimeOffsetVar,
                    TimeSpanVar = timeSpanVar,
                    NullTimeSpanVar = nullTimeSpanVar,
                    ByteVar = byteVar,
                    NullByteVar = nullByteVar,
                    SByteVar = sByteVar,
                    NullSByteVar = nullSByteVar,
                    GuidVar = guidVar,
                    NullGuidVar = nullGuidVar,
                    EnumVar = enumVar,
                    NullEnumVar = nullEnumVar
                }
            };
        }
    }

    /// <summary>
    /// Simple Url Binding Enum
    /// </summary>
    [Flags]
    public enum SimpleUrlBindingEnum
    {
        /// <summary>The none</summary>
        None = 0,

        /// <summary>The one</summary>
        One = 1,

        /// <summary>The two</summary>
        Two = 2,

        /// <summary>The four</summary>
        Four = 4,

        /// <summary>The eight</summary>
        Eight = 8
    }

    /// <summary>
    /// 
    /// </summary>
    public class SimpleUrlBindingRs
    {
        /// <summary>Gets or sets the string variable.</summary>
        /// <value>The string variable.</value>
        public string StringVar { get; set; }

        /// <summary>Gets or sets the character variable.</summary>
        /// <value>The character variable.</value>
        public char CharVar { get; set; }

        /// <summary>Gets or sets the null character variable.</summary>
        /// <value>The null character variable.</value>
        public char? NullCharVar { get; set; }

        /// <summary>Gets or sets the int16 variable.</summary>
        /// <value>The int16 variable.</value>
        public short Int16Var { get; set; }

        /// <summary>Gets or sets the u int16 variable.</summary>
        /// <value>The u int16 variable.</value>
        public ushort UInt16Var { get; set; }

        /// <summary>Gets or sets the null int16 variable.</summary>
        /// <value>The null int16 variable.</value>
        public short? NullInt16Var { get; set; }

        /// <summary>Gets or sets the null u int16 variable.</summary>
        /// <value>The null u int16 variable.</value>
        public ushort? NullUInt16Var { get; set; }

        /// <summary>Gets or sets the int32 variable.</summary>
        /// <value>The int32 variable.</value>
        public int Int32Var { get; set; }

        /// <summary>Gets or sets the u int32 variable.</summary>
        /// <value>The u int32 variable.</value>
        public uint UInt32Var { get; set; }

        /// <summary>Gets or sets the null int32 variable.</summary>
        /// <value>The null int32 variable.</value>
        public int? NullInt32Var { get; set; }

        /// <summary>Gets or sets the null u int32 variable.</summary>
        /// <value>The null u int32 variable.</value>
        public uint? NullUInt32Var { get; set; }

        /// <summary>Gets or sets the int64 variable.</summary>
        /// <value>The int64 variable.</value>
        public long Int64Var { get; set; }

        /// <summary>Gets or sets the u int64 variable.</summary>
        /// <value>The u int64 variable.</value>
        public ulong UInt64Var { get; set; }

        /// <summary>Gets or sets the null int64 variable.</summary>
        /// <value>The null int64 variable.</value>
        public long? NullInt64Var { get; set; }

        /// <summary>Gets or sets the null u int64 variable.</summary>
        /// <value>The null u int64 variable.</value>
        public ulong? NullUInt64Var { get; set; }

        /// <summary>Gets or sets the double variable.</summary>
        /// <value>The double variable.</value>
        public double DoubleVar { get; set; }

        /// <summary>Gets or sets the null double variable.</summary>
        /// <value>The null double variable.</value>
        public double? NullDoubleVar { get; set; }

        /// <summary>Gets or sets the decimal variable.</summary>
        /// <value>The decimal variable.</value>
        public decimal DecimalVar { get; set; }

        /// <summary>Gets or sets the null decimal variable.</summary>
        /// <value>The null decimal variable.</value>
        public decimal? NullDecimalVar { get; set; }

        /// <summary>Gets or sets the float variable.</summary>
        /// <value>The float variable.</value>
        public float FloatVar { get; set; }

        /// <summary>Gets or sets the null float variable.</summary>
        /// <value>The null float variable.</value>
        public float? NullFloatVar { get; set; }

        /// <summary>Gets or sets a value indicating whether [bool variable].</summary>
        /// <value><c>true</c> if [bool variable]; otherwise, <c>false</c>.</value>
        public bool BoolVar { get; set; }

        /// <summary>Gets or sets the null bool variable.</summary>
        /// <value>The null bool variable.</value>
        public bool? NullBoolVar { get; set; }

        /// <summary>Gets or sets the date time variable.</summary>
        /// <value>The date time variable.</value>
        public DateTime DateTimeVar { get; set; }

        /// <summary>Gets or sets the null date time variable.</summary>
        /// <value>The null date time variable.</value>
        public DateTime? NullDateTimeVar { get; set; }

        /// <summary>Gets or sets the date time offset variable.</summary>
        /// <value>The date time offset variable.</value>
        public DateTimeOffset DateTimeOffsetVar { get; set; }

        /// <summary>Gets or sets the null date time offset variable.</summary>
        /// <value>The null date time offset variable.</value>
        public DateTimeOffset? NullDateTimeOffsetVar { get; set; }

        /// <summary>Gets or sets the time span variable.</summary>
        /// <value>The time span variable.</value>
        public TimeSpan TimeSpanVar { get; set; }

        /// <summary>Gets or sets the null time span variable.</summary>
        /// <value>The null time span variable.</value>
        public TimeSpan? NullTimeSpanVar { get; set; }

        /// <summary>Gets or sets the byte variable.</summary>
        /// <value>The byte variable.</value>
        public byte ByteVar { get; set; }

        /// <summary>Gets or sets the null byte variable.</summary>
        /// <value>The null byte variable.</value>
        public byte? NullByteVar { get; set; }

        /// <summary>Gets or sets the s byte variable.</summary>
        /// <value>The s byte variable.</value>
        public sbyte SByteVar { get; set; }

        /// <summary>Gets or sets the null s byte variable.</summary>
        /// <value>The null s byte variable.</value>
        public sbyte? NullSByteVar { get; set; }

        /// <summary>Gets or sets the unique identifier variable.</summary>
        /// <value>The unique identifier variable.</value>
        public Guid GuidVar { get; set; }

        /// <summary>Gets or sets the null unique identifier variable.</summary>
        /// <value>The null unique identifier variable.</value>
        public Guid? NullGuidVar { get; set; }

        /// <summary>Gets or sets the enum variable.</summary>
        /// <value>The enum variable.</value>
        public SimpleUrlBindingEnum EnumVar { get; set; }

        /// <summary>Gets or sets the null enum variable.</summary>
        /// <value>The null enum variable.</value>
        public SimpleUrlBindingEnum? NullEnumVar { get; set; }
    }
}
