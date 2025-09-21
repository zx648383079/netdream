using NetDream.Shared.Interfaces.Entities;

namespace NetDream.Modules.UserIdentity.Entities
{
    public class ZoneEntity : IIdEntity, ITimestampEntity
    {
        public int Id { get; set; }

        public string Name { get; set; } = string.Empty;

        public string Icon { get; set; } = string.Empty;

        public string Description { get; set; } = string.Empty;
        public byte IsOpen { get; set; }

        public byte Status { get; set; }


        public int UpdatedAt { get; set; }


        public int CreatedAt { get; set; }
    }
}
