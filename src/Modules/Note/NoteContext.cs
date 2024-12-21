using Microsoft.EntityFrameworkCore;
using NetDream.Modules.Note.Entities;
using NetDream.Modules.Note.Migrations;

namespace NetDream.Modules.Note
{
    public class NoteContext(DbContextOptions<NoteContext> options): DbContext(options)
    {
        public DbSet<NoteEntity> Notes {get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new NoteEntityTypeConfiguration());
            base.OnModelCreating(modelBuilder);
        }
    }
}
