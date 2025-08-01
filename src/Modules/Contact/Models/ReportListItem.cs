using NetDream.Modules.Contact.Entities;
using NetDream.Shared.Interfaces.Entities;

namespace NetDream.Modules.Contact.Models
{
    public class ReportListItem: ReportEntity, IWithUserModel
    {

        public IUser? User { get; set; }
    }
}
