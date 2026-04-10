using NetDream.Shared.Models;
using System.Collections.Generic;

namespace NetDream.Shared.Interfaces
{
    public interface ICategoryRepository
    {
        /// <summary>
        /// 获取全部
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public IListLabelItem[] Get(ModuleTargetType type);
        /// <summary>
        /// 获取分类信息
        /// </summary>
        /// <param name="type"></param>
        /// <param name="id"></param>
        /// <returns></returns>
        public IOperationResult<IListLabelItem> Get(ModuleTargetType type, int id);
        public IOperationResult<IListLabelItem> Get(ModuleTargetType type, int id, bool children);
        /// <summary>
        /// 获取子代分类
        /// </summary>
        /// <param name="type"></param>
        /// <param name="parent"></param>
        /// <returns></returns>
        public IListLabelItem[] Children(ModuleTargetType type, int parent);

        public IOperationResult Add(ModuleTargetType type, IListLabelItem item);
        public IOperationResult Update(ModuleTargetType type, IListLabelItem item);
        public void Remove(ModuleTargetType type, int id);
        /// <summary>
        /// 获取全部生成树结构
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public ITreeItem[] Tree(ModuleTargetType type);
        /// <summary>
        /// 获取全部并生成 level 
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public ILevelItem[] All(ModuleTargetType type);
        public ILevelItem[] Get(ModuleTargetType type, int[] excludes);

        public void Include(ModuleTargetType type, IEnumerable<IWithCategoryModel> items);
        /// <summary>
        /// 获取子孙后代ID
        /// </summary>
        /// <param name="type"></param>
        /// <param name="parent"></param>
        /// <returns></returns>
        public int[] Include(ModuleTargetType type, int parent);
    }

    public interface IWithCategoryModel
    {
        public int CatId { get; }

        public IListLabelItem? Category { set; }

    }
}
