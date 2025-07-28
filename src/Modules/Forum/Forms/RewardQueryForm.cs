using NetDream.Shared.Models;
using System.ComponentModel.DataAnnotations;

namespace NetDream.Modules.Forum.Forms
{
    public class RewardQueryForm : QueryForm
    {
        [Required]
        public int Id { get; set; }

        public int Type { get; set; }
    }
}
