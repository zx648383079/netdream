using NetDream.Shared.Models;
using System.Collections.Generic;

namespace NetDream.Shared.Interfaces
{
    public interface IInteractRepository
    {
        /// <summary>
        /// 获取操作的总记录
        /// </summary>
        /// <param name="itemType"></param>
        /// <param name="target"></param>
        /// <param name="action"></param>
        /// <returns></returns>
        public int Count(ModuleTargetType type, int target, InteractType action);
        /// <summary>
        /// 当前用户是否执行操作
        /// </summary>
        /// <param name="user"></param>
        /// <param name="itemType"></param>
        /// <param name="target"></param>
        /// <param name="action"></param>
        /// <returns></returns>
        public bool Has(int user, ModuleTargetType type, int target, InteractType action);
        /// <summary>
        /// 仅执行 action,不做取消操作
        /// </summary>
        /// <param name="user"></param>
        /// <param name="itemType"></param>
        /// <param name="target"></param>
        /// <param name="action"></param>
        /// <returns></returns>
        public bool Add(int user, ModuleTargetType type, int target, InteractType action);
        /// <summary>
        /// 判断当前用户执行了那一个操作
        /// </summary>
        /// <param name="user"></param>
        /// <param name="itemType"></param>
        /// <param name="target"></param>
        /// <param name="onlyAction"></param>
        /// <returns></returns>
        public InteractType Get(int user, ModuleTargetType type, int target, InteractType[] onlyAction);
        /// <summary>
        /// 判断当前用户执行了那一个操作
        /// </summary>
        /// <param name="user"></param>
        /// <param name="itemType"></param>
        /// <param name="target"></param>
        /// <returns></returns>
        public InteractType Get(int user, ModuleTargetType type, int target);
        /// <summary>
        /// 取消或执行某个操作
        /// </summary>
        /// <param name="user"></param>
        /// <param name="itemType"></param>
        /// <param name="target"></param>
        /// <param name="action"></param>
        /// <returns></returns>
        public bool Toggle(int user, ModuleTargetType type, int target, InteractType action);
        public RecordToggleType Update(int user, ModuleTargetType type, int target, InteractType action);
        public RecordToggleType Update(int user, ModuleTargetType type, int target, InteractType action, InteractType[] searchAction);
    }
}
