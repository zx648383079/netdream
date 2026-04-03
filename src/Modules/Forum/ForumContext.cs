using Microsoft.EntityFrameworkCore;
using NetDream.Modules.Forum.Entities;
using NetDream.Modules.Forum.Migrations;

namespace NetDream.Modules.Forum
{
    public class ForumContext(DbContextOptions<ForumContext> options): DbContext(options)
    {
        public DbSet<ForumClassifyEntity> ForumClassifies {get; set; }
        public DbSet<ForumEntity> Forums {get; set; }
        public DbSet<ForumModeratorEntity> ForumModerators {get; set; }
        public DbSet<ThreadEntity> Threads {get; set; }
        public DbSet<ThreadLogEntity> ThreadLogs {get; set; }
        public DbSet<ThreadPostEntity> ThreadPosts {get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new ForumClassifyEntityTypeConfiguration());
            modelBuilder.ApplyConfiguration(new ForumEntityTypeConfiguration());
            modelBuilder.ApplyConfiguration(new ForumModeratorEntityTypeConfiguration());
            modelBuilder.ApplyConfiguration(new ThreadEntityTypeConfiguration());
            modelBuilder.ApplyConfiguration(new ThreadLogEntityTypeConfiguration());
            modelBuilder.ApplyConfiguration(new ThreadPostEntityTypeConfiguration());
            base.OnModelCreating(modelBuilder);
        }
    }
}
