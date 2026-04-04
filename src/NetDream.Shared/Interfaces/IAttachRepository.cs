using NetDream.Shared.Models;
using System.Collections.Generic;

namespace NetDream.Shared.Interfaces
{
    public interface IAttachRepository
    {
        public IEnumerable<IAttachment> Get(ModuleTargetType type, int target);
        public IAttachment Get(int id);
        public void Add(ModuleTargetType type, int target, byte fileType, string file);
        public void Update(int id, byte fileType, string file);
        public void Remove(ModuleTargetType type, int target);
        public void Remove(int id);
    }
}
