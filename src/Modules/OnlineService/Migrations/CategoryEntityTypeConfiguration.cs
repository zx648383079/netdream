using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NetDream.Modules.OnlineService.Entities;

namespace NetDream.Modules.OnlineService.Migrations
{
    public class CategoryEntityTypeConfiguration : IEntityTypeConfiguration<CategoryEntity>
    {
        public void Configure(EntityTypeBuilder<CategoryEntity> builder)
        {
            builder.ToTable("service_category", table => table.HasComment(""));
            builder.HasKey("id");
            builder.Property(table => table.Id).HasColumnName("id");
            builder.Property(table => table.Name).HasColumnName("name");

            builder.HasMany(p => p.Items)
                .WithOne(b => b.Category)
                .HasPrincipalKey(p => p.Id)
                .HasForeignKey(b => b.CatId);

            builder.HasMany(p => p.Words)
                .WithOne(b => b.Category)
                .HasPrincipalKey(p => p.Id)
                .HasForeignKey(b => b.CatId);
        }
    }
}
