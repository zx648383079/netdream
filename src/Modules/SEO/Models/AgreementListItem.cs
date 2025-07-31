
namespace NetDream.Modules.SEO.Models
{
    public class AgreementListItem
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Title { get; set; } = string.Empty;
        public string Language { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public int Status { get; set; }

        public int UpdatedAt { get; set; }

        public int CreatedAt { get; set; }
    }
}
