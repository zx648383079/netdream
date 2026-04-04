using NetDream.Modules.Shop.Entities;
using NetDream.Shared.Interfaces;

namespace NetDream.Modules.Shop.Market.Models
{
    public class CommentListItem : CommentEntity, IWithUserModel
    {
        public GoodsListItem? Goods { get; set; }
        public CommentImageEntity[] Images { get; set; }
        public IUser? User { get; set; }
    }
}
