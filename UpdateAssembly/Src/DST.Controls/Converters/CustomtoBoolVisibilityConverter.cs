using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace DST.Controls.Converters
{
    public class CustomtoBoolVisibilityConverter : IMultiValueConverter
    {
        //public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        //{
        //    bool isAdd = (bool)parameter;
        //    if (isAdd) // 新增
        //    {
        //        return Visibility.Visible;
        //    }

        //    if (value == null) // 编辑
        //    {
        //        return Visibility.Hidden;
        //    }
        //    else
        //    {
        //        return Visibility.Visible;
        //    }
        //}

        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            if (values.Length == 2)
            {
            }
            return Visibility.Visible;
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}