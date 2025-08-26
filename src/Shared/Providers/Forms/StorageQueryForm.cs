using NetDream.Shared.Models;

namespace NetDream.Shared.Providers.Forms
{
    public class StorageQueryForm  : QueryForm
    {

        public int Folder { get; set; }

        public string[] Extension { get; set; }

        public StorageQueryForm()
        {
            
        }

        public StorageQueryForm(QueryForm form)
            : base(form)
        {
            
        }
    }
}
