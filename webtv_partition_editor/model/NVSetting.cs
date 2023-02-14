using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace webtv_partition_editor
{
    public enum NVDataType
    {
        NULL_TERMINATED_STRING = 0,
        UINT16 = 1,
        INT16 = 2,
        UINT32 = 3,
        INT32 = 4,
        UINT64 = 5,
        INT64 = 6,
        BOOLEAN = 7,
        CHAR = 8,
        BINARY_BLOB = 9,
    };

    public enum NVDataEditor
    {
        AUTODETECT = 0,
        HEX_EDITOR = 1,
        BINARY_EDITOR = 2,
        BOOLEAN_EDITOR = 3,
        STRING_EDITOR = 4,
        INTEGER_EDITOR = 5,
        IP_EDITOR = 6,
        FILE_EDITOR = 7,
    };

    public struct NVSettingValue
    {
        public byte[] current_value { get; set; }
        public byte[] default_value { get; set; }
    }

    public class NVSetting
    {
        public string name { get; set; }
        public string title { get; set; }
        public string description { get; set; }
        public string notes { get; set; }
        public NVDataType data_type { get; set; }
        public NVDataEditor data_editor { get; set; }
        public NVSettingValue value { get; set; }
    }
}
