using NetDream.Core.Interfaces.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetDream.Core.Interfaces
{
    public interface IGlobeOption
    {
        public T? Get<T>(string key);


        public void AddGroup(string groupName, IEnumerable<IOptionConfigureItem> items);
        public void AddGroup(string groupName, Func<IEnumerable<IOptionConfigureItem>> cb);
    }
}
