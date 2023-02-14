using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace webtv_partition_editor
{
    public enum ObjectLocationType
    { 
        EXPLODED_PRIMARY_LOCATION = -1,
        PRIMARY_LOCATION = 0,
        SECONDARY_LOCATION = 1,
        BACKUP_LOCATION = 2,
        FILE_LOCATION = 3
    };

    public class ObjectLocation
    {
        public ObjectLocationType type { get; set; }
        public ulong offset { get; set; }
        public ulong size_bytes { get; set; }
    }
}
