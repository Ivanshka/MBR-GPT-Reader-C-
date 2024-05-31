using Ext4ConsoleReader.HardDisk;
using Ext4ConsoleReader.PartitionTable.GPT;
using Ext4ConsoleReader.PartitionTable.MBR;
using System;

namespace Ext4ConsoleReader.Service
{
    public class PartitionTablePrinter
    {
        private PartitionTableService partitionTableService;

        public PartitionTablePrinter(PartitionTableService partitionTableService)
        {
            this.partitionTableService = partitionTableService;
        }

        public void PrintDisk(PhysicalDisk disk)
        {
            if (partitionTableService.IsGptDisk(disk))
            {
                PrintGptDisk(disk);
            }
            else
            {
                PrintMbrDisk(disk);
            }
        }

        private void PrintGptDisk(PhysicalDisk disk)
        {
            Console.WriteLine("Разметка диска: Guid Partition Table\n");

            GuidPartitionTable guidPartitionTable = partitionTableService.ReadGpt(disk);

            for (int i = 0; i < guidPartitionTable.Partitions.Count; i++)
            {
                GptPartition partition = guidPartitionTable.Partitions[i];

                string size = GetPartitionSizeString(partition.GetPartitionSize());

                Console.WriteLine($"Логический диск #{i + 1}");
                Console.WriteLine($"Имя: {partition.PartitionName()}");
                Console.WriteLine($"GUID раздела: {partition.GetPartitionGuid()}");
                Console.WriteLine($"GUID типа раздела: {partition.GetPartitionTypeGuid()}");
                Console.WriteLine($"Типа раздела: {partitionTableService.GetPartitionTypeDescription(partition)}");
                Console.WriteLine($"Размер раздела: {size}\n");
            }
        }

        private string GetPartitionSizeString(ulong size)
        {
            if (size > 1_073_741_824)
                return Math.Round(size / 1_073_741_824f, 2) + " GB";
            else
                return Math.Round(size / 1048576f, 2) + " MB";
        }

        private void PrintMbrDisk(PhysicalDisk disk)
        {
            Console.WriteLine("Разметка диска: Master Boot Record\n");

            MasterBootRecord masterBootRecord = partitionTableService.ReadMbr(disk);

            for (int i = 0; i < masterBootRecord.Partitions.Count; i++)
            {
                MbrPartition partition = masterBootRecord.Partitions[i];

                string isActive = partition.IsActive() ? "да" : "нет";

                Console.WriteLine($"Логический диск #{i + 1}");
                Console.WriteLine($"Тип: {partition.Type()}");
                Console.WriteLine($"Активен: {isActive}");
                Console.WriteLine($"Количество секторов: {partition.PartitionSectorAmount}\n");
            }
        }
    }
}
