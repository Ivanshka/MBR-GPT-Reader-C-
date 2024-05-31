namespace Ext4ConsoleReader.FileSystem.Ext4
{
    // https://wiki.osdev.org/Ext4
    public class Ext4
    {
        private SuperBlock superBlock;

        public bool isExtFileSystem() => superBlock.MagicSignature == 0x53EF;

        public bool IsFileSystemClean() => superBlock.RawFileSystemState == 1;

        public bool IsFileSystemHasErrors() => superBlock.RawFileSystemState == 2;

        public OperatingSystem GetCreatorOS() => (OperatingSystem)superBlock.CreatorOsId;
    }
}
