using NetDream.Shared.Interfaces.Entities;

namespace NetDream.Shared.Providers.Entities
{
    public class ScoreLogEntity : IIdEntity, ICreatedEntity
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public int ItemId { get; set; }
        public byte ItemType { get; set; }
        public byte Score { get; set; }

        public int FromId { get; set; }
        public byte FromType { get; set; }

        public int CreatedAt { get; set; }
    }
}
