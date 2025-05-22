using NetDream.Shared.Interfaces.Entities;
using NetDream.Shared.Models;

namespace NetDream.Modules.Legwork.Models
{
    public class ServiceListItem : IWithUserModel, IWithCategoryModel
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public string Name { get; set; } = string.Empty;
        public int CatId { get; set; }
        public string Thumb { get; set; } = string.Empty;
        public string Brief { get; set; } = string.Empty;
        public float Price { get; set; }

        public int Status { get; set; }
        public IUser? User { get; set; }
        public ListLabelItem? Category { get; set; }
    }
}
