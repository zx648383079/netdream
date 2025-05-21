using NetDream.Shared.Interfaces.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetDream.Modules.Catering.Models
{
    public class StaffListItem : IWithUserModel
    {
        public int Id { get; set; }
        public int StoreId { get; set; }
        public int UserId { get; set; }
        public int RoleId { get; set; }
        public int UpdatedAt { get; set; }
        public int CreatedAt { get; set; }
        public IUser? User {  get; set; }
    }
}
