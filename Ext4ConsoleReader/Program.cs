using Ext4ConsoleReader.HardDisk;
using Ext4ConsoleReader.Service;
using System;
using System.Collections.Generic;

namespace Ext4ConsoleReader
{
    internal class Program
    {
        static void Main()
        {
            // https://github.com/Bobandy/SimpleConsoleMenu

            PartitionTableService tableService = new PartitionTableService();
            PartitionTablePrinter tablePrinter = new PartitionTablePrinter(tableService);

            PhysicalDisk disk = SelectPhysicalDisk();

            Console.WriteLine($"Диск выбран!\n");

            PrintPhysicalDiskInfo(disk);

            Console.WriteLine("Читаем список дисков...\n");

            tablePrinter.PrintDisk(disk);

            Console.ReadLine();
        }

        private static PhysicalDisk SelectPhysicalDisk()
        {
            List<PhysicalDisk> physicalDisks = DiskManager.GetPhysicalDisks();
            int selectedDisk;

            Console.WriteLine("Установлены следующие диски:");
            for (int i = 0; i < physicalDisks.Count; i++)
            {
                PhysicalDisk physicalDisk = physicalDisks[i];
                Console.WriteLine($"{i + 1}) {physicalDisk.Model}");
            }

            do
            {
                selectedDisk = SelectDiskFromUser();
            } while (selectedDisk < 1 || selectedDisk > physicalDisks.Count);

            return physicalDisks[selectedDisk - 1];
        }

        private static void PrintPhysicalDiskInfo(PhysicalDisk disk)
        {
            Console.WriteLine($"Производитель: {disk.Manufacturer}");
            Console.WriteLine($"Модель: {disk.Model}");
            Console.WriteLine($"Размер диска: {disk.Size} байт");
            Console.WriteLine($"Байт на сектор: {disk.BytesPerSector}");
            Console.WriteLine($"Количество разделов: {disk.PartitionAmount}");
            Console.WriteLine($"Индекс устройства в системе: {disk.Index}");
            Console.WriteLine($"ID устройства в системе: {disk.DeviceId}\n");
        }

        private static int SelectDiskFromUser()
        {
            int selectedDisk;
            string input;

            do
            {
                Console.Write("Введите номер диска для анализа: ");
                input = Console.ReadLine();
            } while (!int.TryParse(input, out selectedDisk));

            return selectedDisk;
        }
    }
}
