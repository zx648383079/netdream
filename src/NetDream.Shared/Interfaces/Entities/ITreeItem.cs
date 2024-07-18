using System.Collections;
using System.Collections.Generic;

namespace NetDream.Shared.Interfaces.Entities
{
    public interface ITreeBaseItem<T> where T : IEnumerable
    {
        public int Id { get; }
        public int ParentId { get; }

        public T Children { get; set; }

        public int Level { get; set; }
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
