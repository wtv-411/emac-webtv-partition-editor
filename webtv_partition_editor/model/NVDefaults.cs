using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace webtv_partition_editor
{
    static class NVDefaults
    {
        static Dictionary<string, NVSetting> defaults = new Dictionary<string, NVSetting>()
        {
            {
                "tlly", new NVSetting()
                        {
                            name = "tlly",
                            title = "TellyScript",
                            notes = "ReadTellyScriptFromNV",
                            data_type = NVDataType.BINARY_BLOB,
                            data_editor = NVDataEditor.FILE_EDITOR,
                        }
            },
            {
                "hn", new NVSetting()
                        {
                            name = "hn",
                            title = "Head-waiter Host",
                            notes = "Network::RestoreState",
                            data_type = NVDataType.NULL_TERMINATED_STRING,
                            data_editor = NVDataEditor.STRING_EDITOR,
                        }
            },
            {
                "hp", new NVSetting()
                        {
                            name = "hp",
                            title = "Head-waiter Port",
                            notes = "Network::RestoreState",
                            data_type = NVDataType.UINT16,
                            data_editor = NVDataEditor.INTEGER_EDITOR,
                        }
            },
            {
                "hf", new NVSetting()
                        {
                            name = "hf",
                            title = "Head-waiter Flags",
                            notes = "Network::RestoreState",
                            data_type = NVDataType.UINT32,
                            data_editor = NVDataEditor.HEX_EDITOR,
                        }
            },
            {
                "hl", new NVSetting()
                        {
                            name = "hl",
                            title = "Head-waiter Limit",
                            notes = "Network::RestoreState",
                            data_type = NVDataType.CHAR,
                            data_editor = NVDataEditor.INTEGER_EDITOR,
                        }
            },
            {
                "*n", new NVSetting()
                        {
                            name = "hf",
                            title = "Star Host",
                            notes = "Network::RestoreState",
                            data_type = NVDataType.NULL_TERMINATED_STRING,
                            data_editor = NVDataEditor.STRING_EDITOR,
                        }
            },
            {
                "*p", new NVSetting()
                        {
                            name = "*p",
                            title = "Star Port",
                            notes = "Network::RestoreState",
                            data_type = NVDataType.UINT16,
                            data_editor = NVDataEditor.INTEGER_EDITOR,
                        }
            },
            {
                "*f", new NVSetting()
                        {
                            name = "*f",
                            title = "Star Flags",
                            notes = "Network::RestoreState",
                            data_type = NVDataType.UINT32,
                            data_editor = NVDataEditor.HEX_EDITOR,
                        }
            },
            {
                "*l", new NVSetting()
                        {
                            name = "*l",
                            title = "Star Limit",
                            notes = "Network::RestoreState",
                            data_type = NVDataType.CHAR,
                            data_editor = NVDataEditor.INTEGER_EDITOR,
                        }
            },


            {
                "FLip", new NVSetting()
                        {
                            name = "FLip",
                            title = "Flash IP",
                            notes = "UpdateFlash",
                            data_type = NVDataType.UINT32,
                            data_editor = NVDataEditor.IP_EDITOR,
                        }
            },
            {
                "FLpo", new NVSetting()
                        {
                            name = "FLpo",
                            title = "Flash Port",
                            notes = "UpdateFlash",
                            data_type = NVDataType.UINT16,
                            data_editor = NVDataEditor.INTEGER_EDITOR,
                        }
            },
            {
                "FLth", new NVSetting()
                        {
                            name = "FLth",
                            title = "Flash Path",
                            notes = "UpdateFlash",
                            data_type = NVDataType.NULL_TERMINATED_STRING,
                            data_editor = NVDataEditor.STRING_EDITOR,
                        }
            },



            {
                "boot", new NVSetting()
                        {
                            name = "boot",
                            title = "Boot URL",
                            notes = "Network::RestoreBootURL/Network::RestoreState",
                            data_type = NVDataType.NULL_TERMINATED_STRING,
                            data_editor = NVDataEditor.STRING_EDITOR,
                        }
            },
            {
                "wtch", new NVSetting()
                        {
                            name = "wtch",
                            title = "Message Watch URL",
                            notes = "Network::RestoreState",
                            data_type = NVDataType.NULL_TERMINATED_STRING,
                            data_editor = NVDataEditor.STRING_EDITOR,
                        }
            },
            
            {
                "TOUR", new NVSetting()
                        {
                            name = "TOUR",
                            title = "Tourist Enabled",
                            notes = "RestoreTouristState",
                            data_type = NVDataType.BOOLEAN,
                            data_editor = NVDataEditor.BOOLEAN_EDITOR,
                        }
            },
            {
                "DLLI", new NVSetting()
                        {
                            name = "DLLI",
                            title = "Download Login URL",
                            notes = "Network::RestoreState",
                            data_type = NVDataType.NULL_TERMINATED_STRING,
                            data_editor = NVDataEditor.STRING_EDITOR,
                        }
            },
            {
                "DLLS", new NVSetting()
                        {
                            name = "DLLS",
                            title = "Download List URL",
                            notes = "Network::RestoreState",
                            data_type = NVDataType.NULL_TERMINATED_STRING,
                            data_editor = NVDataEditor.STRING_EDITOR,
                        }
            },
            {
                "Dial", new NVSetting()
                        {
                            name = "Dial",
                            title = "Working Dial Number",
                            notes = "Network::RestoreState",
                            data_type = NVDataType.NULL_TERMINATED_STRING,
                            data_editor = NVDataEditor.STRING_EDITOR,
                        }
            },
            {
                "FONE", new NVSetting()
                        {
                            name = "FONE",
                            title = "Phone Settings",
                            notes = "InitPhoneSettings/InstallPhoneSettings/Network::RestoreState",
                            data_type = NVDataType.BINARY_BLOB,
                            data_editor = NVDataEditor.FILE_EDITOR,
                        }
            },
            {
                "ANI ", new NVSetting()
                        {
                            name = "ANI ",
                            title = "Telephone number ID (ANI)",
                            notes = "Network::RestoreState",
                            data_type = NVDataType.NULL_TERMINATED_STRING,
                            data_editor = NVDataEditor.STRING_EDITOR,
                        }
            },
            {
                "SEID", new NVSetting()
                        {
                            name = "SEID",
                            title = "Metering Session ID",
                            notes = "Network::RestoreMeteringDataFromNVRam",
                            data_type = NVDataType.BINARY_BLOB,
                            data_editor = NVDataEditor.AUTODETECT,
                        }
            },
            {
                "MCTM", new NVSetting()
                        {
                            name = "MCTM",
                            title = "Metering Charged Time",
                            notes = "Network::RestoreMeteringDataFromNVRam",
                            data_type = NVDataType.BINARY_BLOB,
                            data_editor = NVDataEditor.AUTODETECT,
                        }
            },
            {
                "MFTM", new NVSetting()
                        {
                            name = "MFTM",
                            title = "Metering Free Time",
                            notes = "Network::RestoreMeteringDataFromNVRam",
                            data_type = NVDataType.BINARY_BLOB,
                            data_editor = NVDataEditor.AUTODETECT,
                        }
            },
            {
                "clog", new NVSetting()
                        {
                            name = "clog",
                            title = "Phone Call Log",
                            notes = "InitializePhoneLog",
                            data_type = NVDataType.BINARY_BLOB,
                            data_editor = NVDataEditor.AUTODETECT,
                        }
            },
            {
                "TZNA", new NVSetting()
                        {
                            name = "TZNA",
                            title = "Time Zone Name",
                            notes = "Clock::RestoreState",
                            data_type = NVDataType.NULL_TERMINATED_STRING,
                            data_editor = NVDataEditor.STRING_EDITOR,
                        }
            },
            {
                "TZOF", new NVSetting()
                        {
                            name = "TZOF",
                            title = "Time Zone Offset",
                            notes = "Clock::RestoreState",
                            data_type = NVDataType.BINARY_BLOB,
                            data_editor = NVDataEditor.AUTODETECT,
                        }
            },
            {
                "TZDR", new NVSetting()
                        {
                            name = "TZDR",
                            title = "Time Zone DST Rule",
                            notes = "",
                            data_type = NVDataType.BINARY_BLOB,
                            data_editor = NVDataEditor.AUTODETECT,
                        }
            },
            {
                "D3EK", new NVSetting()
                        {
                            name = "D3EK",
                            title = "Cookie Encryption Key",
                            notes = "",
                            data_type = NVDataType.BINARY_BLOB,
                            data_editor = NVDataEditor.HEX_EDITOR,
                        }
            },
            {
                "DLOF", new NVSetting()
                        {
                            name = "DLOF",
                            title = "Data Download Check Time Offset",
                            notes = "",
                            data_type = NVDataType.BINARY_BLOB,
                            data_editor = NVDataEditor.AUTODETECT,
                        }
            },
            {
                "MWOF", new NVSetting()
                        {
                            name = "MWOF",
                            title = "Message Watch Check Time Offset",
                            notes = "AlarmClock::RestoreState",
                            data_type = NVDataType.BINARY_BLOB,
                            data_editor = NVDataEditor.AUTODETECT,
                        }
            },
            {
                "DLOC", new NVSetting()
                        {
                            name = "DLOC",
                            title = "Data Download Enabled",
                            notes = "",
                            data_type = NVDataType.BINARY_BLOB,
                            data_editor = NVDataEditor.AUTODETECT,
                        }
            },
            {
                "TVSS", new NVSetting()
                        {
                            name = "TVSS",
                            title = "TV Signal Source",
                            notes = "System::RestoreStatePhase2",
                            data_type = NVDataType.BINARY_BLOB,
                            data_editor = NVDataEditor.AUTODETECT,
                        }
            },
            {
                "TVAL", new NVSetting()
                        {
                            name = "TVAL",
                            title = "TV Listings Auto-download (Old)",
                            notes = "",
                            data_type = NVDataType.BINARY_BLOB,
                            data_editor = NVDataEditor.AUTODETECT,
                        }
            },
            {
                "UPRQ", new NVSetting()
                        {
                            name = "UPRQ",
                            title = "Upgrade Path",
                            notes = "RestoreUpgradePath",
                            data_type = NVDataType.NULL_TERMINATED_STRING,
                            data_editor = NVDataEditor.STRING_EDITOR,
                        }
            },
            {
                "TVDL", new NVSetting()
                        {
                            name = "TVDL",
                            title = "TV Listings Auto-download",
                            notes = "",
                            data_type = NVDataType.BINARY_BLOB,
                            data_editor = NVDataEditor.AUTODETECT,
                        }
            },
            {
                "TVZP", new NVSetting()
                        {
                            name = "TVZP",
                            title = "TV Zip Code",
                            notes = "System::RestoreStatePhase1",
                            data_type = NVDataType.BINARY_BLOB,
                            data_editor = NVDataEditor.AUTODETECT,
                        }
            },
            {
                "TVHM", new NVSetting()
                        {
                            name = "TVHM",
                            title = "TV Home State",
                            notes = "TVHome::RestoreState",
                            data_type = NVDataType.BINARY_BLOB,
                            data_editor = NVDataEditor.AUTODETECT,
                        }
            },
            {
                "INTR", new NVSetting()
                        {
                            name = "INTR",
                            title = "TV Home Intro Version",
                            notes = "TVHome::RestoreState",
                            data_type = NVDataType.BINARY_BLOB,
                            data_editor = NVDataEditor.AUTODETECT,
                        }
            },
            {
                "TVCH", new NVSetting()
                        {
                            name = "TVCH",
                            title = "Current TV Channel",
                            notes = "TVController::RestoreState",
                            data_type = NVDataType.BINARY_BLOB,
                            data_editor = NVDataEditor.AUTODETECT,
                        }
            },
            {
                "TVLH", new NVSetting()
                        {
                            name = "TVLH",
                            title = "TV List Headend",
                            notes = "",
                            data_type = NVDataType.BINARY_BLOB,
                            data_editor = NVDataEditor.AUTODETECT,
                        }
            },
            {
                "TVEC", new NVSetting()
                        {
                            name = "TVEC",
                            title = "TV Channel Record Flags",
                            notes = "TVController::RestoreState",
                            data_type = NVDataType.BINARY_BLOB,
                            data_editor = NVDataEditor.AUTODETECT,
                        }
            },
            {
                "TVBI", new NVSetting()
                        {
                            name = "TVBI",
                            title = "TV IR Blaster Info",
                            notes = "TVController::RestoreState",
                            data_type = NVDataType.BINARY_BLOB,
                            data_editor = NVDataEditor.AUTODETECT,
                        }
            },
            {
                "CAPW", new NVSetting()
                        {
                            name = "CAPW",
                            title = "TV Access Control/CAM Password",
                            notes = "TVAccess::RestoreState",
                            data_type = NVDataType.NULL_TERMINATED_STRING,
                            data_editor = NVDataEditor.STRING_EDITOR,
                        }
            },
            {
                "CAAL", new NVSetting()
                        {
                            name = "CAAL",
                            title = "TV Access Control/CAM 'Not Rated' Blocked",
                            notes = "TVAccess::RestoreState",
                            data_type = NVDataType.BINARY_BLOB,
                            data_editor = NVDataEditor.AUTODETECT,
                        }
            },
            {
                "CAPL", new NVSetting()
                        {
                            name = "CAPL",
                            title = "TV Access Control/CAM Panel Locked",
                            notes = "TVAccess::RestoreState",
                            data_type = NVDataType.BINARY_BLOB,
                            data_editor = NVDataEditor.AUTODETECT,
                        }
            },
            {
                "CAVL", new NVSetting()
                        {
                            name = "CAVL",
                            title = "TV Access Control/CAM PPV Locked",
                            notes = "TVAccess::RestoreState",
                            data_type = NVDataType.BINARY_BLOB,
                            data_editor = NVDataEditor.AUTODETECT,
                        }
            },
            {
                "CASL", new NVSetting()
                        {
                            name = "CASL",
                            title = "TV Access Control/CAM PPV Spending Limit",
                            notes = "TVAccess::RestoreState",
                            data_type = NVDataType.BINARY_BLOB,
                            data_editor = NVDataEditor.AUTODETECT,
                        }
            },
            {
                "GALK", new NVSetting()
                        {
                            name = "GALK",
                            title = "Game Access Locked",
                            notes = "TVAccess::RestoreState",
                            data_type = NVDataType.BINARY_BLOB,
                            data_editor = NVDataEditor.AUTODETECT,
                        }
            },
            {
                "GARA", new NVSetting()
                        {
                            name = "GARA",
                            title = "Game Access ESRB Rating Limit",
                            notes = "TVAccess::RestoreState",
                            data_type = NVDataType.BINARY_BLOB,
                            data_editor = NVDataEditor.AUTODETECT,
                        }
            },
            {
                "CAER", new NVSetting()
                        {
                            name = "CAER",
                            title = "TV Access/CAM Extended Rating Limits",
                            notes = "TVAccess::RestoreState",
                            data_type = NVDataType.BINARY_BLOB,
                            data_editor = NVDataEditor.AUTODETECT,
                        }
            },
            {
                "CATR", new NVSetting()
                        {
                            name = "CATR",
                            title = "TV Access/CAM TV Rating Limit",
                            notes = "TVAccess::RestoreState",
                            data_type = NVDataType.BINARY_BLOB,
                            data_editor = NVDataEditor.AUTODETECT,
                        }
            },
            {
                "CAMR", new NVSetting()
                        {
                            name = "CAMR",
                            title = "TV Access/CAM MPAA Rating Limit",
                            notes = "TVAccess::RestoreState",
                            data_type = NVDataType.BINARY_BLOB,
                            data_editor = NVDataEditor.AUTODETECT,
                        }
            },
            
            
            
            {
                "DVRP", new NVSetting()
                        {
                            name = "DVRP",
                            title = "????",
                            notes = "DVRInterface::RestoreState",
                            data_type = NVDataType.BINARY_BLOB,
                            data_editor = NVDataEditor.AUTODETECT,
                        }
            },
            {
                "DVRK", new NVSetting()
                        {
                            name = "DVRK",
                            title = "DVR MPEG ID",
                            notes = "DVRInterface::RestoreState",
                            data_type = NVDataType.BINARY_BLOB,
                            data_editor = NVDataEditor.AUTODETECT,
                        }
            },
            {
                "DigA", new NVSetting()
                        {
                            name = "DigA",
                            title = "Digital Audio Mode",
                            notes = "DigitalAudioRestoreState",
                            data_type = NVDataType.BINARY_BLOB,
                            data_editor = NVDataEditor.AUTODETECT,
                        }
            },
            
            {
                "TVC2", new NVSetting()
                        {
                            name = "TVC2",
                            title = "TV Channel Flags",
                            notes = "TVController::RestoreState",
                            data_type = NVDataType.BINARY_BLOB,
                            data_editor = NVDataEditor.AUTODETECT,
                        }
            },
            {
                "TVCM", new NVSetting()
                        {
                            name = "TVCM",
                            title = "TV Channel Mode",
                            notes = "TVController::RestoreState",
                            data_type = NVDataType.BINARY_BLOB,
                            data_editor = NVDataEditor.AUTODETECT,
                        }
            },
            {
                "RCNT", new NVSetting()
                        {
                            name = "RCNT",
                            title = "Recent TV Channels",
                            notes = "TVRecent::RestoreState",
                            data_type = NVDataType.BINARY_BLOB,
                            data_editor = NVDataEditor.AUTODETECT,
                        }
            },
            {
                "TVLD", new NVSetting()
                        {
                            name = "TVLD",
                            title = "TV Log Disabled Mask",
                            notes = "TVLog::Open",
                            data_type = NVDataType.BINARY_BLOB,
                            data_editor = NVDataEditor.AUTODETECT,
                        }
            },
            {
                "VRSI", new NVSetting()
                        {
                            name = "VRSI",
                            title = "VCR Controller IR Setup Info",
                            notes = "VCRController::RestoreState",
                            data_type = NVDataType.BINARY_BLOB,
                            data_editor = NVDataEditor.AUTODETECT,
                        }
            },
            {
                "VRON", new NVSetting()
                        {
                            name = "VRON",
                            title = "VCR Controller IR Enabled",
                            notes = "VCRController::RestoreState",
                            data_type = NVDataType.BOOLEAN,
                            data_editor = NVDataEditor.BOOLEAN_EDITOR,
                        }
            },
            {
                "MLOC", new NVSetting()
                        {
                            name = "MLOC",
                            title = "Movie Search URL",
                            notes = "TVDatabase::RestoreState",
                            data_type = NVDataType.BINARY_BLOB,
                            data_editor = NVDataEditor.AUTODETECT,
                        }
            },
            {
                "IRBD", new NVSetting()
                        {
                            name = "IRBD",
                            title = "IR Blaster Delay",
                            notes = "IRBlasterDB::RestoreDelay",
                            data_type = NVDataType.BINARY_BLOB,
                            data_editor = NVDataEditor.AUTODETECT,
                        }
            },
            {
                "IRCS", new NVSetting()
                        {
                            name = "IRCS",
                            title = "IR Blaster Code Set History",
                            notes = "IRBlasterDB::RestoreCodeSetHistory",
                            data_type = NVDataType.BINARY_BLOB,
                            data_editor = NVDataEditor.AUTODETECT,
                        }
            },
            {
                "PWRF", new NVSetting()
                        {
                            name = "PWRF",
                            title = "Power on Flags",
                            notes = "System::PowerOnLoop/System::RestoreStatePhase1/System::RestoreStatePhase2",
                            data_type = NVDataType.BINARY_BLOB,
                            data_editor = NVDataEditor.AUTODETECT,
                        }
            },
            {
                "COPW", new NVSetting()
                        {
                            name = "COPW",
                            title = "Connect On Power Enabled",
                            notes = "System::RestoreStatePhase1",
                            data_type = NVDataType.BOOLEAN,
                            data_editor = NVDataEditor.BOOLEAN_EDITOR,
                        }
            },
            {
                "COPU", new NVSetting()
                        {
                            name = "COPU",
                            title = "Connect On Power URL",
                            notes = "System::RestoreStatePhase1",
                            data_type = NVDataType.BINARY_BLOB,
                            data_editor = NVDataEditor.AUTODETECT,
                        }
            },
            {
                "CORF", new NVSetting()
                        {
                            name = "CORF",
                            title = "????",
                            notes = "",
                            data_type = NVDataType.BINARY_BLOB,
                            data_editor = NVDataEditor.AUTODETECT,
                        }
            },

            {
                "PrSt", new NVSetting()
                        {
                            name = "PrSt",
                            title = "????",
                            notes = "System::RestoreStatePhase2",
                            data_type = NVDataType.BINARY_BLOB,
                            data_editor = NVDataEditor.AUTODETECT,
                        }
            },


            {
                "TLCK", new NVSetting()
                        {
                            name = "TLCK",
                            title = "TV Access/CAM Blocked Channels Temoraily Unlocked???",
                            notes = "TVAccess::RestoreState",
                            data_type = NVDataType.BOOLEAN,
                            data_editor = NVDataEditor.BOOLEAN_EDITOR,
                        }
            },
            {
                "LOCK", new NVSetting()
                        {
                            name = "LOCK",
                            title = "TV Access/CAM Blocked Channels Enabled",
                            notes = "TVAccess::RestoreState",
                            data_type = NVDataType.BOOLEAN,
                            data_editor = NVDataEditor.BOOLEAN_EDITOR,
                        }
            },
            {
                "BRDR", new NVSetting()
                        {
                            name = "BRDR",
                            title = "Screen Border Color",
                            notes = "System::RestoreStatePhase2",
                            data_type = NVDataType.BINARY_BLOB,
                            data_editor = NVDataEditor.AUTODETECT,
                        }
            },
            {
                "locp", new NVSetting()
                        {
                            name = "locp",
                            title = "Local Dialup POP Count",
                            notes = "System::RestoreStatePhase2",
                            data_type = NVDataType.BINARY_BLOB,
                            data_editor = NVDataEditor.AUTODETECT,
                        }
            },
            {
                "ITVE", new NVSetting()
                        {
                            name = "ITVE",
                            title = "iTV Enabled",
                            notes = "System::RestoreStatePhase2",
                            data_type = NVDataType.BOOLEAN,
                            data_editor = NVDataEditor.BOOLEAN_EDITOR,
                        }
            },
            {
                "SPos", new NVSetting()
                        {
                            name = "SPos",
                            title = "Screen Position",
                            notes = "VideoDriver::RestoreDisplayState",
                            data_type = NVDataType.BINARY_BLOB,
                            data_editor = NVDataEditor.AUTODETECT,
                        }
            },
            {
                "CHAN", new NVSetting()
                        {
                            name = "CHAN",
                            title = "Stored TV Tuner Band",
                            notes = "",
                            data_type = NVDataType.BINARY_BLOB,
                            data_editor = NVDataEditor.AUTODETECT,
                        }
            },

            {
                "LANG", new NVSetting()
                        {
                            name = "LANG",
                            title = "????",
                            notes = "UltimateTV",
                            data_type = NVDataType.BINARY_BLOB,
                            data_editor = NVDataEditor.AUTODETECT,
                        }
            },
            {
                "DVRM", new NVSetting()
                        {
                            name = "DVRM",
                            title = "????",
                            notes = "UltimateTV",
                            data_type = NVDataType.BINARY_BLOB,
                            data_editor = NVDataEditor.AUTODETECT,
                        }
            },
            {
                "FMod", new NVSetting()
                        {
                            name = "FMod",
                            title = "????",
                            notes = "UltimateTV",
                            data_type = NVDataType.BINARY_BLOB,
                            data_editor = NVDataEditor.AUTODETECT,
                        }
            },
            {
                "CAMP", new NVSetting()
                        {
                            name = "CAMP",
                            title = "????",
                            notes = "UltimateTV",
                            data_type = NVDataType.BINARY_BLOB,
                            data_editor = NVDataEditor.AUTODETECT,
                        }
            },
            {
                "CATV", new NVSetting()
                        {
                            name = "CATV",
                            title = "????",
                            notes = "UltimateTV",
                            data_type = NVDataType.BINARY_BLOB,
                            data_editor = NVDataEditor.AUTODETECT,
                        }
            },
            {
                "CAEX", new NVSetting()
                        {
                            name = "CAEX",
                            title = "????",
                            notes = "UltimateTV",
                            data_type = NVDataType.BINARY_BLOB,
                            data_editor = NVDataEditor.AUTODETECT,
                        }
            },
            {
                "CANR", new NVSetting()
                        {
                            name = "CANR",
                            title = "????",
                            notes = "UltimateTV",
                            data_type = NVDataType.BINARY_BLOB,
                            data_editor = NVDataEditor.AUTODETECT,
                        }
            },
            {
                "WEBL", new NVSetting()
                        {
                            name = "WEBL",
                            title = "????",
                            notes = "UltimateTV",
                            data_type = NVDataType.BINARY_BLOB,
                            data_editor = NVDataEditor.AUTODETECT,
                        }
            },
            {
                "RDVR", new NVSetting()
                        {
                            name = "RDVR",
                            title = "????",
                            notes = "UltimateTV",
                            data_type = NVDataType.BINARY_BLOB,
                            data_editor = NVDataEditor.AUTODETECT,
                        }
            },
            {
                "RDVL", new NVSetting()
                        {
                            name = "RDVL",
                            title = "????",
                            notes = "UltimateTV",
                            data_type = NVDataType.BINARY_BLOB,
                            data_editor = NVDataEditor.AUTODETECT,
                        }
            },
            {
                "WGPS", new NVSetting()
                        {
                            name = "WGPS",
                            title = "????",
                            notes = "UltimateTV",
                            data_type = NVDataType.BINARY_BLOB,
                            data_editor = NVDataEditor.AUTODETECT,
                        }
            },
            {
                "AUDL", new NVSetting()
                        {
                            name = "AUDL",
                            title = "????",
                            notes = "UltimateTV",
                            data_type = NVDataType.BINARY_BLOB,
                            data_editor = NVDataEditor.AUTODETECT,
                        }
            },
            {
                "AUDD", new NVSetting()
                        {
                            name = "AUDD",
                            title = "????",
                            notes = "UltimateTV",
                            data_type = NVDataType.BINARY_BLOB,
                            data_editor = NVDataEditor.AUTODETECT,
                        }
            },
            {
                "Anam", new NVSetting()
                        {
                            name = "Anam",
                            title = "????",
                            notes = "UltimateTV",
                            data_type = NVDataType.BINARY_BLOB,
                            data_editor = NVDataEditor.AUTODETECT,
                        }
            },
            {
                "JOYY", new NVSetting()
                        {
                            name = "JOYY",
                            title = "????",
                            notes = "UltimateTV",
                            data_type = NVDataType.BINARY_BLOB,
                            data_editor = NVDataEditor.AUTODETECT,
                        }
            },
            {
                "DKON", new NVSetting()
                        {
                            name = "DKON",
                            title = "????",
                            notes = "UltimateTV",
                            data_type = NVDataType.BINARY_BLOB,
                            data_editor = NVDataEditor.AUTODETECT,
                        }
            },
            {
                "REGD", new NVSetting()
                        {
                            name = "REGD",
                            title = "????",
                            notes = "UltimateTV",
                            data_type = NVDataType.BINARY_BLOB,
                            data_editor = NVDataEditor.AUTODETECT,
                        }
            },
            {
                "CHTV", new NVSetting()
                        {
                            name = "CHTV",
                            title = "????",
                            notes = "UltimateTV",
                            data_type = NVDataType.BINARY_BLOB,
                            data_editor = NVDataEditor.AUTODETECT,
                        }
            },
            {
                "UPIN", new NVSetting()
                        {
                            name = "UPIN",
                            title = "????",
                            notes = "UltimateTV",
                            data_type = NVDataType.BINARY_BLOB,
                            data_editor = NVDataEditor.AUTODETECT,
                        }
            },
            {
                "UPVR", new NVSetting()
                        {
                            name = "UPVR",
                            title = "????",
                            notes = "UltimateTV",
                            data_type = NVDataType.BINARY_BLOB,
                            data_editor = NVDataEditor.AUTODETECT,
                        }
            },
            {
                "HSCI", new NVSetting()
                        {
                            name = "HSCI",
                            title = "High Speed????",
                            notes = "UltimateTV",
                            data_type = NVDataType.BOOLEAN,
                            data_editor = NVDataEditor.BOOLEAN_EDITOR,
                        }
            },
            {
                "BILD", new NVSetting()
                        {
                            name = "BILD",
                            title = "????",
                            notes = "UltimateTV",
                            data_type = NVDataType.BINARY_BLOB,
                            data_editor = NVDataEditor.AUTODETECT,
                        }
            },


        };
    }
}
