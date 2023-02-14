using System;
using System.Windows;
using System.Windows.Data;

namespace webtv_partition_editor
{
    class PartitionTypeConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            var part = value as WebTVPartition;

            if (part != null)
            {
                switch (part.type)
                {
                    case PartitionType.FREE:
                        return "FREE";

                    case PartitionType.ONE:
                        return "OOO";

                    case PartitionType.FAT16:
                        return "FAT16";

                    case PartitionType.COMPRESSFS:
                        if (part.disk.layout == DiskLayout.UTV)
                        {
                            return "COMPRESSFS";
                        }
                        else
                        {
                            return "BOOT2";
                        }

                    case PartitionType.FAT16_DVR:
                        return "FAT16 DVR";

                    case PartitionType.BOOT:
                        return "BOOT";

                    case PartitionType.UNALLOCATED:
                        return "UNALLOCATED";

                    default:
                        return "UNKNOWN [" + (part.type).ToString("X") + "]";
                }
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
