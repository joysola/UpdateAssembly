using System;
using System.Globalization;
using System.Windows.Data;

namespace DST.Controls.Converters
{
    public class ZeroOneToStringConverter : IValueConverter
    {
        /// <summary>
        /// 将0、1转换成 string
        /// </summary>
        /// <param name="value">0、1</param>
        /// <param name="targetType"></param>
        /// <param name="parameter">传入需要组装的文字</param>
        /// <param name="culture"></param>
        /// <returns></returns>
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null)
            {
                return null;
            }
            int num = (int)value;
            string text = (string)parameter;
            switch (num)
            {
                case 0:
                    text = "未" + text;
                    break;

                case 1:
                    text = "已" + text;
                    break;

                default:
                    text = null;
                    break;
            }
            return text;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            string text = value.ToString();
            if (text.Contains("已"))
            {
                return 1;
            }
            if (text.Contains("未"))
            {
                return 0;
            }
            if (text.Contains("请选择"))
            {
                return -1;
            }
            return null;
        }
    }
}