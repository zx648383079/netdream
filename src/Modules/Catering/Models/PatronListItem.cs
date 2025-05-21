using NetDream.Shared.Interfaces.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetDream.Modules.Catering.Models
{
    public class PatronListItem : IWithUserModel
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public int GroupId { get; set; }
        public int Amount { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Remark { get; set; } = string.Empty;
        public int CreatedAt { get; set; }
        public IUser? User { get; set; }
    }
}
