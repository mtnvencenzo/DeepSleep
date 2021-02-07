namespace DeepSleep.Media.Converters
{
    using System;
    using System.Text.Json;
    using System.Text.Json.Serialization;

    /// <summary>
    /// 
    /// </summary>
    public class BooleanConverter : JsonConverter<bool>
    {
        /// <summary>Reads and converts the JSON to type</summary>
        /// <param name="reader">The reader.</param>
        /// <param name="typeToConvert">The type to convert.</param>
        /// <param name="options">An object that specifies serialization options to use.</param>
        /// <returns>The converted value.</returns>
        /// <exception cref="JsonException">Value '{value}' cannot be converted to a boolean value</exception>
        public override bool Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            if (reader.TokenType == JsonTokenType.Null)
            {
                return false;
            }

            if (reader.TokenType == JsonTokenType.None)
            {
                return false;
            }

            if (reader.TokenType == JsonTokenType.True)
            {
                return true;
            }

            if (reader.TokenType == JsonTokenType.False)
            {
                return false;
            }

            if (reader.TokenType == JsonTokenType.Number)
            {
                reader.TryGetDouble(out var number);
                return number == 1d;
            }

            if (reader.TokenType == JsonTokenType.String)
            {
                string value = reader.GetString()?.ToLowerInvariant();

                if (value == null)
                {
                    return false;
                }
                if (value.Equals("true"))
                {
                    return true;
                }
                if (value.Equals("false"))
                {
                    return false;
                }
                if (value.Equals("0"))
                {
                    return false;
                }
                if (value.Equals("1"))
                {
                    return true;
                }
            }


            throw new JsonException($"Value cannot be converted to a boolean value");
        }

        /// <summary>Writes a specified value as JSON.</summary>
        /// <param name="writer">The writer to write to.</param>
        /// <param name="value">The value to convert to JSON.</param>
        /// <param name="options">An object that specifies serialization options to use.</param>
        public override void Write(Utf8JsonWriter writer, bool value, JsonSerializerOptions options)
        {
            writer.WriteBooleanValue(value);
        }
    }
}
