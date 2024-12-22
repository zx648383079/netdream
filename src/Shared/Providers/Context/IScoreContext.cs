using Microsoft.EntityFrameworkCore;
using NetDream.Shared.Providers.Entities;

namespace NetDream.Shared.Providers.Context
{
    public interface ISoreContext
    {

        public DbSet<ScoreLogEntity> ScoreLogs { get; set; }

        public int SaveChanges();
    }
}
