using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetDream.Modules.Auth.Forms
{
    public class RoleForm
    {
        public int Id { get; set; }
        [Required]
        public string Name { get; set; } = string.Empty;
        [Required]
        public string DisplayName { get; set; } = string.Empty;

        public string Description { get; set; } = string.Empty;
    }

    public class PermissionForm
    {
        public int Id { get; set; }
        [Required]
        public string Name { get; set; } = string.Empty;
        [Required]
        public string DisplayName { get; set; } = string.Empty;

        public string Description { get; set; } = string.Empty;
    }
}
