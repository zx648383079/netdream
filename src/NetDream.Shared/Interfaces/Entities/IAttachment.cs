namespace NetDream.Shared.Interfaces
{
    public interface IAttachment
    {
        public int Id { get; }
        public byte ItemType { get; }
        public int ItemId { get; }

        public string Thumb { get; }
        public byte FileType { get; }
        public string File { get; }
        public string AccessPassword { get; }
        public string ArchivePassword { get; }
    }
}
