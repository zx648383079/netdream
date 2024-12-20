using NetDream.Modules.Auth.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetDream.Modules.Auth.Models
{
    public class UserRole
    {
        public RoleEntity? Role { get; set; }

        public string[] Roles { get; set; } = [];
        public string[] Permissions { get; set; } = [];
    }
}
