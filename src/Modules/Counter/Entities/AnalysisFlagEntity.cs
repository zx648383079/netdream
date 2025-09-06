using NetDream.Shared.Interfaces.Entities;

namespace NetDream.Modules.Counter.Entities
{
    public class AnalysisFlagEntity : IIdEntity, ICreatedEntity
    {
        public int Id { get; set; }

        public byte ItemType { get; set; }

        public string ItemValue { get; set; } = string.Empty;
        public int Flags { get; set; }

        public int UserId { get; set; }

        public int CreatedAt { get; set; }
    }
}
