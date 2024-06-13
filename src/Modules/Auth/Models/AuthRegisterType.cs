using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetDream.Modules.Auth.Models
{
    public enum AuthRegisterType: byte
    {
        All,
        InviteCode,
        Close
    }
}
