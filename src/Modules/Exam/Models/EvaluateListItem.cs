using NetDream.Modules.Exam.Entities;
using NetDream.Shared.Interfaces.Entities;

namespace NetDream.Modules.Exam.Models
{
    public class EvaluateListItem : PageEvaluateEntity, IWithUserModel
    {
        public IUser? User { get; set; }
    }
}
