using System;
using System.Collections.ObjectModel;

namespace webtv_partition_editor
{
    class ObjectLocationCollection : ObservableCollection<ObjectLocation>
    {
        private void add_lc2_build_offsets()
        {
            this.Add(new ObjectLocation()
            {
                type = ObjectLocationType.PRIMARY_LOCATION,
                offset = 0x80600,
                size_bytes = 0x800000,
            });

            this.Add(new ObjectLocation()
            {
                type = ObjectLocationType.BACKUP_LOCATION,
                offset = 0x880600,
                size_bytes = 0x800000,
            });

            this.Add(new ObjectLocation()
            {
                type = ObjectLocationType.SECONDARY_LOCATION,
                offset = 0x10C1000,
                size_bytes = 0x400000,
            });

            this.Add(new ObjectLocation()
            {
                type = ObjectLocationType.EXPLODED_PRIMARY_LOCATION,
                offset = 0x880600,
                size_bytes = 0xC40A00,
            });
        }

        private void add_webstar_build_offsets()
        {
            this.Add(new ObjectLocation()
            {
                type = ObjectLocationType.PRIMARY_LOCATION,
                offset = 0x80600,
                size_bytes = 0x1000000,
            });
        }

        private void add_utv_build_offsets()
        {
            this.Add(new ObjectLocation()
            {
                type = ObjectLocationType.PRIMARY_LOCATION,
                offset = 0x15480600,
                size_bytes = 0x2000000,
            });

            this.Add(new ObjectLocation()
            {
                type = ObjectLocationType.SECONDARY_LOCATION,
                offset = 0x13480600,
                size_bytes = 0x2000000,
            });
        }

        public void add_enumerated_build(WebTVPartition part)
        {
            if (part.sector_start == 0 && part.disk != null)
            {
                if (part.disk.layout == DiskLayout.UTV)
                {
                    this.add_utv_build_offsets();
                }
                else if (part.disk.layout == DiskLayout.WEBSTAR)
                {
                    this.add_webstar_build_offsets();
                }
                else
                {
                    this.add_lc2_build_offsets();
                }
            }
            else
            {
                if (part.disk.layout == DiskLayout.WEBSTAR && part.type == PartitionType.BOOT2)
                {
                    this.Add(new ObjectLocation()
                    {
                        type = ObjectLocationType.PRIMARY_LOCATION,
                        offset = 0,
                        size_bytes = (part.sector_length * part.disk.sector_bytes_length),
                    });
                }
                else if (part.type == PartitionType.BOOT)
                {
                    this.Add(new ObjectLocation()
                    {
                        type = ObjectLocationType.PRIMARY_LOCATION,
                        offset = WebTVPartitionManager.PARTITON_DATA_OFFSET,
                        size_bytes = (part.sector_length * part.disk.sector_bytes_length) - WebTVPartitionManager.PARTITON_DATA_OFFSET,
                    });
                }
            }
        }
    }
}
