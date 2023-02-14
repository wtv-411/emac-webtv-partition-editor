using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Runtime.InteropServices;

namespace webtv_partition_editor
{
    [StructLayout(LayoutKind.Sequential, Size = NVRAM.HD_NVRAM_SIZE), Serializable]
    public unsafe struct webtv_hd_nvram
    {
        [MarshalAsAttribute(UnmanagedType.ByValArray, SizeConst = 4)]
        public byte[] nvram_checksum;

        [MarshalAsAttribute(UnmanagedType.ByValArray, SizeConst = 12)]
        public byte[] unknown1;

        [MarshalAsAttribute(UnmanagedType.ByValArray, SizeConst = NVRAM.HD_NVRAM_SIZE - 4 - 12)]
        public byte[] nvram_settings_buf;

        [MarshalAsAttribute(UnmanagedType.ByValArray, SizeConst = NVRAM.HD_NVRAM_NAX_SETTING_COUNT)]
        public nvram_setting[] nvram_settings;
    }

    [StructLayout(LayoutKind.Sequential, Pack = 4), Serializable]
    public unsafe struct nvram_setting
    {
        [MarshalAsAttribute(UnmanagedType.ByValArray, SizeConst = 4)]
        public byte[] value_size;

        [MarshalAsAttribute(UnmanagedType.ByValArray, SizeConst = 4)]
        public byte[] setting_name;

        public IntPtr setting_value;
    }
    
    class NVRAM : WebTVIO
    {
        public const int HD_NVRAM_SIZE = 0x4000;
        public const int HD_NVRAM_NAX_SETTING_COUNT = 0x7FE;
        public bool is_viewer_nvram = false;

        public void unserialize_nvram()
        {
            GCHandle build_header_handle = new GCHandle();

            try
            {
                var nvram_data = new byte[NVRAM.HD_NVRAM_SIZE];
                int nvram_data_offset = 0;
                
                this.Read(nvram_data, 0, 0, NVRAM.HD_NVRAM_SIZE);

                build_header_handle = GCHandle.Alloc(nvram_data, GCHandleType.Pinned);
                var build_header_pointer = build_header_handle.AddrOfPinnedObject();

                var nvram_image = (webtv_hd_nvram)Marshal.PtrToStructure(build_header_pointer, typeof(webtv_hd_nvram));

                nvram_data_offset += 4 + 12;
                for (var i = 0; i < nvram_image.nvram_settings.Length; i++)
                {
                    nvram_data_offset += 4 + 4;

                    if (nvram_image.nvram_settings[i].setting_name[0] == 0x00)
                    {
                        break;
                    }

                    var value_size = get_value_size(nvram_image.nvram_settings[i].value_size);

                    if (value_size > 0 && value_size < NVRAM.HD_NVRAM_SIZE)
                    {
                        nvram_image.nvram_settings[i].setting_value = Marshal.AllocHGlobal(value_size);

                        Marshal.Copy(nvram_data, nvram_data_offset, nvram_image.nvram_settings[i].setting_value, value_size);

                        nvram_data_offset += WORD.size_bound_to_xwords(value_size, WORD.WordSizes.DWORD);
                    }

                }

                //return nvram_image;
            }
            catch (Exception e)
            {
                throw new InvalidDataException("Couldn't parse NVRAM. " + e.Message);
            }
            finally
            {
                if (build_header_handle.IsAllocated)
                {
                    build_header_handle.Free();
                }
            }
        }

        private int get_value_size(byte[] value_size_array)
        {
            int value_size = 0;

            if (is_viewer_nvram)
            {
                value_size = BitConverter.ToInt32(value_size_array, 0);
            }
            else
            {
                value_size = BigEndianConverter.ToInt32(value_size_array, 0);
            }

            return value_size;
        }

        public NVRAM(WebTVDiskIO io, ObjectLocation nvram_location)
            : base(io, get_object_bounds(nvram_location))
        {
        }

        public NVRAM(string file_name) 
            : base(file_name)
        {
        }

        public NVRAM(Stream reader)
            : base(reader)
        {
        }

    }
}
