using NetDream.Shared.Models;
using System.ComponentModel.DataAnnotations;

namespace NetDream.Modules.Plan.Forms
{
    public class CommentQueryForm : QueryForm
    {
        [Required]
        public int Task { get; set; }
    }
}
