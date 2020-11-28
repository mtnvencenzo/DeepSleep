namespace DeepSleep.Formatting.Converters
{
    using System;
    using System.Text.Json;
    using System.Text.Json.Serialization;

    internal class NullableBooleanConverter : JsonConverter<bool?>
    {
        public override bool? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
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

            throw new JsonException($"Value '{value}' cannot be converted to a boolean value");
        }
        public override void Write(Utf8JsonWriter writer, bool? value, JsonSerializerOptions options)
        {
            throw new NotImplementedException();
        }
    }
}
