using NetDream.Shared.Models;
using System.Collections.Generic;

namespace NetDream.Shared.Interfaces
{
    public interface IMetaRepository
    {

        public IDictionary<string, string> Get(ModuleTargetType type, int target, string language);
        public string Get(ModuleTargetType type, int target, string language, string name);
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
        /// 设置一条数据
        /// </summary>
        /// <param name="type"></param>
        /// <param name="target"></param>
        /// <param name="language"></param>
        /// <param name="name"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public IOperationResult Update(ModuleTargetType type, int target, string language, string name, string value);
        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="type"></param>
        /// <param name="target"></param>
        public void Remove(ModuleTargetType type, int target);
        public void Remove(ModuleTargetType type, int target, string language);
        public void Remove(ModuleTargetType type, int target, string language, string name);
    }
}
