using Microsoft.EntityFrameworkCore;
using NetDream.Shared.Providers.Entities;

namespace NetDream.Shared.Providers.Context
{
    public interface ICollectContext
    {

        public DbSet<CollectLogEntity> Collects { get; set; }

        public int SaveChanges();
    }
}
