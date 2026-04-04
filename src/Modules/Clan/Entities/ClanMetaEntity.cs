using NetDream.Shared.Interfaces;

namespace NetDream.Modules.Clan.Entities
{
    public class ClanMetaEntity : IIdEntity, ITimestampEntity
    {
        public int Id { get; set; }
        public int ClanId { get; set; }


        public string Name { get; set; }
        public string Content { get; set; }
   
        public int UserId { get; set; }

        public string Author { get; set; }
        public byte Position { get; set; }
        public int ModifyAt { get; set; }

        public int UpdatedAt { get; set; }

        public int CreatedAt { get; set; }
    }
}
