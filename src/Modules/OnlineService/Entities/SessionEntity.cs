using NetDream.Shared.Interfaces.Entities;
using NPoco;
namespace Modules.OnlineService.Entities
{
    [TableName(ND_TABLE_NAME)]
    public class SessionEntity: IIdEntity, ITimestampEntity
    {
        internal const string ND_TABLE_NAME = "service_session";
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Remark { get; set; } = string.Empty;
        [Column("user_id")]
        public int UserId { get; set; }
        [Column("service_id")]
        public int ServiceId { get; set; }
        public string Ip { get; set; } = string.Empty;
        [Column("user_agent")]
        public string UserAgent { get; set; } = string.Empty;
        public byte Status { get; set; }
        [Column("service_word")]
        public int ServiceWord { get; set; }
        [Column("updated_at")]
        public int UpdatedAt { get; set; }
        [Column("created_at")]
        public int CreatedAt { get; set; }
    }
}
