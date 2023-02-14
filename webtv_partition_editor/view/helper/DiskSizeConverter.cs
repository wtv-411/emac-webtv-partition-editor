using System;
using System.Windows;
using System.Windows.Data;

namespace webtv_partition_editor
{
    class DiskSizeConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            var disk = value as WebTVDisk;

            if (disk != null)
            {
                return BytesToString.bytes_to_iec(disk.size_bytes);
            }
            else 
            {
                return "Bad";
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
