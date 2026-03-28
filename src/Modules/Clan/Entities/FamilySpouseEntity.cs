using NetDream.Shared.Interfaces.Entities;

namespace NetDream.Modules.Clan.Entities
{
    public class FamilySpouseEntity : IIdEntity, ITimestampEntity
    {
        public int Id { get; set; }
        public int FamilyId { get; set; }
        public int SpouseId { get; set; }
        public int Relation { get; set; }
        public byte Status { get; set; }
        public string StartAt { get; set; }
        public string EndAt { get; set; }


        public int UpdatedAt { get; set; }

        public int CreatedAt { get; set; }
    }
}
