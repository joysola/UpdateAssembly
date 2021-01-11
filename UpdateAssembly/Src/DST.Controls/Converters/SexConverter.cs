using DST.Database;
using System;
using System.Globalization;
using System.Linq;
using System.Windows.Data;

namespace DST.Controls.Converters
{
    public class SexConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            string sexCode = value?.ToString();
            var sexValue = ExtendApiDict.Instance.SexDict.FirstOrDefault(x => x.dictKey == sexCode)?.dictValue;
            return sexValue;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}