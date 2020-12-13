namespace DeepSleep.Formatting.Converters
{
    using System;
    using System.Text.Json;
    using System.Text.Json.Serialization;

    internal class ObjectConverter : JsonConverter<object>
    {
        /// <summary>Reads and converts the JSON to type</summary>
        /// <param name="reader">The reader.</param>
        /// <param name="typeToConvert">The type to convert.</param>
        /// <param name="options">An object that specifies serialization options to use.</param>
        /// <returns>The converted value.</returns>
        /// <exception cref="JsonException"></exception>
        public override object Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            if (reader.TokenType == JsonTokenType.Null)
            {
                return null;
            }

            if (reader.TokenType == JsonTokenType.True)
            {
                return true;
            }

            if (reader.TokenType == JsonTokenType.False)
            {
                return false;
            }

            // Forward to the JsonElement converter
            var converter = options.GetConverter(typeof(JsonElement)) as JsonConverter<JsonElement>;
            if (converter != null)
            {
                var element = converter.Read(ref reader, typeToConvert, options);

                if (element.ValueKind == JsonValueKind.False)
                {
                    return false;
                }

                if (element.ValueKind == JsonValueKind.True)
                {
                    return true;
                }

                if (element.ValueKind == JsonValueKind.Undefined)
                {
                    return null;
                }

                if (element.ValueKind == JsonValueKind.Null)
                {
                    return null;
                }

                if (element.ValueKind == JsonValueKind.String)
                {
                    return element.GetString();
                }

                if (element.ValueKind == JsonValueKind.Number)
                {
                    return element.GetDouble();
                }

                return element;
            }

            throw new JsonException();
        }

        /// <summary>Writes a specified value as JSON.</summary>
        /// <param name="writer">The writer to write to.</param>
        /// <param name="value">The value to convert to JSON.</param>
        /// <param name="options">An object that specifies serialization options to use.</param>
        /// <exception cref="System.NotImplementedException"></exception>
        public override void Write(Utf8JsonWriter writer, object value, JsonSerializerOptions options)
        {
            throw new NotImplementedException();
        }
    }
}
