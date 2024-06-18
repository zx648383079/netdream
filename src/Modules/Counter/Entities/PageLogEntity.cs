using NPoco;
namespace Modules.Counter.Entities
{
    [TableName(ND_TABLE_NAME)]
    public class PageLogEntity
    {
        internal const string ND_TABLE_NAME = "ctr_page_log";
        public int Id { get; set; }
        public string Url { get; set; } = string.Empty;
        [Column("visit_count")]
        public int VisitCount { get; set; }
    }
}
