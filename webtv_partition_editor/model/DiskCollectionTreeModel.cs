using System;
using System.Linq;
using System.Text;
using Aga.Controls.Tree;
using System.Collections;
using System.Collections.Generic;
using System.Windows.Media.Imaging;

namespace webtv_partition_editor
{
    class DiskCollectionTreeModel : ITreeModel
    {
        public WebTVDiskCollection disks { get; set; }

        public IEnumerable GetChildren(object parent)
        {
            if (parent == null)
            {
                foreach (WebTVDisk disk in disks)
                {
                    yield return new TreeListViewDatum()
                    {
                        id = disk.id,
                        object_type = TreeListViewDatum.ItemObjectType.DISK,
                        icon = (BitmapImage)(new DiskIconConverter()).Convert(disk, null, null, null),
                        name = disk.name,
                        mount_point = "",
                        collation = (new DiskCollationConverter()).Convert(disk, null, null, null).ToString(),
                        type = (new DiskTypeConverter()).Convert(disk, null, null, null).ToString(),
                        status = (new DiskStatusConverter()).Convert(disk, null, null, null).ToString(),
                        capacity = (new DiskSizeConverter()).Convert(disk, null, null, null).ToString(),
                        range = "",
                        partition_table = disk.partition_table
                    };
                }

            }
            else 
            {
                var _parent = parent as TreeListViewDatum;

                if (_parent.partition_table != null)
                {
                    foreach (WebTVPartition part in _parent.partition_table)
                    {
                        yield return new TreeListViewDatum()
                        {
                            id = part.id,
                            object_type = TreeListViewDatum.ItemObjectType.PARTITION,
                            icon = (BitmapImage)(new PartitionIconConverter()).Convert(part, null, null, null),
                            name = part.name,
                            mount_point = (new PartitionMountPointConverter()).Convert(part.server, null, null, null).ToString(),
                            collation = "",
                            type = (new PartitionTypeConverter()).Convert(part, null, null, null).ToString(),
                            status = (new PartitionStatusFlagsConverter()).Convert(part, null, null, null).ToString(),
                            capacity = (new PartitionSizeConverter()).Convert(part, null, null, null).ToString(),
                            range = (new PartitionRangeConverter()).Convert(part, null, null, null).ToString(),
                            partition_table = null
                        };
                    }
                }
            }
        }

        public bool HasChildren(object parent)
        {
            if (parent == null)
            {
                return true;
            }
            else 
            {
                var _parent = parent as TreeListViewDatum;

                return (_parent.partition_table != null && _parent.partition_table.Count > 0);
            }
        }

        public DiskCollectionTreeModel(WebTVDiskCollection _disks)
        {
            this.disks = _disks;
        }
    }
}
