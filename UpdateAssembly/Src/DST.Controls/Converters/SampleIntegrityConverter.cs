using System;
using System.Globalization;
using System.Windows.Data;

namespace DST.Controls.Converters
{
    /// <summary>
    /// 样本完整度转换器
    /// </summary>
    public class SampleIntegrityConverter : IValueConverter
    {
        /// <summary>
        /// { "0", "完好" }, { "1", "损坏" }, { "2", "丢失" }
        /// </summary>
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            string result = "完好";
            if (value != null && !string.IsNullOrEmpty(value.ToString()))
            {
                switch (value.ToString())
                {
                    case "0":
                        result = "完好";
                        break;

                    case "1":
                        result = "损坏";
                        break;

                    case "2":
                        result = "丢失";
                        break;

                    default:
                        break;
                }
            }

            return result;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}