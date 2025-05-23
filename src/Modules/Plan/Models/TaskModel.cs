using NetDream.Modules.Plan.Entities;
using NetDream.Shared.Interfaces.Entities;

namespace NetDream.Modules.Plan.Models
{
    public class TaskModel : TaskEntity, IWithUserModel
    {
        public IUser? User { get; set; }

        public TaskEntity[] Children { get; set; }
    }
}
