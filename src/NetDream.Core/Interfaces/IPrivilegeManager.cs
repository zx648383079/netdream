using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetDream.Core.Interfaces
{
    public interface IPrivilegeManager
    {
        /// <summary>
        /// 注册权限
        /// </summary>
        /// <param name="data"></param>
        public void AddPermission(IDictionary<string, string> data);
        /// <summary>
        /// 注册权限
        /// </summary>
        /// <param name="name"></param>
        /// <param name="label"></param>
        public void AddPermission(string name, string label);
        /// <summary>
        /// 注册角色
        /// </summary>
        /// <param name="name"></param>
        /// <param name="label"></param>
        public void AddRole(string name, string label);
        public void AddRole(string name, string label, IDictionary<string, string> permissionItems);
    }
}
