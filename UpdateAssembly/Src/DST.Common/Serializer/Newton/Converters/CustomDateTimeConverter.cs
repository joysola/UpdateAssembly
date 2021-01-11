using Newtonsoft.Json.Converters;

namespace DST.Common.Converter
{
    /// <summary>
    /// Newtonjson格式化日期格式
    /// </summary>
    public class CustomDateTimeConverter : IsoDateTimeConverter
    {
        public CustomDateTimeConverter()
        {
            base.DateTimeFormat = "yyyy-MM-dd HH:mm:ss";
        }
    }
}