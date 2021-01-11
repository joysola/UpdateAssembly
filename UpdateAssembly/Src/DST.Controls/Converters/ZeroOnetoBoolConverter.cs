using System;
using System.Globalization;
using System.Windows.Data;

namespace DST.Controls.Converters
{
    public class ZeroOnetoBoolConverter : IValueConverter
    {
        /// <summary>
        ///
        /// </summary>
        /// <param name="value">0、1</param>
        /// <param name="targetType"></param>
        /// <param name="parameter"></param>
        /// <param name="culture"></param>
        /// <returns></returns>
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            bool result = false;
            if (value != null && parameter != null)
            {
                string status = parameter.ToString();
                switch (status)
                {
                    case "完好":
                        result = value.ToString() == "0";
                        break;

                    case "损坏":
                        result = value.ToString() == "1";
                        break;

                    case "丢失":
                        result = value.ToString() == "2";
                        break;
                }
            }

            return result;
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="value"></param>
        /// <param name="targetType"></param>
        /// <param name="parameter">0、1</param>
        /// <param name="culture"></param>
        /// <returns></returns>
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return parameter;
        }
    }
}