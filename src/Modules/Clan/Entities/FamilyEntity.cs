using NetDream.Shared.Interfaces.Entities;

namespace NetDream.Modules.Clan.Entities
{
    public class FamilyEntity : IIdEntity, ITimestampEntity
    {
        public int Id { get; set; }

        public int ClanId { get; set; }

        public int UserId { get; set; }
        public int ParentId { get; set; }
        public int MotherId { get; set; }
        public int SpouseId { get; set; }
        public int LevelId { get; set; }


        public string Name { get; set; }
        public string SecondaryName { get; set; }
        public byte Sex { get; set; }

        public string Lifetime { get; set; }

        public string BirthAt { get; set; }
        public string DeathAt { get; set; }


        public byte Status { get; set; }

        public int UpdatedAt { get; set; }

        public int CreatedAt { get; set; }
    }
}
