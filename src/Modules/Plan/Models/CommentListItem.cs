using NetDream.Modules.Plan.Entities;
using NetDream.Shared.Interfaces.Entities;

namespace NetDream.Modules.Plan.Models
{
    public class CommentListItem : CommentEntity, IWithUserModel
    {
        public IUser? User { get; set; }
    }
}
