using NetDream.Shared.Models;

namespace NetDream.Modules.Contact.Forms
{
    public class ReportQueryForm : QueryForm
    {

        public byte ItemType { get; set; }

        public int ItemId { get; set; }

        public byte Type { get; set; }
    }
}
