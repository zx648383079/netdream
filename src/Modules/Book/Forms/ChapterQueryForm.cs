using NetDream.Shared.Models;
using System.ComponentModel.DataAnnotations;

namespace NetDream.Modules.Book.Forms
{
    public class ChapterQueryForm : QueryForm
    {
        [Required]
        public int Book { get; set; }
    }
}
