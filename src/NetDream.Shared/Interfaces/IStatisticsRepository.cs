using System.Collections.Generic;

namespace NetDream.Shared.Interfaces
{
    public interface IStatisticsRepository
    {
        public IDictionary<string, int> Subtotal();
    }
}
