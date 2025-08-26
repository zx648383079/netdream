using NetDream.Shared.Models;
using System.ComponentModel.DataAnnotations;

namespace NetDream.Modules.Book.Forms
{
    public class RoleQueryForm : QueryForm
    {
        [Required]
        public int Book { get; set; }
    }
}
