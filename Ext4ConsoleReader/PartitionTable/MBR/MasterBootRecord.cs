using DStream;
using Ext4ConsoleReader.HardDisk;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Ext4ConsoleReader.PartitionTable.MBR
{
    public class MasterBootRecord
    {
        public byte[] Loader { get; } // 446 bytes
        public List<MbrPartition> Partitions { get; } // 4 partitions
        public byte[] EndSignature { get; }

        public static MasterBootRecord ReadFrom(byte[] rawMbr)
        {
            return new MasterBootRecord(rawMbr);
        }

        public MasterBootRecord(byte[] rawMbr)
        {
            Loader = new byte[446];
            Partitions = new List<MbrPartition>();
            EndSignature = new byte[2];

            Array.Copy(rawMbr, 0, Loader, 0, 446);

            for (int i = 0; i < 4; i++)
            {
                byte[] partitionData = new byte[16];
                Array.Copy(rawMbr, 446 + 16 * i, partitionData, 0, 16);

                MbrPartition partition = new MbrPartition(partitionData);

                if (partition.RawType != 0) // filter empty records
                {
                    Partitions.Add(partition);
                }
            }

            Array.Copy(rawMbr, 510, EndSignature, 0, 2);
        }

        public bool IsCorrect()
        {
            return EndSignature[0] == 0x55 && EndSignature[1] == 0xAA;
        }

        public bool IsGPT()
        {
            return Partitions.Any() && Partitions[0].RawType == 0xEE;
        }
    }
}
