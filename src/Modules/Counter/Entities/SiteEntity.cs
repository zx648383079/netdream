using NetDream.Shared.Interfaces.Entities;

namespace NetDream.Modules.Counter.Entities
{
    public class SiteEntity : IIdEntity, ITimestampEntity
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public int UserId { get; set; }

        public int CreatedAt { get; set; }

        public int UpdatedAt { get; set; }
    }
}
