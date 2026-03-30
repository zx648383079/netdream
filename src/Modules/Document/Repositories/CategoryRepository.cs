using NetDream.Shared.Interfaces;
using NetDream.Shared.Interfaces.Entities;
using NetDream.Shared.Models;
using NetDream.Shared.Providers;
using System.Linq;

namespace NetDream.Modules.Document.Repositories
{
    public class CategoryRepository(ICategoryRepository store)
    {

        public void Remove(int id)
        {
            store.Remove(ModuleTargetType.Document, id);
        }

        public ILevelItem[] LevelTree(int[] excludes)
        {
            return store.All(ModuleTargetType.Document).Where(i => !excludes.Contains(i.Id)).ToArray();
        }

        public ITreeItem[] Tree()
        {
            return store.Tree(ModuleTargetType.Document);
        }
    }
}
