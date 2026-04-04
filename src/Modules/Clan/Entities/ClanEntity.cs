using NetDream.Shared.Interfaces;

namespace NetDream.Modules.Clan.Entities
{
    public class ClanEntity: IIdEntity, ITimestampEntity
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Cover { get; set; }
        public string Description { get; set; }
        public byte Status { get; set; }
        public int UserId { get; set; }
        public int ModifyAt { get; set; }


        public int UpdatedAt { get; set; }

        public int CreatedAt { get; set; }
    }
}
