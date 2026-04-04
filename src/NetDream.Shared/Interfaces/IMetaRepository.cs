using NetDream.Shared.Models;
using System.Collections.Generic;

namespace NetDream.Shared.Interfaces
{
    public interface IMetaRepository
    {

        public IDictionary<string, string> Get(ModuleTargetType type, int target, string language);
        public IDictionary<string, string> Get(ModuleTargetType type, int target, string language, IDictionary<string, string> def);
        /// <summary>
        /// 会删除旧的数据
        /// </summary>
        /// <param name="type"></param>
        /// <param name="target"></param>
        /// <param name="data"></param>
        /// <returns></returns>
        public IOperationResult Replace(ModuleTargetType type, int target, string language, IDictionary<string, string> data);
        /// <summary>
        /// 不会删除其他旧数据
        /// </summary>
        /// <param name="type"></param>
        /// <param name="target"></param>
        /// <param name="data"></param>
        /// <returns></returns>
        public IOperationResult Update(ModuleTargetType type, int target, string language, IDictionary<string, string> data);
        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="type"></param>
        /// <param name="target"></param>
        public void Remove(ModuleTargetType type, int target);
    }
}
