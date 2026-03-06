using Microsoft.EntityFrameworkCore;
using NetDream.Modules.Team.Entities;
using NetDream.Modules.Team.Migrations;

namespace NetDream.Modules.Team
{
    public class TeamContext(DbContextOptions<TeamContext> options) : DbContext(options)
    {

        public DbSet<TeamEntity> Teams { get; set; }
        public DbSet<TeamUserEntity> TeamUsers { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new TeamEntityTypeConfiguration());
            modelBuilder.ApplyConfiguration(new TeamUserEntityTypeConfiguration());
            base.OnModelCreating(modelBuilder);
        }
    }
}
