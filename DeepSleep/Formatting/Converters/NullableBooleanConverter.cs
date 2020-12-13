namespace DeepSleep.Formatting.Converters
{
    using System;
    using System.Text.Json;
    using System.Text.Json.Serialization;

    internal class NullableBooleanConverter : JsonConverter<bool?>
    {
        /// <summary>Reads and converts the JSON to type</summary>
        /// <param name="reader">The reader.</param>
        /// <param name="typeToConvert">The type to convert.</param>
        /// <param name="options">An object that specifies serialization options to use.</param>
        /// <returns>The converted value.</returns>
        /// <exception cref="JsonException">Value '{value}' cannot be converted to a boolean value</exception>
        public override bool? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
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

            try
            {
                var val = reader.GetInt32();
                return val == 1;
            }
            catch { }

            string value = reader.GetString();
            string chkValue = value?.ToLower();

            if (chkValue == null)
            {
                return null;
            }
            if (chkValue.Equals("true"))
            {
                return true;
            }
            if (chkValue.Equals("false"))
            {
                return false;
            }
            if (chkValue.Equals("0"))
            {
                return false;
            }
            if (chkValue.Equals("1"))
            {
                return true;
            }

            throw new JsonException($"Value '{value}' cannot be converted to a boolean value");
        }

        /// <summary>Writes a specified value as JSON.</summary>
        /// <param name="writer">The writer to write to.</param>
        /// <param name="value">The value to convert to JSON.</param>
        /// <param name="options">An object that specifies serialization options to use.</param>
        /// <exception cref="System.NotImplementedException"></exception>
        public override void Write(Utf8JsonWriter writer, bool? value, JsonSerializerOptions options)
        {
            throw new NotImplementedException();
        }
    }
}
