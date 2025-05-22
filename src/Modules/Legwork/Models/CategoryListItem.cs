namespace NetDream.Modules.Legwork.Models
{
    public class CategoryListItem
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Icon { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public byte Status { get; set; }
    }
}
