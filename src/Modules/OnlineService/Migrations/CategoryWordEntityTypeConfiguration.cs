using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NetDream.Modules.OnlineService.Entities;

namespace NetDream.Modules.OnlineService.Migrations
{
    public class CategoryWordEntityTypeConfiguration : IEntityTypeConfiguration<CategoryWordEntity>
    {
        public void Configure(EntityTypeBuilder<CategoryWordEntity> builder)
        {
            builder.ToTable("service_category_word", table => table.HasComment(""));
            builder.HasKey(i => i.Id);
            builder.Property(table => table.Id).HasColumnName("id");
            builder.Property(table => table.Content).HasColumnName("content");
            builder.Property(table => table.CatId).HasColumnName("cat_id");
        }
    }
}
