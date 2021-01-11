using System;
using System.Globalization;
using System.Windows.Data;

namespace DST.Controls.Converters
{
    public class SexRadioBtnConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            string sexCode = value?.ToString();
            string par = parameter?.ToString();
            return sexCode == par;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            bool isChecked = (bool)value;
            return parameter;
        }
    }
}