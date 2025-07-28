namespace NetDream.Modules.Document.Models
{
    public interface IPageModel
    {
        public int Id { get; }
        public string Name { get; }

        public int ProjectId { get; }

        public int VersionId { get; }

        public int ParentId { get; }
        public byte Type { get; }

        public int UpdatedAt { get; }

        public int CreatedAt { get; }
    }
}
