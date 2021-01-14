﻿namespace DeepSleep.Formatting.Converters
{
    using System;
    using System.Globalization;
    using System.Text.Json;
    using System.Text.Json.Serialization;

    /// <summary>
    /// 
    /// </summary>
    public class NullableTimeSpanConverter : JsonConverter<TimeSpan?>
    {
        /// <summary>Reads and converts the JSON to type</summary>
        /// <param name="reader">The reader.</param>
        /// <param name="typeToConvert">The type to convert.</param>
        /// <param name="options">An object that specifies serialization options to use.</param>
        /// <returns>The converted value.</returns>
        public override TimeSpan? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            if (reader.TokenType == JsonTokenType.Null)
            {
                return null;
            }

            if (reader.TokenType == JsonTokenType.None)
            {
                return null;
            }

            if (reader.TokenType == JsonTokenType.String)
            {
                string value = reader.GetString();

                if (value == null)
                {
                    return null;
                }

                return TimeSpan.Parse(value, CultureInfo.CurrentCulture);
            }

            throw new JsonException($"Value cannot be converted to a timespan? value");
        }

        /// <summary>Writes a specified value as JSON.</summary>
        /// <param name="writer">The writer to write to.</param>
        /// <param name="value">The value to convert to JSON.</param>
        /// <param name="options">An object that specifies serialization options to use.</param>
        public override void Write(Utf8JsonWriter writer, TimeSpan? value, JsonSerializerOptions options)
        {
            if (value == null)
            {
                writer.WriteNullValue();
            }
            else
            {
                writer.WriteStringValue(value.Value.ToString());
            }
        }
    }
}
