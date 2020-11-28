using DeepSleep.Formatting.Converters;
using System;
using System.Collections.Generic;
namespace DeepSleep.Formatting
{
    using System.Text.Json;
    using System.Text.Json.Serialization;

    internal static class JsonReaderSerializationOptions
    {
        internal readonly static JsonSerializerOptions ReaderOptions;

        static JsonReaderSerializationOptions()
        {
            ReaderOptions = new JsonSerializerOptions
            {
                AllowTrailingCommas = false,
                DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingDefault,
                IgnoreReadOnlyFields = true,
                IgnoreReadOnlyProperties = true,
                IncludeFields = false,
                NumberHandling = JsonNumberHandling.AllowReadingFromString,
                PropertyNameCaseInsensitive = true,
                ReadCommentHandling = JsonCommentHandling.Skip
            };

            ReaderOptions.Converters.Add(new NullableBooleanConverter());
            ReaderOptions.Converters.Add(new BooleanConverter());
            ReaderOptions.Converters.Add(new JsonStringEnumConverter(allowIntegerValues: true));
            ReaderOptions.Converters.Add(new NullableTimeSpanConverter());
            ReaderOptions.Converters.Add(new TimeSpanConverter());
            ReaderOptions.Converters.Add(new NullableDateTimeConverter());
            ReaderOptions.Converters.Add(new DateTimeConverter());
            ReaderOptions.Converters.Add(new NullableDateTimeOffsetConverter());
            ReaderOptions.Converters.Add(new DateTimeOffsetConverter());
            ReaderOptions.Converters.Add(new ObjectConverter());
        }
    }
}
