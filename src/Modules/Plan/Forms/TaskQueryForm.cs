using NetDream.Shared.Models;

namespace NetDream.Modules.Plan.Forms
{
    public class TaskQueryForm : QueryForm
    {
        public int Status { get; set; }

        public int ParentId { get; set; }
    }
}
