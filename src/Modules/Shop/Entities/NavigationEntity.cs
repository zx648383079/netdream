
namespace NetDream.Modules.Shop.Entities
{
    
    public class NavigationEntity
    {
        
        public int Id { get; set; }
        public string Type { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
        public string Url { get; set; } = string.Empty;
        public string Target { get; set; } = string.Empty;
        public int Visible { get; set; }
        public int Position { get; set; }
    }
}
