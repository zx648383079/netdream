using NetDream.Modules.Exam.Entities;
using NetDream.Shared.Interfaces.Entities;

namespace NetDream.Modules.Exam.Models
{
    public class UpgradeLogListItem : UpgradeUserEntity, IWithUserModel, IWithUpgradeModel
    {
        public IUser? User { get; set; }
        public UpgradeEntity? Upgrade { get; set; }
    }
}
