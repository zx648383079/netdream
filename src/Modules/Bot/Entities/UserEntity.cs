using NPoco;
namespace Modules.Bot.Entities
{
    [TableName(ND_TABLE_NAME)]
    public class UserEntity
    {
        internal const string ND_TABLE_NAME = "bot_user";
        public int Id { get; set; }
        public string Openid { get; set; } = string.Empty;
        public string Nickname { get; set; } = string.Empty;
        public byte Sex { get; set; }
        public string City { get; set; } = string.Empty;
        public string Country { get; set; } = string.Empty;
        public string Province { get; set; } = string.Empty;
        public string Language { get; set; } = string.Empty;
        public string Avatar { get; set; } = string.Empty;
        [Column("subscribe_at")]
        public int SubscribeAt { get; set; }
        [Column("union_id")]
        public string UnionId { get; set; } = string.Empty;
        public string Remark { get; set; } = string.Empty;
        [Column("group_id")]
        public int GroupId { get; set; }
        [Column("updated_at")]
        public int UpdatedAt { get; set; }
        [Column("bot_id")]
        public int BotId { get; set; }
        [Column("note_name")]
        public string NoteName { get; set; } = string.Empty;
        public byte Status { get; set; }
        [Column("is_black")]
        public byte IsBlack { get; set; }
        [Column("created_at")]
        public int CreatedAt { get; set; }
    }
}
