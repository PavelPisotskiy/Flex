using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace FlexDesktop.Converters
{
    public class BiteConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if ((long)value >= 1073741824)
            {
                return ((long)value / 1073741824.0).ToString("0.##") + " ГБ";
            }
            else if ((long)value >= 1048576)
            {
                return ((long)value / 1048576.0).ToString("0.##") + " МБ";
            }
            else if ((long)value >= 1024)
            {
                return ((long)value / 1024.0).ToString("0.##") + " КБ";
            }
            else
            {
                return value + " Б";
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value.ToString().Contains(" ГБ"))
            {
                return System.Convert.ToInt64(value.ToString().Replace(" ГБ", "")) * 1073741824;
            }
            else if (value.ToString().Contains(" МБ"))
            {
                return System.Convert.ToInt64(value.ToString().Replace(" МБ", "")) * 1048576;
            }
            else if (value.ToString().Contains(" КБ"))
            {
                return System.Convert.ToInt64(value.ToString().Replace(" КБ", "")) * 1024;
            }
            else
            {
                return System.Convert.ToInt64(value.ToString().Replace(" Б", ""));
            }

        }
    }
}
