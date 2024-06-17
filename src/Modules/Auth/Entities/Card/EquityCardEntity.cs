using NPoco;

namespace NetDream.Modules.Auth.Entities
{
    [TableName(ND_TABLE_NAME)]
    public class EquityCardEntity
    {
        internal const string ND_TABLE_NAME = "equity_card";
        public int Id { get; set; }

        public string Name { get; set; } = string.Empty;

        public string Icon { get; set; } = string.Empty;

        public string Configure { get; set; } = string.Empty;

        public byte Status { get; set; }

        [Column("updated_at")]
        public int UpdatedAt { get; set; }

        [Column("created_at")]
        public int CreatedAt { get; set; }

    }
}
