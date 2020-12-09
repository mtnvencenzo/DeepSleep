namespace Api.DeepSleep.Controllers.Binding
{
    using global::DeepSleep;
    using global::DeepSleep.Formatting;
    using System;
    using System.Collections.Generic;

    public class SimpleUrlBindingController
    {
        private IApiRequestContextResolver apiRequestContextResolver;

        public SimpleUrlBindingController(IApiRequestContextResolver apiRequestContextResolver)
        {
            this.apiRequestContextResolver = apiRequestContextResolver;
        }

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
            //var context = this.apiRequestContextResolver.GetContext();

            //var i = context.RequestServices.GetServices(typeof(IFormatStreamReaderWriter));

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

    [Flags]
    public enum SimpleUrlBindingEnum
    {
        None = 0,

        One = 1,

        Two = 2,

        Four = 4,

        Eight = 8
    }

    public class SimpleUrlBindingRs
    {
        // ----------------------------------
        // String Vars
        public string StringVar { get; set; }


        // ----------------------------------
        // Char Vars
        public char CharVar { get; set; }

        public char? NullCharVar { get; set; }


        // ----------------------------------
        // Int16 Vars
        public short Int16Var { get; set; }

        public ushort UInt16Var { get; set; }

        public short? NullInt16Var { get; set; }

        public ushort? NullUInt16Var { get; set; }


        // ----------------------------------
        // Int32 Vars
        public int Int32Var { get; set; }

        public uint UInt32Var { get; set; }

        public int? NullInt32Var { get; set; }

        public uint? NullUInt32Var { get; set; }


        // ----------------------------------
        // Int64 Vars
        public long Int64Var { get; set; }

        public ulong UInt64Var { get; set; }

        public long? NullInt64Var { get; set; }

        public ulong? NullUInt64Var { get; set; }


        // ----------------------------------
        // Double Vars
        public double DoubleVar { get; set; }

        public double? NullDoubleVar { get; set; }


        // ----------------------------------
        // Decimal Vars
        public decimal DecimalVar { get; set; }

        public decimal? NullDecimalVar { get; set; }


        // ----------------------------------
        // Float Vars
        public float FloatVar { get; set; }

        public float? NullFloatVar { get; set; }


        // ----------------------------------
        // Boolean Vars
        public bool BoolVar { get; set; }

        public bool? NullBoolVar { get; set; }


        // ----------------------------------
        // DateTime Vars
        public DateTime DateTimeVar { get; set; }

        public DateTime? NullDateTimeVar { get; set; }


        // ----------------------------------
        // DateTimeOffset Vars
        public DateTimeOffset DateTimeOffsetVar { get; set; }

        public DateTimeOffset? NullDateTimeOffsetVar { get; set; }


        // ----------------------------------
        // TimeSpan Vars
        public TimeSpan TimeSpanVar { get; set; }

        public TimeSpan? NullTimeSpanVar { get; set; }


        // ----------------------------------
        // Byte Vars
        public byte ByteVar { get; set; }

        public byte? NullByteVar { get; set; }

        public sbyte SByteVar { get; set; }

        public sbyte? NullSByteVar { get; set; }

        // ----------------------------------
        // Guid Vars
        public Guid GuidVar { get; set; }

        public Guid? NullGuidVar { get; set; }

        // ----------------------------------
        // Enum Vars
        public SimpleUrlBindingEnum EnumVar { get; set; }

        public SimpleUrlBindingEnum? NullEnumVar { get; set; }
    }
}
