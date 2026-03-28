using NetDream.Shared.Models;
using System.Collections.Generic;

namespace NetDream.Shared.Interfaces
{
    public interface ITagRepository
    {
        /// <summary>
        /// 根据文章获取所有标签
        /// </summary>
        /// <param name="type"></param>
        /// <param name="target"></param>
        /// <returns></returns>
        public string[] Get(ModuleTargetType type, int target);
        /// <summary>
        /// 根据标签获取所有文章
        /// </summary>
        /// <param name="type"></param>
        /// <param name="tag"></param>
        /// <returns></returns>
        public int[] Get(ModuleTargetType type, string tag);
        /// <summary>
        /// 搜索标签获取所有文章
        /// </summary>
        /// <param name="type"></param>
        /// <param name="keywords"></param>
        /// <returns></returns>
        public int[] Search(ModuleTargetType type, string keywords);
        /// <summary>
        /// 文章绑定标签
        /// </summary>
        /// <param name="type"></param>
        /// <param name="target"></param>
        /// <param name="tags"></param>
        /// <returns></returns>
        public IOperationResult Bind(ModuleTargetType type, int target, IEnumerable<string> tags);
        /// <summary>
        /// 文章删除标签
        /// </summary>
        /// <param name="type"></param>
        /// <param name="target"></param>
        /// <param name="tags"></param>
        /// <returns></returns>
        public IOperationResult Remove(ModuleTargetType type, int target, IEnumerable<string> tags);
        /// <summary>
        /// 获取文章所有标签下的其他文章
        /// </summary>
        /// <param name="type"></param>
        /// <param name="target"></param>
        /// <returns></returns>
        public int[] GetRelation(ModuleTargetType type, int target);
        /// <summary>
        /// 获取标签的统计
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public StatisticsItem[] Get(ModuleTargetType type);
    }
}
