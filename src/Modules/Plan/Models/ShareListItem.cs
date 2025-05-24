using NetDream.Shared.Interfaces.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetDream.Modules.Plan.Models
{
    public class ShareListItem : IWithUserModel, IWithTaskModel
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public int TaskId { get; set; }
        public byte ShareType { get; set; }
        public int UpdatedAt { get; set; }
        public int CreatedAt { get; set; }
        public IUser? User { get; set; }
        public TaskLabelItem? Task { get; set; }
    }
}
