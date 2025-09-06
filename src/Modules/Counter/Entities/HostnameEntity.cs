using NetDream.Shared.Interfaces.Entities;

namespace NetDream.Modules.Counter.Entities
{
    public class HostnameEntity : IIdEntity
    {
        public int Id { get; set; }

        public string Name { get; set; }
    }
}
