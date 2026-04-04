using NetDream.Shared.Interfaces;
using System.Collections.Generic;

namespace NetDream.Shared.Models
{
    public class TreeGroup: Dictionary<int, ITreeGroupItem>, ITreeGroup
    {
        public void Add(ITreeGroupItem item)
        {
            TryAdd(item.Id, item);
        }
    }
}
