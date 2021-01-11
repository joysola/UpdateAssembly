using System;
using System.Globalization;
using System.Windows.Data;

namespace DST.Converters
{
    /// <summary>
    /// 这是一个颠倒黑白的世界
    /// </summary>
    public class TrueToFalseConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var v = (bool)value;
            return !v;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var v = (bool)value;
            return !v;
        }
    }
}