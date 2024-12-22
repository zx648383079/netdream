using NetDream.Shared.Interfaces.Entities;

namespace NetDream.Shared.Providers.Entities
{
    public class CollectLogEntity : IIdEntity, ICreatedEntity
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public int ItemId { get; set; }
        public byte ItemType { get; set; }
        public string ExtraData { get; set; } = string.Empty;
        public int CreatedAt { get; set; }
    }
}
