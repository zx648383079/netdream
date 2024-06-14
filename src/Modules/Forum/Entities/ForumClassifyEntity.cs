using NPoco;
namespace Modules.Forum.Entities
{
    [TableName(ND_TABLE_NAME)]
    public class ForumClassifyEntity
    {
        internal const string ND_TABLE_NAME = "bbs_forum_classify";
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Icon { get; set; } = string.Empty;
        [Column("forum_id")]
        public int ForumId { get; set; }
        public byte Position { get; set; }
    }
}
