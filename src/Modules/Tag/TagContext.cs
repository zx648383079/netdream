using Microsoft.EntityFrameworkCore;
using NetDream.Modules.Tag.Entities;
using NetDream.Modules.Tag.Migrations;

namespace NetDream.Modules.Tag
{
    public class TagContext(DbContextOptions<TagContext> options) : DbContext(options)
    {

        public DbSet<TagEntity> Tags { get; set; }
        public DbSet<TagLinkEntity> TagLinks { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new TagEntityTypeConfiguration());
            modelBuilder.ApplyConfiguration(new TagLinkEntityTypeConfiguration());
            base.OnModelCreating(modelBuilder);
        }
    }
}
