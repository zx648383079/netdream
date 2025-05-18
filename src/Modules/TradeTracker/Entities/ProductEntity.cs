using NetDream.Shared.Interfaces.Entities;

namespace NetDream.Modules.TradeTracker.Entities
{
    public class ProductEntity : IIdEntity, ITimestampEntity
    {
        public int Id { get; set; }
        public int ParentId { get; set; }
        public string Name { get; set; } = string.Empty;
        public string EnName { get; set; } = string.Empty;
        public int CatId { get; set; }
        public int ProjectId { get; set; }
        public string UniqueCode { get; set; } = string.Empty;
        public byte IsSku { get; set; }
        public int UpdatedAt { get; set; }
        public int CreatedAt { get; set; }
    }
}
