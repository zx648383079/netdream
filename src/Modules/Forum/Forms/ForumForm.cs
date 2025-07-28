using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetDream.Modules.Forum.Forms
{
    public class ForumForm
    {
        public int Id { get; set; }
        [Required]
        public string Name { get; set; } = string.Empty;
        public string Thumb { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;

        public int ParentId { get; set; }
        public byte Type { get; set; }
        public byte Position { get; set; }

        public ClassifyForm[] Classifies { get; set; }
        public ModeratorForm[] Moderators { get; set; }
    }

    public class ModeratorForm
    {
        public int UserId { get; set; }

        public int RoleId { get; set; }
    }

    public class ClassifyForm
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Icon { get; set; } = string.Empty;

        public byte Position { get; set; }
        public int ForumId { get; set; }
    }
}
