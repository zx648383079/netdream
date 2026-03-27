using NetDream.Shared.Repositories;
using System.Collections.Generic;

namespace NetDream.Shared.Interfaces
{
    public interface ITagRepository
    {

        public string[] Get(ModuleTargetType type, int target);
        public int[] Get(ModuleTargetType type, string tag);
        public IOperationResult Bind(ModuleTargetType type, int target, IEnumerable<string> tags);
        public IOperationResult Remove(ModuleTargetType type, int target, IEnumerable<string> tags);
    }
}
