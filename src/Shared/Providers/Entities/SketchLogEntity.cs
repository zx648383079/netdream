using NetDream.Shared.Interfaces.Entities;

namespace NetDream.Shared.Providers.Entities
{
    public class SketchLogEntity : IIdEntity, ITimestampEntity
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public int ItemId { get; set; }
        public byte ItemType { get; set; }
        public string Data { get; set; } = string.Empty;

        public string Ip { get; set; } = string.Empty;
        public int UpdatedAt { get; set; }
        public int CreatedAt { get; set; }
    }
}
