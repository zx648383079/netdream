using NetDream.Modules.Navigation.Entities;
using NetDream.Shared.Interfaces.Entities;
using NetDream.Shared.Providers.Entities;

namespace NetDream.Modules.Navigation.Models
{
    public class SiteListItem : IWithCategoryModel, IWithUserModel
    {
        public int Id { get; set; }
        public string Schema { get; set; } = string.Empty;
        public string Domain { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
        public string Logo { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;

        public int CatId { get; set; }
        public int UserId { get; set; }
        public int CreatedAt { get; set; }

        public CategoryEntity? Category { get; set; }
        public IUser? User { get; set; }

        public TagEntity[] Tags { get; set; }
    }
}
