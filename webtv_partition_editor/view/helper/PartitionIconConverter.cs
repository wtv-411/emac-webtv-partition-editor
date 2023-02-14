using System;
using System.Windows;
using System.Windows.Data;
using System.Windows.Media.Imaging;

namespace webtv_partition_editor
{
    class PartitionIconConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            var images_path = "pack://application:,,,/webtv_partition_editor;component/view/static/images";

            var part = value as WebTVPartition;

            if (part != null)
            {
                switch (part.type)
                {
                    case PartitionType.FREE:
                        return new BitmapImage(new Uri(images_path + "/partition-free.png", UriKind.Absolute));

                    case PartitionType.ONE:
                        return new BitmapImage(new Uri(images_path + "/partition-one.png", UriKind.Absolute));

                    case PartitionType.FAT16:
                        return new BitmapImage(new Uri(images_path + "/partition-fat.png", UriKind.Absolute));

                    case PartitionType.BOOT:
                        return new BitmapImage(new Uri(images_path + "/partition-boot.png", UriKind.Absolute));

                    case PartitionType.FAT16_DVR:
                        return new BitmapImage(new Uri(images_path + "/partition-fat.png", UriKind.Absolute));

                    case PartitionType.COMPRESSFS:
                        if (part.disk.layout == DiskLayout.UTV)
                        {
                            return new BitmapImage(new Uri(images_path + "/partition-compressfs.png", UriKind.Absolute));
                        }
                        else
                        {
                            return new BitmapImage(new Uri(images_path + "/partition-boot.png", UriKind.Absolute));
                        }

                    case PartitionType.UNALLOCATED:
                        return new BitmapImage(new Uri(images_path + "/partition-unallocated.png", UriKind.Absolute));

                    default:
                        return new BitmapImage(new Uri(images_path + "/partition-unknown.png", UriKind.Absolute));
                }
            }

            return new BitmapImage(new Uri(images_path + "/partition-unknown.png", UriKind.Absolute));
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
