using NetDream.Shared.Interfaces;
using NetDream.Shared.Interfaces;

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

        public IListLabelItem? Category { get; set; }
        public IUser? User { get; set; }

        public string[] Tags { get; set; }
    }
}
