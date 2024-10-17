using NetDream.Modules.Chat.Entities;
using NPoco;

namespace NetDream.Modules.Chat.Models
{
    public class GroupModel: GroupEntity
    {
        public Page<GroupUserModel>? Users { get; set; }
    }
}
