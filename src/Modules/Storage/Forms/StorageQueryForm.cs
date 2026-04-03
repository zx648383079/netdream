using NetDream.Shared.Models;

namespace NetDream.Modules.Storage.Forms
{
    public class StorageQueryForm : QueryForm
    {
        public string Type { get; set; }

        public string[] Extension { get; set; }

        public StorageQueryForm()
        {
            
        }

        public StorageQueryForm(ExplorerQueryForm form): base(form)
        {
            Type = form.Type;
        }
    }
}
