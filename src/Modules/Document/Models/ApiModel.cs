using NetDream.Modules.Document.Entities;

namespace NetDream.Modules.Document.Models
{
    public class ApiModel : ApiEntity, IPageModel
    {
        public FieldEntity[] Header { get; set; }
        public FieldEntity[] Request { get; set; }
        public FieldTreeItem[] Response { get; set; }
    }
}
