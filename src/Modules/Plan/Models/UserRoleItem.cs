using NetDream.Shared.Interfaces.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetDream.Modules.Plan.Models
{
    public class UserRoleItem : IUser
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string Avatar { get; set; }

        public bool IsOnline { get; set; }

        public string RoleName { get; set; }

        public bool Editable { get; set; }
    }
}
