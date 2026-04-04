using NetDream.Modules.Contact.Entities;
using NetDream.Shared.Interfaces;

namespace NetDream.Modules.Contact.Models
{
    public class ReportListItem: ReportEntity, IWithUserModel
    {

        public IUser? User { get; set; }
    }
}
