using System;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace DST.Common.Converter
{
    /// <summary>
    /// json中字符串或数字 转成 数字 的json转换器
    /// </summary>
    public class StringIntConverter : JsonConverter<int>
    {
        public override int Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            int result = -1; // -1表示失败
            if (reader.TokenType == JsonTokenType.Number)// 数字时
            {
                result = reader.GetInt32();
            }
            else if (reader.TokenType == JsonTokenType.String) // 字符串时
            {
                var str = reader.GetString();
                Int32.TryParse(str, out result);
            }
            return result;
        }

        public override void Write(Utf8JsonWriter writer, int value, JsonSerializerOptions options)
        {
            throw new NotImplementedException();
        }
    }
}