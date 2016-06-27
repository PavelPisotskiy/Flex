using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace FlexDesktop.Converters
{
    class SpeedConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if ((int)value >= 1073741824)
            {
                return ((int)value / 1073741824.0).ToString("0.##") + " ГБ/с";
            }
            else if ((int)value >= 1048576)
            {
                return ((int)value / 1048576.0).ToString("0.##") + " МБ/с";
            }
            else if ((int)value >= 1024)
            {
                return ((int)value / 1024.0).ToString("0.##") + " КБ/с";
            }
            else
            {
                return value + " Б/с";
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value.ToString().Contains(" ГБ/с"))
            {
                return System.Convert.ToInt32(value.ToString().Replace(" ГБ/с", "")) * 1073741824;
            }
            else if (value.ToString().Contains(" МБ/с"))
            {
                return System.Convert.ToInt32(value.ToString().Replace(" МБ/с", "")) * 1048576;
            }
            else if (value.ToString().Contains(" КБ/с"))
            {
                return System.Convert.ToInt32(value.ToString().Replace(" КБ/с", "")) * 1024;
            }
            else
            {
                return System.Convert.ToInt32(value.ToString().Replace(" Б/с", ""));
            }

        }
    }
}
