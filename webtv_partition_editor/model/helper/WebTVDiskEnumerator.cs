using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Management;
using System.Collections.ObjectModel;
using System.IO;

namespace webtv_partition_editor
{
    public class DiskWMIEntry
    {
        public string device_id { get; set; }
        public string title { get; set; }
        public string model { get; set; }
        public string name { get; set; }
        public string manufacture { get; set; }
        public string serial_number { get; set; }
        public string firmware_revision { get; set; }
        public string status { get; set; }
        public DiskSecurityStatus security_status { get; set; }
        public string security_info { get; set; }
        public bool is_security_locked { get; set; }
        public ulong size_bytes { get; set; }
        public uint sector_bytes_length { get; set; }
        public bool is_webtv_disk { get; set; }

        public WebTVDisk open_disk(WebTVDiskCollationConverter byte_converter = null, FileAccess access = FileAccess.Read, DiskByteTransform target_byte_transform = DiskByteTransform.NOSWAP)
        {
            try
            {
                var disk = new WebTVDisk()
                {
                    name = this.title,
                    path = this.device_id,
                    size_bytes = this.size_bytes,
                    sector_bytes_length = this.sector_bytes_length
                };

                disk.io = new WebTVDiskIO(disk, byte_converter, access, target_byte_transform);

                return disk;
            }
            catch
            {
                return null;
            }

        }
    }

    public class DiskWMIEntries : ObservableCollection<DiskWMIEntry>
    {
    }

    //[Provider("CIMWin32")]class Win32_DiskDrive : CIM_DiskDrive
    //{
    //  uint16   Availability;
    //  uint32   BytesPerSector;
    //  uint16   Capabilities[];
    //  string   CapabilityDescriptions[];
    //  string   Caption;
    //  string   CompressionMethod;
    //  uint32   ConfigManagerErrorCode;
    //  boolean  ConfigManagerUserConfig;
    //  string   CreationClassName;
    //  uint64   DefaultBlockSize;
    //  string   Description;
    //  string   DeviceID;
    //  boolean  ErrorCleared;
    //  string   ErrorDescription;
    //  string   ErrorMethodology;
    //  string   FirmwareRevision;
    //  uint32   Index;
    //  datetime InstallDate;
    //  string   InterfaceType;
    //  uint32   LastErrorCode;
    //  string   Manufacturer;
    //  uint64   MaxBlockSize;
    //  uint64   MaxMediaSize;
    //  boolean  MediaLoaded;
    //  string   MediaType;
    //  uint64   MinBlockSize;
    //  string   Model;
    //  string   Name;
    //  boolean  NeedsCleaning;
    //  uint32   NumberOfMediaSupported;
    //  uint32   Partitions;
    //  string   PNPDeviceID;
    //  uint16   PowerManagementCapabilities[];
    //  boolean  PowerManagementSupported;
    //  uint32   SCSIBus;
    //  uint16   SCSILogicalUnit;
    //  uint16   SCSIPort;
    //  uint16   SCSITargetId;
    //  uint32   SectorsPerTrack;
    //  string   SerialNumber;
    //  uint32   Signature;
    //  uint64   Size;
    //  string   Status;
    //  uint16   StatusInfo;
    //  string   SystemCreationClassName;
    //  string   SystemName;
    //  uint64   TotalCylinders;
    //  uint32   TotalHeads;
    //  uint64   TotalSectors;
    //  uint64   TotalTracks;
    //  uint32   TracksPerCylinder;
    //};

    class WebTVDiskEnumerator
    {
        public bool has_webtv_layout(WebTVDisk disk)
        {
            var partition_table_offsets = WebTVPartitionManager.possible_partition_table_offsets();

            foreach (var partition_table_offset in partition_table_offsets)
            {
                var partition_manager = new WebTVPartitionManager(disk);

                if (partition_manager.is_valid_partition_table(partition_table_offset))
                {
                    return true;
                }
            }

            return false;
        }

        public bool has_webtv_layout(string path, ulong size_bytes, uint sector_bytes_length)
        {
            try
            {
                var disk = new WebTVDisk()
                {
                    path = path,
                    size_bytes = size_bytes,
                    sector_bytes_length = sector_bytes_length
                };

                disk.io = new WebTVDiskIO(disk);

                var _has_webtv_layout = this.has_webtv_layout(disk);

                disk.io.Close();

                return _has_webtv_layout;
            }
            catch
            {
                return false;
            }
        }

        public DiskSecurityStatus get_security_status(WebTVDisk disk)
        {
            return disk.io.get_security_status();
        }

        public DiskSecurityStatus get_security_status(string path, ulong size_bytes, uint sector_bytes_length)
        {
            try
            {
                var disk = new WebTVDisk()
                {
                    path = path,
                    size_bytes = size_bytes,
                    sector_bytes_length = sector_bytes_length
                };

                disk.io = new WebTVDiskIO(disk, null, System.IO.FileAccess.ReadWrite);

                var _get_security_status = this.get_security_status(disk);

                disk.io.Close();

                return _get_security_status;
            }
            catch
            {
                return DiskSecurityStatus.NONE;
            }
        }

        public DiskWMIEntries get_physical_disks(bool test_for_webtv_layout = false)
        {
            var physical_disks = new DiskWMIEntries();

            ManagementObjectSearcher wmi_disk_query = new ManagementObjectSearcher("SELECT DeviceID, Model, Name, Manufacturer, SerialNumber, FirmwareRevision, Status, Size, BytesPerSector FROM Win32_DiskDrive");

            foreach (ManagementObject wmi_disk in wmi_disk_query.Get())
            {
                try
                {
                    var title = "Title";
                    if (wmi_disk["Model"] != null)
                    {
                        title = (string)wmi_disk["Model"];
                    }
                    else
                    {
                        title = (string)wmi_disk["Name"];
                    }


                    var is_webtv_disk = false;
                    var security_info = "Not Supported";
                    var security_status = DiskSecurityStatus.NONE;
                    var is_security_locked = false;
                    if (test_for_webtv_layout)
                    {
                        security_status = this.get_security_status((string)wmi_disk["DeviceID"], (ulong)wmi_disk["Size"], (uint)wmi_disk["BytesPerSector"]);

                        if ((security_status & DiskSecurityStatus.SUPPORTED) == DiskSecurityStatus.SUPPORTED)
                        {
                            if ((security_status & DiskSecurityStatus.ENABLED) != DiskSecurityStatus.ENABLED)
                            {
                                security_info = "Disabled";
                            }
                            else
                            {
                                if ((security_status & DiskSecurityStatus.LOCKED) == DiskSecurityStatus.LOCKED)
                                {
                                    is_security_locked = true;
                                    security_info = "Locked";
                                }
                                else
                                {
                                    is_security_locked = false;
                                    security_info = "Not Locked";
                                }
                            }

                            if ((security_status & DiskSecurityStatus.FROZEN) == DiskSecurityStatus.FROZEN)
                            {
                                security_info += ", Frozen";
                            }

                            if ((security_status & DiskSecurityStatus.ATTEMPTS_EXCEEDED) == DiskSecurityStatus.ATTEMPTS_EXCEEDED)
                            {
                                security_info += ", Password Tries Exceeded";
                            }

                            if ((security_status & DiskSecurityStatus.HAS_ENHANCED_ERASE) == DiskSecurityStatus.HAS_ENHANCED_ERASE)
                            {
                                security_info += ", Has Enhanced Erase";
                            }

                            if ((security_status & DiskSecurityStatus.SECURITY_MAXIMUM) == DiskSecurityStatus.SECURITY_MAXIMUM)
                            {
                                security_info += ", Maximum Security";
                            }
                            else
                            {
                                security_info += ", High Security";
                            }
                        }


                        if (!is_security_locked)
                        {
                            is_webtv_disk = this.has_webtv_layout((string)wmi_disk["DeviceID"], (ulong)wmi_disk["Size"], (uint)wmi_disk["BytesPerSector"]);
                        }
                    }
                    else
                    {
                        is_webtv_disk = false;
                    }

                    physical_disks.Add(new DiskWMIEntry()
                    {
                        device_id = (string)wmi_disk["DeviceID"],
                        title = title,
                        model = (string)wmi_disk["Model"],
                        name = (string)wmi_disk["Name"],
                        manufacture = (string)wmi_disk["Manufacturer"],
                        serial_number = (string)wmi_disk["SerialNumber"],
                        firmware_revision = (string)wmi_disk["FirmwareRevision"],
                        security_info = security_info,
                        security_status = security_status,
                        is_security_locked = is_security_locked,
                        status = (string)wmi_disk["Status"],
                        size_bytes = (ulong)wmi_disk["Size"],
                        sector_bytes_length = (uint)wmi_disk["BytesPerSector"],
                        is_webtv_disk = is_webtv_disk
                    });
                }
                catch { }
            }

            return physical_disks;
        }
    }
}
