using NetDream.Shared.Interfaces.Entities;

namespace NetDream.Modules.Clan.Entities
{
    public class FamilyLogEntity : IIdEntity, ICreatedEntity
    {
        public int Id { get; set; }
        public int FamilyId { get; set; }
        public string RelationFamily { get; set; }
        public string Event { get; set; }
        public string Remark { get; set; }

        public int EditUser { get; set; }

        public int CreatedAt { get; set; }
    }
}
