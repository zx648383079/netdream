using NetDream.Core.Models;
using System.Collections.Generic;

namespace NetDream.Core.Interfaces
{
    public interface IUserStatistics
    {
        public IEnumerable<StatisticsItem> Subtotal(int user);
    }
}
