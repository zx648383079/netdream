using NetDream.Modules.Chat.Entities;
using NetDream.Shared.Interfaces;

namespace NetDream.Modules.Chat.Models
{
    public class GroupModel: GroupEntity
    {
        public IPage<GroupUserModel>? Users { get; set; }
    }
}
