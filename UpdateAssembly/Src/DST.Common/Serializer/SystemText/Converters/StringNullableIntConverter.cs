using System;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace DST.Common.Converter
{
    public class StringNullableIntConverter : JsonConverter<int?>
    {
        public override int? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            int? result = null;

            if (reader.TokenType == JsonTokenType.String)
            {
                var str = reader.GetString();
                int temp;
                if (int.TryParse(str, out temp))
                {
                    result = temp;
                }
            }
            return result;
        }

        public override void Write(Utf8JsonWriter writer, int? value, JsonSerializerOptions options)
        {
            writer.WriteStringValue(value.ToString());
        }
    }
}