using System;

namespace Ext4ConsoleReader.PartitionTable.MBR
{
    public class MbrPartition
    {
        private byte isActive;
        private byte beginHead;
        private short beginSectorCylinder; // bits 0-5 are sector, bits 6-7 are head
        private byte type;
        private byte endHead;
        private short endSectorCylinder; // bits 0-5 are sector, bits 6-7 are head
        private int firstSectorOffset;
        private int partitionSectorAmount;

        public MbrPartition(byte[] partitionData)
        {
            isActive = partitionData[0];
            beginHead = partitionData[1];
            beginSectorCylinder = BitConverter.ToInt16(partitionData, 2);
            type = partitionData[4];
            endHead = partitionData[5];
            endSectorCylinder = BitConverter.ToInt16(partitionData, 6);
            firstSectorOffset = BitConverter.ToInt32(partitionData, 8);
            partitionSectorAmount = BitConverter.ToInt32(partitionData, 12);
        }

        public bool IsActive() => isActive == 0x80;

        public byte BeginHead => beginHead;

        public short BeginSectorCylinder => beginSectorCylinder;

        public byte RawType => type;

        public string Type()
        {
            switch (type)
            {
                case 0x0: return "Free space";
                case 0x1: return "FAT-12";
                case 0x4: return "FAT-16B / FAT-16 / FAT-12";
                case 0x5: return "Extended partition";
                case 0x7: return "IFS, HPFS, NTFS, exFAT or other";
                case 0xB: return "FAT-32";
                case 0xC: return "FAT-32X (FAT-32 with LBA)";
                case 0xF: return "Extended partition with LBA";
                case 0x27: return "Hidden NTFS (system recovery partition)";
                case 0x41: return "Old Linux / Minix, PPC PReP Boot";
                case 0x42: return "Old Linux swap, SFS, Windows Dynamic Disk";
                case 0x43: return "Old Linux";
                case 0x63: return "UNIX";
                case 0x82: return "Linux swap, Sun Solaris (old)";
                case 0x83: return "Linux";
                case 0x85: return "Linux extended";
                case 0x93: return "Amoeba, hidden Linux";
                case 0x94: return "Amoeba BBT";
                case 0xA5: return "Hibernation partition";
                case 0xB6: return "Mirror master - FAT-16 Windows NT partition";
                case 0xB7: return "Mirror master - NTFS/HPFS Windows NT partition";
                case 0xC2: return "Hidden Linux";
                case 0xC3: return "Hidden Linux swap";
                case 0xC6: return "Mirror slave - FAT-16 Windows NT partition";
                case 0xC7: return "Mirror slave - NTFS Windows NT partition";
                case 0xCD: return "Memory dump";
                case 0xDA: return "Data, not fyle system";
                case 0xDD: return "Hidden memory dump";
                case 0xDE: return "Dell utility";
                case 0xED: return "Hybrid GPT";
                case 0xEE: return "GPT";
                case 0xEF: return "System UEFI partition";
                default: return "unknown";
            }
        }

        public byte EndHead => endHead;

        public short EndSectorCylinder => endSectorCylinder;

        public int FirstSectorOffset => firstSectorOffset;

        public int PartitionSectorAmount => partitionSectorAmount;
    }
}
