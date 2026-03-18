namespace NetDream.Shared.Interfaces.Entities
{
    public interface IAttachment
    {
        public int Id { get; }
        public byte ItemType { get; }
        public int ItemId { get; }

        public string Thumb { get; }
        public byte FileType { get; }
        public string File { get; }
    }
}
