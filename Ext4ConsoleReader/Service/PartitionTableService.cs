using DStream;
using Ext4ConsoleReader.HardDisk;
using System.IO;
using System;
using Ext4ConsoleReader.PartitionTable.GPT;
using Ext4ConsoleReader.PartitionTable.MBR;

namespace Ext4ConsoleReader.Service
{
    public class PartitionTableService
    {
        public GuidPartitionTable ReadGpt(PhysicalDisk disk)
        {
            int gptSize = 34 * 512;

            byte[] rawGpt = new byte[gptSize];

            using (DeviceStream deviceStream = new DeviceStream(disk.DeviceId, FileAccess.Read))
            {
                int readBytesAmount = deviceStream.Read(rawGpt, 0, gptSize);
                if (readBytesAmount != gptSize)
                    throw new Exception($"Fail when reading the first sectors! Read bytes != {gptSize} :(");
            }

            return new GuidPartitionTable(rawGpt);
        }

        public MasterBootRecord ReadMbr(PhysicalDisk disk)
        {
            int mbrSize = 512;

            byte[] rawMbr = new byte[mbrSize];

            using (DeviceStream deviceStream = new DeviceStream(disk.DeviceId, FileAccess.Read))
            {
                int readBytesAmount = deviceStream.Read(rawMbr, 0, mbrSize);
                if (readBytesAmount != mbrSize)
                    throw new Exception($"Fail when reading the first sector! Read bytes != {mbrSize} :(");
            }

            return new MasterBootRecord(rawMbr);
        }

        public bool IsGptDisk(PhysicalDisk disk) => ReadMbr(disk).IsGPT();

        public string GetPartitionTypeDescription(GptPartition partition)
        {
            string guid = partition.GetPartitionTypeGuid();

            string unsupportedMessage = $"Тип \"{guid}\" не распознан. Можете поискать его самостоятельно здесь (https://ru.wikipedia.org/wiki/Таблица_разделов_GUID#Идентификаторы_(GUID)_различных_типов_разделов) или здесь (https://en.wikipedia.org/wiki/GUID_Partition_Table#Partition_type_GUIDs)";

            GptPartitionType partitionType;

            bool isTypeSupported = partition.GetPartitionType(out partitionType);

            if (!isTypeSupported)
            {
                return unsupportedMessage;
            }

            switch (partitionType)
            {
                // without platform
                case GptPartitionType.MBR_PARTITION_SCHEME: return "Схема разделов MBR";
                case GptPartitionType.EFI_SYSTEM_PARTITION: return "Системный раздел EFI";
                case GptPartitionType.BIOS_BOOT_PARTITION: return "Загрузочный раздел BIOS";
                case GptPartitionType.INTEL_FAST_FLASH_PARTITION: return "Раздел Intel Fast Flash (iFFS) (для технологии Intel Rapid Start)";
                case GptPartitionType.SONY_BOOT_PARTITION: return "Загрузочный раздел Sony";
                case GptPartitionType.LENOVO_BOOT_PARTITION: return "Загрузочный раздел Lenovo";
                // Windows
                case GptPartitionType.MICROSOFT_RESERVED_PARTITION: return "Резервный раздел Microsoft (MSR)";
                case GptPartitionType.BASIC_DATA_PARTITION: return "Раздел основных данных (Windows)";
                case GptPartitionType.WINDOWS_LDM_METADATA_PARTITION: return "Менеджер логических томов, раздел метаданных (Windows)";
                case GptPartitionType.WINDOWS_LDM_DATA_PARTITION: return "Менеджер логических томов, раздел данных (Windows)";
                case GptPartitionType.WINDOWS_RECOVERY_ENVIRONMENT: return "Раздел восстановления Windows";
                // Linux
                case GptPartitionType.LINUX_FILESYSTEM_DATA: return "Раздел данных (Linux)";
                case GptPartitionType.LINUX_RAID_PARTITION: return "RAID раздел (Linux)";
                case GptPartitionType.LINUX_SWAP_PARTITION: return "Swap-раздел Linux";
                case GptPartitionType.LINUX_LVM_PARTITION: return "Раздел Менеджера логических томов (LVM, Linux)";
                case GptPartitionType.LINUX_HOME_PARTITION: return "Раздел /home (Linux)";
                case GptPartitionType.LINUX_SERVER_DATA_PARTITION: return "Раздел /srv (данные сервера, Linux)";
                case GptPartitionType.LINUX_PLAIN_DM_CRYPT_PARTITION: return "Раздел dm-crypt (Linux)";
                case GptPartitionType.LINUX_UNIFIED_KEY_SETUP_PARTITION: return "Раздел LUKS (Linux)";
                case GptPartitionType.LINUX_RESERVED: return "Зарезервировано (Linux)";
            }

            return unsupportedMessage;
        }
    }
}
