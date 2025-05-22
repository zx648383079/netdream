namespace NetDream.Modules.Legwork.Models
{
    public class ProviderModel
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Logo { get; set; } = string.Empty;
        public string Tel { get; set; } = string.Empty;
        public string Address { get; set; } = string.Empty;
        public byte OverallRating { get; set; }
        public int CreatedAt { get; set; }
    }
}