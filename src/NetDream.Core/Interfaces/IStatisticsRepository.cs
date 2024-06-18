using System.Collections.Generic;

namespace NetDream.Core.Interfaces
{
    public interface IStatisticsRepository
    {
        public IDictionary<string, int> Subtotal();
    }
}
