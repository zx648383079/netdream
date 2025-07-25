using NetDream.Modules.Forum.Entities;
using NetDream.Shared.Interfaces.Entities;

namespace NetDream.Modules.Forum.Models
{
    public class PostListItem : ThreadPostEntity, IWithUserModel
    {
        public IUser? User {  get; set; }

        public bool IsPublicPost { get; set; }

        public bool Deleteable { get; set; }
    }
}
