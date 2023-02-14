using System;
using System.Windows;
using System.Windows.Data;

namespace webtv_partition_editor
{
    class ObjectLocationNameConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            var build_location = value as ObjectLocation;

            if (build_location != null)
            {
                var build_location_name = "";

                switch (build_location.type)
                {
                    case ObjectLocationType.FILE_LOCATION:
                        build_location_name += "Build File";
                        break;

                    case ObjectLocationType.PRIMARY_LOCATION:
                        build_location_name += "Primary";
                        break;

                    case ObjectLocationType.SECONDARY_LOCATION:
                        build_location_name += "Secondary?";
                        break;

                    case ObjectLocationType.BACKUP_LOCATION:
                        build_location_name += "Backup?";
                        break;

                    case ObjectLocationType.EXPLODED_PRIMARY_LOCATION:
                        build_location_name += "Exploded Primary";
                        break;

                    default:
                        build_location_name += "Unknown";
                        break;
                }

                if (build_location.type != ObjectLocationType.FILE_LOCATION)
                {
                    build_location_name += ", offset=" + build_location.offset.ToString("X");

                    build_location_name += ", size=" + BytesToString.bytes_to_iec(build_location.size_bytes);
                }

                return build_location_name;
            }

            return "Bad";
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
