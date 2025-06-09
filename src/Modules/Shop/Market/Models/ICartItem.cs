using System.Collections.Generic;

namespace NetDream.Modules.Shop.Market.Models
{
    public interface ICartSource
    {

        public ICartItem[] Load();
        public void Save(ICartItem[] items);
    }

    

    

  
}
