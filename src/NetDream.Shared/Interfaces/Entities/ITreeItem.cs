using System.Collections;
using System.Collections.Generic;

namespace NetDream.Shared.Interfaces.Entities
{
    public interface ITreeLinkItem
    {
        public int Id { get; }
        public int ParentId { get; }
    }
    public interface ILevelItem : ITreeLinkItem
    {
        public int Level { get; set; }
    }
    public interface ITreeBaseItem<T> : ILevelItem where T : IEnumerable
    {
        public T Children { get; set; }
    }
    public interface ITreeItem: ITreeBaseItem<IList<ITreeItem>>
    {
    }

    public interface ITreeGroupItem : ITreeBaseItem<ITreeGroup>
    {
    }

    public interface ITreeGroup: IDictionary<int, ITreeGroupItem>
    {
    }
}
