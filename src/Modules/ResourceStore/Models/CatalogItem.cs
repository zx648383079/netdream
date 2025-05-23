namespace NetDream.Modules.ResourceStore.Models
{
    public class CatalogItem
    {
        public string Name { get; set; }
        public byte Type { get; set; }
        public string Icon { get; set; }

        public CatalogItem[]? Children { get; set; }
    }
}
