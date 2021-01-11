using DST.Database.Model;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;

namespace DST.Controls.Converters
{
    public class ProductTypeConverter : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            List<ProductType> result = new List<ProductType>();
            if (values.Length == 2 && !values.Contains(DependencyProperty.UnsetValue))
            {
                var id = values[0] as string;
                var dict = values[1] as List<ProductModel>;
                result = dict.FirstOrDefault(x => x.id == id)?.productTypes;
            }
            return result;
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
