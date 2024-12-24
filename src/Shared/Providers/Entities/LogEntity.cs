using NetDream.Shared.Interfaces.Entities;

namespace NetDream.Shared.Providers.Entities
{
    public class LogEntity : IActionEntity, IIdEntity, ICreatedEntity
    {

        public int Id { get; set; }

        public byte ItemType { get; set; }

        public int ItemId { get; set; }

        public int UserId { get; set; }
        public byte Action { get; set; }
        public string Data { get; set; } = string.Empty;
        public string Ip { get; set; } = string.Empty;
        public int CreatedAt { get; set; }

    }
}
