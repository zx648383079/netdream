using System.Collections.Generic;

namespace NetDream.Modules.Shop.Market
{
    public interface ICartGroupItem
    {
        public string Name { get; }
        public IStoreLabel? Store { get; }

        public IList<ICartItem> Items { get; }
    }
}
