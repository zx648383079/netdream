
namespace NetDream.Modules.Counter.Entities
{
    
    public class PageLogEntity
    {
        
        public int Id { get; set; }
        public string Url { get; set; } = string.Empty;
        
        public int VisitCount { get; set; }
    }
}
