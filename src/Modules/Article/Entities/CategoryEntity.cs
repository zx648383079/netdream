using NetDream.Shared.Interfaces;

namespace NetDream.Modules.Article.Entities
{
    public class CategoryEntity: IIdEntity, ITimestampEntity
    {

        public int Id { get; set; }
        public int ParentId { get; set; }

        public byte Type { get; set; }
        public string Name { get; set; } = string.Empty;
        public string EnName { get; set; } = string.Empty;
        public string Thumb { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string ExtraData { get; set; } = string.Empty;

        public byte Status { get; set; }

        public int UpdatedAt { get; set; }

        public int CreatedAt { get; set; }
    }
}
