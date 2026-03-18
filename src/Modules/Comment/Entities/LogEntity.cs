using NetDream.Shared.Interfaces.Entities;

namespace NetDream.Modules.Comment.Entities
{
    public class LogEntity : IActionEntity, IIdEntity, ICreatedEntity
    {

        public int Id { get; set; }

        public byte ItemType { get; set; }

        public int ItemId { get; set; }

        public int UserId { get; set; }
        public byte Action { get; set; }
        public int CreatedAt { get; set; }

    }
}
