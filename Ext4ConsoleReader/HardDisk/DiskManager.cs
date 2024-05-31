using System;
using System.Collections.Generic;
using System.Management;

namespace Ext4ConsoleReader.HardDisk
{
    public class DiskManager
    {
        public static double GetPhysicalDiskCount()
        {
            var query = new ManagementObjectSearcher("SELECT * FROM Win32_DiskDrive");
            return query.Get().Count;
        }

        public static List<PhysicalDisk> GetPhysicalDisks()
        {
            List<PhysicalDisk> disks = new List<PhysicalDisk>();

            var query = new ManagementObjectSearcher("SELECT * FROM Win32_DiskDrive");
            foreach (var disk in query.Get())
            {
                uint bytesPerSector = 0;
                string caption = null;
                string model = null;
                string deviceId = null;
                string manufacturer = null;
                ulong size = 0;
                uint partitions = 0;
                uint index = 0;

                try { bytesPerSector = (uint)disk["BytesPerSector"]; } catch { }
                try { caption = (string)disk["Caption"]; } catch { }
                try { model = (string)disk["Model"]; } catch { }
                try { deviceId = (string)disk["DeviceID"]; } catch { }
                try { manufacturer = (string)disk["Manufacturer"]; } catch { }
                try { size = (ulong)disk["Size"]; } catch { }
                try { partitions = (uint)disk["Partitions"]; } catch { }
                try { index = (uint)disk["Index"]; } catch { }

                disks.Add(new PhysicalDisk(bytesPerSector, caption, model, deviceId, manufacturer, size, partitions, index));
            }

            return disks;
        }
    }
}
