using NetDream.Shared.Interfaces.Entities;
using System;

namespace NetDream.Shared.Providers.Entities
{
    public class DayLogEntity : IIdEntity, ICreatedEntity
    {
        public int Id { get; set; }
        public string HappenDay { get; set; } = string.Empty;
        public int ItemId { get; set; }
        public byte ItemType { get; set; }
        public byte Action { get; set; }

        public int HappenCount { get; set; }
        public int CreatedAt { get; set; }
    }
}
