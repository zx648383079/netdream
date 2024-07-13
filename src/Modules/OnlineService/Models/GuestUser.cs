using NetDream.Shared.Interfaces.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetDream.Modules.OnlineService.Models
{
    public class GuestUser(): IUser
    {
        public int Id { get; }
        public string Name { get; } = "Guest";

        public string Avatar { get; } = "assets/images/avatar/0.png";
    }
}
