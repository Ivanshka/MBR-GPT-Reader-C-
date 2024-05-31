using System;
using System.Collections.Generic;

using Ext4ConsoleReader.PartitionTable.MBR;

namespace Ext4ConsoleReader.PartitionTable.GPT
{
    public class GuidPartitionTable
    {
        public MasterBootRecord ProtectedMbr { get; }
        public GPTHeader Header { get; }
        public List<GptPartition> Partitions { get; } = new List<GptPartition>(); // 128 partitions

        public GuidPartitionTable(byte[] rawGpt)
        {
            byte[] buffer = new byte[512];
            Array.Copy(rawGpt, 0, buffer, 0, 512);
            ProtectedMbr = MasterBootRecord.ReadFrom(buffer);
            Array.Copy(rawGpt, 512, buffer, 0, 512);
            Header = new GPTHeader(buffer);

            buffer = new byte[128];
            for (int i = 0; i < 128; i++)
            {
                Array.Copy(rawGpt, 1024 + i * 128, buffer, 0, 128);
                GptPartition partition = new GptPartition(buffer);
                byte[] rawTypeGuid = partition.RawTypeGuid();

                if (rawTypeGuid[0] == 0 && rawTypeGuid[1] == 0) // skip empty records
                    continue;

                Partitions.Add(partition);
            }
        }
    }
}
