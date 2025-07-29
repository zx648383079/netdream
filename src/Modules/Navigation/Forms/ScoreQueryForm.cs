using NetDream.Shared.Models;
using System.ComponentModel.DataAnnotations;

namespace NetDream.Modules.Navigation.Forms
{
    public class ScoreQueryForm : QueryForm
    {
        [Required]
        public int Site { get; set; }
    }
}
