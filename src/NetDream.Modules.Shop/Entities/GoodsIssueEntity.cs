using NPoco;
namespace NetDream.Modules.Shop.Entities
{
    [TableName(ND_TABLE_NAME)]
    public class GoodsIssueEntity
    {
        internal const string ND_TABLE_NAME = "shop_goods_issue";
        public int Id { get; set; }
        [Column("goods_id")]
        public int GoodsId { get; set; }
        public string Question { get; set; } = string.Empty;
        public string Answer { get; set; } = string.Empty;
        [Column("ask_id")]
        public int AskId { get; set; }
        [Column("answer_id")]
        public int AnswerId { get; set; }
        public int Status { get; set; }
        [Column("updated_at")]
        public int UpdatedAt { get; set; }
        [Column("created_at")]
        public int CreatedAt { get; set; }
    }
}
