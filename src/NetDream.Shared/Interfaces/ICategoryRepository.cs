using NetDream.Shared.Models;
using System.Collections.Generic;

namespace NetDream.Shared.Interfaces
{
    public interface ICategoryRepository
    {
        public IListLabelItem[] Get(ModuleTargetType type);
        public IOperationResult<IListLabelItem> Get(ModuleTargetType type, int id);

        public IOperationResult Add(ModuleTargetType type, IListLabelItem item);
        public IOperationResult Update(ModuleTargetType type, IListLabelItem item);
        public void Remove(ModuleTargetType type, int id);
        public ITreeItem[] Tree(ModuleTargetType type);

        public ILevelItem[] All(ModuleTargetType type);
        public ILevelItem[] Get(ModuleTargetType type, int[] excludes);

        public void Include(ModuleTargetType type, IEnumerable<IWithCategoryModel> items);
    }

    public interface IWithCategoryModel
    {
        public int CatId { get; }

        public IListLabelItem? Category { set; }

    }
}
