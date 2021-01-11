using System;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace DST.Common.Converter
{
    public class CustomDateTimeStrConverter : JsonConverter<DateTime>
    {
        private readonly string format = "yyyy-MM-dd HH:mm:ss";

        public override DateTime Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            var result = DateTime.MinValue;
            if (reader.TokenType == JsonTokenType.String)
            {
                string str = reader.GetString();
                try
                {
                    result = DateTime.Parse(str);// 将字符串转成DateTime
                }
                catch
                {
                }
            }
            return result;
        }

        public override void Write(Utf8JsonWriter writer, DateTime value, JsonSerializerOptions options)
        {
            writer.WriteStringValue(value.ToString(format)); // 将datetime转成固定格式字符串
        }
    }
}