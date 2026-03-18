using NetDream.Shared.Interfaces.Entities;
using System.Collections.Generic;

namespace NetDream.Shared.Interfaces
{
    public interface IAttachRepository
    {
        public IEnumerable<IAttachment> Get(byte itemType, int itemId);
        public IAttachment Get(int id);
        public void Add(byte itemType, int itemId, byte fileType, string file);
        public void Update(int id, byte fileType, string file);
        public void Remove(byte itemType, int itemId);
        public void Remove(int id);
    }
}
