using System;

namespace Ext4ConsoleReader.FileSystem.Ext4
{
    // https://wiki.osdev.org/Ext4
    public class SuperBlock
    {
        public int TotalInodeNumber { get; }
        public int TotalBlockNumber { get; }
        public int ReserevedBlockNumber { get; }
        public int UnallocatedBlockNumber { get; }
        public int UnallocatedInodeNumber { get; }
        public int SuperblockOwnerBlock { get; } // Block number of the block containing the superblock.
        public int BlockSizeShift { get; } // log2 (block size) - 10.
        public int FragmentSizeShift { get; } // log2 (fragment size) - 10.
        public int BlockNumberInBlockGroup { get; }
        public int FragmentNumberInBlockGroup { get; }
        public int InodeNumberInBlockGroup { get; }
        public int LastMountTime { get; }
        public int LastWriteTime { get; }
        public int MountedTimesSinceChecked { get; } // Number of times the volume has been mounted since its last consistency check (fsck)
        public int AllowedMountesBeforeCheck { get; } // Number of times the volume has been mounted since its last consistency check (fsck)
        public short MagicSignature { get; } // Magic signature (0xef53), used to help confirm the presence of Ext4 on a volume
        public short RawFileSystemState { get; }
        public short ErrorHandler { get; }
        public short MinorVersionPart { get; }
        public int LastCheckTime { get; }
        public int CheckInterval { get; }
        public int CreatorOsId { get; }
        public short MajorVersionPart { get; }
        public short UserOfReserevedBlocksId { get; }
        public short UserGroupOfReserevedBlocksId { get; }

        public SuperBlock(byte[] rawSuperBlock)
        {
            if (rawSuperBlock.Length != 1024)
            {
                throw new Exception("Super block length is not equal 1024 bytes");
            }

            TotalInodeNumber = BitConverter.ToInt32(rawSuperBlock, 0);
            TotalBlockNumber = BitConverter.ToInt32(rawSuperBlock, 4);
            ReserevedBlockNumber = BitConverter.ToInt32(rawSuperBlock, 8);
            UnallocatedBlockNumber = BitConverter.ToInt32(rawSuperBlock, 12);
            UnallocatedInodeNumber = BitConverter.ToInt32(rawSuperBlock, 16);
            SuperblockOwnerBlock = BitConverter.ToInt32(rawSuperBlock, 20);
            BlockSizeShift = BitConverter.ToInt32(rawSuperBlock, 24);
            FragmentSizeShift = BitConverter.ToInt32(rawSuperBlock, 28);
            BlockNumberInBlockGroup = BitConverter.ToInt32(rawSuperBlock, 32);
            FragmentNumberInBlockGroup = BitConverter.ToInt32(rawSuperBlock, 36);
            InodeNumberInBlockGroup = BitConverter.ToInt32(rawSuperBlock, 40);
            LastMountTime = BitConverter.ToInt32(rawSuperBlock, 44);
            LastWriteTime = BitConverter.ToInt32(rawSuperBlock, 48);
            MountedTimesSinceChecked = BitConverter.ToInt32(rawSuperBlock, 52);
            AllowedMountesBeforeCheck = BitConverter.ToInt32(rawSuperBlock, 54);
            MagicSignature = BitConverter.ToInt16(rawSuperBlock, 56);
            RawFileSystemState = BitConverter.ToInt16(rawSuperBlock, 58);
            ErrorHandler = BitConverter.ToInt16(rawSuperBlock, 60);
            MinorVersionPart = BitConverter.ToInt16(rawSuperBlock, 62);
            LastCheckTime = BitConverter.ToInt32(rawSuperBlock, 64);
            CheckInterval = BitConverter.ToInt32(rawSuperBlock, 68);
            CreatorOsId = BitConverter.ToInt32(rawSuperBlock, 72);
            MajorVersionPart = BitConverter.ToInt16(rawSuperBlock, 76);
            UserOfReserevedBlocksId = BitConverter.ToInt16(rawSuperBlock, 80);
            UserGroupOfReserevedBlocksId = BitConverter.ToInt16(rawSuperBlock, 82);
        }
    }
}
