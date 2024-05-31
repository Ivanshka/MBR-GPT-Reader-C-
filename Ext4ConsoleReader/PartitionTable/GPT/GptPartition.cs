using System;
using System.Collections.Generic;
using System.Text;

namespace Ext4ConsoleReader.PartitionTable.GPT
{
    public class GptPartition
    {
        private static Dictionary<string, GptPartitionType> PARTITION_TYPE_GUID_TO_TYPE = new Dictionary<string, GptPartitionType>()
        {
            // without platform
            { "024DEE41-33E7-11D3-9D69-0008C781F39F", GptPartitionType.MBR_PARTITION_SCHEME },
            { "C12A7328-F81F-11D2-BA4B-00A0C93EC93B", GptPartitionType.EFI_SYSTEM_PARTITION },
            { "21686148-6449-6E6F-744E-656564454649", GptPartitionType.BIOS_BOOT_PARTITION },
            { "D3BFE2DE-3DAF-11DF-BA40-E3A556D89593", GptPartitionType.INTEL_FAST_FLASH_PARTITION },
            { "F4019732-066E-4E12-8273-346C5641494F", GptPartitionType.SONY_BOOT_PARTITION },
            { "BFBFAFE7-A34F-448A-9A5B-6213EB736C22", GptPartitionType.LENOVO_BOOT_PARTITION },
            // Windows
            { "E3C9E316-0B5C-4DB8-817D-F92DF00215AE", GptPartitionType.MICROSOFT_RESERVED_PARTITION },
            { "EBD0A0A2-B9E5-4433-87C0-68B6B72699C7", GptPartitionType.BASIC_DATA_PARTITION },
            { "5808C8AA-7E8F-42E0-85D2-E1E90434CFB3", GptPartitionType.WINDOWS_LDM_METADATA_PARTITION },
            { "AF9B60A0-1431-4F62-BC68-3311714A69AD", GptPartitionType.WINDOWS_LDM_DATA_PARTITION },
            { "DE94BBA4-06D1-4D40-A16A-BFD50179D6AC", GptPartitionType.WINDOWS_RECOVERY_ENVIRONMENT },
            // Linux
            { "0FC63DAF-8483-4772-8E79-3D69D8477DE4", GptPartitionType.LINUX_FILESYSTEM_DATA },
            { "A19D880F-05FC-4D3B-A006-743F0F84911E", GptPartitionType.LINUX_RAID_PARTITION },
            { "0657FD6D-A4AB-43C4-84E5-0933C84B4F4F", GptPartitionType.LINUX_SWAP_PARTITION },
            { "E6D6D379-F507-44C2-A23C-238F2A3DF928", GptPartitionType.LINUX_LVM_PARTITION },
            { "933AC7E1-2EB4-4F13-B844-0E14E2AEF915", GptPartitionType.LINUX_HOME_PARTITION },
            { "3B8F8425-20E0-4F3B-907F-1A25A76F98E8", GptPartitionType.LINUX_SERVER_DATA_PARTITION },
            { "7FFEC5C9-2D00-49B7-8941-3EA10A5586B7", GptPartitionType.LINUX_PLAIN_DM_CRYPT_PARTITION },
            { "CA7D7CCB-63ED-4C53-861C-1742536059CC", GptPartitionType.LINUX_UNIFIED_KEY_SETUP_PARTITION },
            { "8DA63339-0007-60C0-C436-083AC8230908", GptPartitionType.LINUX_RESERVED }
        };

        byte[] rawPartitionTypeGuid = new byte[16]; // 16 bytes
        byte[] rawPartitionGuid = new byte[16]; // 16 bytes
        ulong firstLbaAddress;
        ulong lastLbaAddress;
        byte[] attributes = new byte[8]; // 8 bytes
        string partitionName; // 72 bytes

        public GptPartition(byte[] rawPartitionInfo)
        {
            Array.Copy(rawPartitionInfo, 0, rawPartitionTypeGuid, 0, 16);
            Array.Copy(rawPartitionInfo, 16, rawPartitionGuid, 0, 16);
            firstLbaAddress = BitConverter.ToUInt64(rawPartitionInfo, 32);
            lastLbaAddress = BitConverter.ToUInt64(rawPartitionInfo, 40);
            Array.Copy(rawPartitionInfo, 48, attributes, 0, 8);
            byte[] rawPartitionName = new byte[72];
            Array.Copy(rawPartitionInfo, 56, rawPartitionName, 0, 72);
            partitionName = Encoding.Unicode.GetString(rawPartitionName);
        }

        public byte[] RawTypeGuid() => rawPartitionTypeGuid;

        public byte[] RawPartitionGuid() => rawPartitionGuid;

        public ulong FirstLbaAddress() => firstLbaAddress;

        public ulong LastLbaAddress() => lastLbaAddress;

        public byte[] Attributes() => attributes;

        public string PartitionName() => partitionName;

        public string GetPartitionTypeGuid() => new Guid(rawPartitionTypeGuid).ToString().ToUpper();

        public string GetPartitionGuid() => new Guid(rawPartitionGuid).ToString().ToUpper();

        public bool GetPartitionType(out GptPartitionType partitionType) => PARTITION_TYPE_GUID_TO_TYPE.TryGetValue(GetPartitionTypeGuid(), out partitionType);

        public ulong GetPartitionSize() => (lastLbaAddress - firstLbaAddress + 1) * 512;
    }
}
