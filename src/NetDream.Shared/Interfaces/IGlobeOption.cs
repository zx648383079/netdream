using NetDream.Shared.Interfaces.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetDream.Shared.Interfaces
{
    public interface IGlobeOption
    {
        public T? Get<T>(string key);


        public void AddGroup(string groupName, IEnumerable<IOptionConfigureItem> items);
        public void AddGroup(string groupName, Func<IEnumerable<IOptionConfigureItem>> cb);
        public void InsertOrUpdate(string key, object data, string name);
    }
}
