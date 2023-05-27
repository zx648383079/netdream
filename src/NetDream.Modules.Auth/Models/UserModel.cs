using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetDream.Modules.Auth.Models
{
    public class UserModel
    {

        public int Id { get; set; }

        public string Token { get; set; } = string.Empty;
    }
}
