using NetDream.Shared.Interfaces.Entities;
using NPoco;

namespace NetDream.Modules.Document.Entities
{
    [TableName(ND_TABLE_NAME)]
    public class CategoryEntity : IIdEntity
    {
        internal const string ND_TABLE_NAME = "doc_category";
        public int Id { get; set; }

        public string Name { get; set; } = string.Empty;

        public string Icon { get; set; } = string.Empty;

        [Column("parent_id")]
        public int ParentId { get; set; }

    }
}
