using NetDream.Shared.Models;
using System.ComponentModel.DataAnnotations;

namespace NetDream.Modules.ResourceStore.Forms
{
    public class FileQueryForm : QueryForm
    {
        [Required]
        public int ResId { get; set; }
    }
}
