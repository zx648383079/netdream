using NetDream.Shared.Models;
using System.Collections.Generic;

namespace NetDream.Shared.Interfaces
{
    public interface IUserStatistics
    {
        public IEnumerable<StatisticsItem> Subtotal(int user);
    }
}
