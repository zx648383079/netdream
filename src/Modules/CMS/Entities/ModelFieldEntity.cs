using NPoco;
namespace NetDream.Modules.CMS.Entities
{
    [TableName(ND_TABLE_NAME)]
    public class ModelFieldEntity
    {
        internal const string ND_TABLE_NAME = "cms_model_field";
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Field { get; set; } = string.Empty;
        [Column("model_id")]
        public int ModelId { get; set; }
        public string Type { get; set; } = string.Empty;
        public int Length { get; set; }
        public byte Position { get; set; }
        [Column("form_type")]
        public byte FormType { get; set; }
        [Column("is_main")]
        public byte IsMain { get; set; }
        [Column("is_required")]
        public byte IsRequired { get; set; }
        [Column("is_search")]
        public byte IsSearch { get; set; }
        [Column("is_disable")]
        public byte IsDisable { get; set; }
        [Column("is_system")]
        public byte IsSystem { get; set; }
        public string Match { get; set; } = string.Empty;
        [Column("tip_message")]
        public string TipMessage { get; set; } = string.Empty;
        [Column("error_message")]
        public string ErrorMessage { get; set; } = string.Empty;
        [Column("tab_name")]
        public string TabName { get; set; } = string.Empty;
        public string Setting { get; set; } = string.Empty;
    }
}
