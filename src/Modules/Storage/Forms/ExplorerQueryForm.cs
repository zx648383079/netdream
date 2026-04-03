using NetDream.Shared.Models;

namespace NetDream.Modules.Storage.Forms
{
    public class ExplorerQueryForm : QueryForm
    {
        public string Path { get; set; }

        public string Filter { get; set; }

        public string Type { get; set; }


        public ExplorerQueryForm()
        {
            
        }

        public ExplorerQueryForm(QueryForm form)
            : base(form)
        {
        }
    }
}
