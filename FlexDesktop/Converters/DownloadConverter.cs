using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace FlexDesktop.Converters
{
    class DownloadConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            MonoTorrent.Common.Priority result = (MonoTorrent.Common.Priority)value;

            if (result == MonoTorrent.Common.Priority.DoNotDownload)
                return false;


            return true;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if ((bool)value == false)
                return MonoTorrent.Common.Priority.DoNotDownload;

            return MonoTorrent.Common.Priority.Normal;
        }
    }
}
