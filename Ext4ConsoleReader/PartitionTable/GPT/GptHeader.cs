using System;

namespace Ext4ConsoleReader.PartitionTable.GPT
{
    public class GPTHeader
    {
        private byte[] signature = new byte[8]; // 8 bytes
        private byte[] headerVersion = new byte[4]; // 4 bytes
        private int headerSize;
        private int headerCrc32Hash;
        private int firstReserved;
        private byte[] primaryGptHeaderAddress = new byte[8]; // 8 bytes
        private byte[] secondaryGptHeaderAddress = new byte[8]; // 8 bytes
        private byte[] firstPartitionAddress = new byte[8]; // 8 bytes
        private byte[] lastPartitionSectorOfDisk = new byte[8]; // 8 bytes
        private byte[] diskGuid = new byte[16]; // 16 bytes
        private byte[] partitionTableAddress = new byte[8]; // 8 bytes
        private int maxPartitionAmount;
        private int partitionRecordSize;
        private int partTableCrc32Hash;
        private byte[] secondReserved = new byte[420]; // 420 bytes

        public GPTHeader(byte[] rawHeader)
        {
            Array.Copy(rawHeader, 0, signature, 0, 8);
            Array.Copy(rawHeader, 8, headerVersion, 0, 4);
            headerSize = BitConverter.ToInt32(rawHeader, 12);
            headerCrc32Hash = BitConverter.ToInt32(rawHeader, 16);
            firstReserved = BitConverter.ToInt32(rawHeader, 20);
            Array.Copy(rawHeader, 24, primaryGptHeaderAddress, 0, 8);
            Array.Copy(rawHeader, 32, secondaryGptHeaderAddress, 0, 8);
            Array.Copy(rawHeader, 40, firstPartitionAddress, 0, 8);
            Array.Copy(rawHeader, 48, lastPartitionSectorOfDisk, 0, 8);
            Array.Copy(rawHeader, 56, diskGuid, 0, 16);
            Array.Copy(rawHeader, 72, partitionTableAddress, 0, 8);
            maxPartitionAmount = BitConverter.ToInt32(rawHeader, 80);
            partitionRecordSize = BitConverter.ToInt32(rawHeader, 84);
            partTableCrc32Hash = BitConverter.ToInt32(rawHeader, 88);
            Array.Copy(rawHeader, 92, secondReserved, 0, 420);
        }
    }
}
