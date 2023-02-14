using System;
using System.Windows;
using System.Windows.Data;
using System.Windows.Media;

namespace webtv_partition_editor
{
    class PartitionTypeColorConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            var part = value as WebTVPartition;

            if (part != null)
            {
                switch (part.type)
                {
                    case PartitionType.FREE:
                        return new SolidColorBrush((Color)ColorConverter.ConvertFromString("#00FF00"));

                    case PartitionType.ONE:
                        return new SolidColorBrush((Color)ColorConverter.ConvertFromString("#008000"));

                    case PartitionType.FAT16:
                        return new SolidColorBrush((Color)ColorConverter.ConvertFromString("#008080"));

                    case PartitionType.BOOT:
                        return new SolidColorBrush((Color)ColorConverter.ConvertFromString("#000080"));

                    case PartitionType.FAT16_DVR:
                        return new SolidColorBrush((Color)ColorConverter.ConvertFromString("#008080"));

                    case PartitionType.COMPRESSFS:
                        if (part.disk.layout == DiskLayout.UTV)
                        {
                            return new SolidColorBrush((Color)ColorConverter.ConvertFromString("#310080"));
                        }
                        else
                        {
                            return new SolidColorBrush((Color)ColorConverter.ConvertFromString("#000080"));
                        }

                    case PartitionType.UNALLOCATED:
                        return new SolidColorBrush((Color)ColorConverter.ConvertFromString("#000000"));

                    default:
                        return new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FF0000"));
                }
            }
            else
            {
                return new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FF0000"));
            }
        }
   
        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
