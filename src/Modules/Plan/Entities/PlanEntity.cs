using NetDream.Shared.Interfaces.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetDream.Modules.Plan.Entities
{
    public class PlanEntity : IIdEntity, ITimestampEntity
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public int TaskId { get; set; }
        public byte PlanType { get; set; }
        public int PlanTime { get; set; }
        public byte Amount { get; set; }
        public byte Priority { get; set; }
        public int UpdatedAt { get; set; }
        public int CreatedAt { get; set; }
    }
}
