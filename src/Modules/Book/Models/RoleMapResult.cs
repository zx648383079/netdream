using NetDream.Modules.Book.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetDream.Modules.Book.Models
{
    public class RoleMapResult
    {
        public RoleEntity[] Items { get; internal set; }
        public RoleRelationEntity[] LinkItems { get; internal set; }
    }
}
