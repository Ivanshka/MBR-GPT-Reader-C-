namespace Ext4ConsoleReader.HardDisk
{
    public class PhysicalDisk
    {
        public uint BytesPerSector { get; }
        public string Caption { get; }
        public string Model { get; }
        public string DeviceId { get; }
        public string Manufacturer { get; }
        public ulong Size { get; }
        public uint PartitionAmount { get; }
        public uint Index { get; }

        public PhysicalDisk(uint bytesPerSector, string caption, string model, string deviceId, string manufacturer, ulong size, uint pertitionAmount, uint index)
        {
            BytesPerSector = bytesPerSector;
            Caption = caption;
            Model = model;
            DeviceId = deviceId;
            Manufacturer = manufacturer;
            Size = size;
            PartitionAmount = pertitionAmount;
            Index = index;
        }
    }
}
